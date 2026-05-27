using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EnemyController : MonoBehaviour
{
    public Enemy enemy;
    public EntityCombat combat;
    public Transform target { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    private StateMachine stateMachine;
    private Dictionary<Enemy.EnemyState, EnemyState> stateDic;

    public bool thereHasGround { get; private set; }
    public bool isTouchWall { get; private set; }
    public bool isAttacking { get; private set; } = false;
    public bool canRetreat { get; private set; }

    [Header("速度相关")]
    [SerializeField] private float moveSpeed = 1.4f;
    [SerializeField] private float battleMoveSpeed = 3.0f;
    private float moveAnimSpeed
    {
        get
        {
            float v = moveSpeed / 1.4f;
            v = Mathf.Clamp(v, 0, 2);
            return v;
        }
    }

    [Header("攻击相关")]
    [SerializeField] private float attackDis = 2.0f;
    [SerializeField] private float minRetreatDis = 1.0f;
    public Vector2 retreatV;

    [Header("受击相关")]
    [SerializeField] private Vector2 knockbackPower;
    [SerializeField] private float knockbackDur;
    private bool isKnocked;
    private Coroutine knockbackCo;

    [Header("方向相关")]
    private bool facingRight = true;
    public float facingDir
    {
        get
        {
            return facingRight ? 1.0f : -1.0f;
        }
    }
    public float DirToTarget
    {
        get
        {
            if (target == null)
                return 0;
            return target.position.x > enemy.transform.position.x ? 1.0f : -1.0f;
        }
    }

    [Header("检测相关")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform secondaryGroundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float groundCheckDis;
    [SerializeField] private float wallCheckDis;
    [SerializeField] private float playerCheckDis;

    [Header("时间相关")]
    public float idleDur = 2.0f;
    public float battleTimeDur = 5.0f;
    public float lastTimeMeetTarget;
    public float inGameTime;

    private LayerMask groundLayer;
    private LayerMask platformLayer;
    private LayerMask playerLayer;
    private Enemy.EnemyState curState;

    private void Awake()
    {
        rb = enemy.rb;
        animator = enemy.animator;
        stateMachine = new StateMachine();
        stateDic = new Dictionary<Enemy.EnemyState, EnemyState>()
        {
            {Enemy.EnemyState.IdleState, new EnemyIdleState(enemy, stateMachine, "Idle")},
            {Enemy.EnemyState.MoveState, new EnemyMoveState(enemy, stateMachine, "Move")},
            {Enemy.EnemyState.AttackState, new EnemyAttackState(enemy, stateMachine, "Attack")},
            {Enemy.EnemyState.BattleState, new EnemyBattleState(enemy, stateMachine, "Battle")},
            {Enemy.EnemyState.DeadState, new EnemyDeadState(enemy, stateMachine, "Dead")},
        };

        stateMachine.Init(stateDic[Enemy.EnemyState.IdleState], enemy);
        groundLayer = LayerMask.GetMask("Ground");
        platformLayer = LayerMask.GetMask("Platform");
        playerLayer = LayerMask.GetMask("Player");
    }

    private void Start()
    {
        Respawn();
    }

    private void Update()
    {
        HandleDetec();
        stateMachine.UpdateActiveState();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDis));
        Gizmos.DrawLine(secondaryGroundCheck.position, secondaryGroundCheck.position + new Vector3(0, -groundCheckDis));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + new Vector3(wallCheckDis * facingDir, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(playerCheckDis * facingDir, 0));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(attackDis * facingDir, 0));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(minRetreatDis * facingDir, 0));

    }

    public void Move()
    {
        SetVelocity(moveSpeed * facingDir, rb.velocity.y);
        UpdateAnimParam("MoveSpeed", moveAnimSpeed);
    }

    public void Stop()
    {
        SetVelocity();
    }

    private void HandleDetec()
    {
        thereHasGround = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDis, groundLayer) ||
            Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDis, platformLayer);

        isTouchWall = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDis, groundLayer);

        if (Physics2D.Raycast(secondaryGroundCheck.position, Vector2.down, groundCheckDis, groundLayer) ||
            Physics2D.Raycast(secondaryGroundCheck.position, Vector2.down, groundCheckDis, platformLayer))
            canRetreat = true;
    }

    public void SetVelocity(float xMultiplier = 0, float yMultiplier = 0)
    {
        if (isKnocked) return;

        rb.velocity = new Vector2(xMultiplier, yMultiplier);
        if (rb.velocity.x > 3.0f && !facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < -3.0f && facingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        enemy.transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    public void ChangeState(Enemy.EnemyState enemyState)
    {
        if (stateDic.ContainsKey(enemyState))
        {
            stateMachine.ChangeState(stateDic[enemyState]);
            curState = enemyState;
        }
    }

    public void AttackOver()
    {
        this.isAttacking = false;
    }

    public void Attack()
    {
        isAttacking = true;
        if (facingDir != DirToTarget)
            Flip();
    }

    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDis, playerLayer | groundLayer);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;
        if (target == null)
            target = hit.transform;
        return hit;
    }

    public float DisToTarget()
    {
        if (target == null)
            return float.MaxValue;

        return Mathf.Abs(target.position.x - enemy.transform.position.x);
    }

    public bool InAttackRange()
    {
        return DisToTarget() <= attackDis;
    }

    public bool FindTarget()
    {
        if (PlayerDetection())
            lastTimeMeetTarget = Time.time;
        inGameTime = Time.time;
        if (inGameTime > lastTimeMeetTarget + battleTimeDur)
            return false;
        return true;
    }

    public void UpdateAnimParam(string animName, float param)
    {
        animator.SetFloat(animName, param);
    }


    public void MoveToTarget()
    {
        SetVelocity(battleMoveSpeed * DirToTarget, rb.velocity.y);
        float animSpeed = battleMoveSpeed / 1.4f;
        UpdateAnimParam("BattleMoveSpeed", animSpeed);
        UpdateAnimParam("xVelocity", rb.velocity.x);
    }

    public void Retreat()
    {
        if (!canRetreat)
            return;
        if (DisToTarget() < minRetreatDis)
            SetVelocity(retreatV.x * -DirToTarget, retreatV.y);
    }

    public void AttackTrigger()
    {
        combat.PerformAttack(enemy.Stats.Attack);
    }

    public void GetDamage(Transform transform)
    {
        enemy.VFX.OnHurtVFX();
        if (enemy.isDead) return;
        Knockback(transform);
        if (curState == Enemy.EnemyState.BattleState || curState == Enemy.EnemyState.AttackState)
            return;
        target = transform;
        lastTimeMeetTarget = Time.time;
        ChangeState(Enemy.EnemyState.BattleState);
    }

    public void Knockback(Transform damageDealer)
    {
        if (!isKnocked)
        {
            int dir = transform.position.x > damageDealer.position.x ? 1 : -1;
            Vector2 knock = knockbackPower;
            knock.x *= dir;
            GetKnockback(knock, knockbackDur);
        }
    }

    public void GetKnockback(Vector2 knock, float dur)
    {
        if (knockbackCo != null)
            StopCoroutine(knockbackCo);
        knockbackCo = StartCoroutine(KnockbackCo(knock, dur));
    }

    private IEnumerator KnockbackCo(Vector2 knock, float dur)
    {
        isKnocked = true;
        rb.velocity = knock;
        yield return new WaitForSeconds(dur);
        rb.velocity = Vector2.zero;
        isKnocked = false;
    }

    public void Death()
    {
        animator.enabled = false;
        enemy.cld.enabled = false;
        rb.velocity = new Vector2(rb.velocity.x, 20.0f);
        rb.gravityScale = 10.0f;
    }

    public void Respawn()
    {
        enemy.isDead = false;
        enemy.StatsManager.Respawn();
        ChangeState(Enemy.EnemyState.IdleState);
    }

    internal void Die()
    {
        if (enemy.isDead)
        {
            EventCenter.Instance.EventTrigger<Vector3>(E_TheEvent.E_IsEnemyDie, enemy.transform.position);
            ChangeState(Enemy.EnemyState.DeadState);
            return;
        }
    }
}
