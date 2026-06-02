using System.Collections;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerBasicAttackState : PlayerState
{
    public PlayerBasicAttackState(StateMachine machine, string stateName) : base(machine, stateName)
    {
        EventCenter.Instance.AddEventListener<bool>(E_TheEvent.E_PlayerAttackOver, AttackOverCheck);
    }

    private float attackDurTimer;
    private int comboIndex = 1;
    private float comboDur = 1.0f;
    private float lastAttackTime = 0;
    private float attackDur = .2f;
    private Coroutine comboCo;
    private bool isOver = false;


    public override void EnterState()
    {
        base.EnterState();
        Reset();
        Controller.PlayAnim("BasicAttack", true);
        Controller.PlayAnim("BasicAttackIndex", comboIndex);
        attackDurTimer = attackDur;
    }

    private void Reset()
    {
        isOver = false;
        if (lastAttackTime + comboDur < Time.time)
            comboIndex = 1;
        else if (comboIndex > 3)
            comboIndex = 1;
    }

    public override void ExitState()
    {
        base.ExitState();
        Controller.PlayAnim("BasicAttack", false);
        comboIndex++;
        lastAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        AttackMove();
        if (InputMgr.ListenPlayerInput(PlayerInputMgr.PlayerInputType.Dash) && Controller.CanDash())
        {
            Controller.ChangeState(Player.PlayerState.DashState);
            return;
        }



        InputMgr.RecordInput(PlayerInputMgr.PlayerInputType.BasicAttack, 0.3f);

        if (isOver)
        {
            if (!Controller.isOnGround || Controller.Fall())
            {
                Controller.ChangeState(Player.PlayerState.FallState);
                return;
            }

            if (InputMgr.CheckInput(PlayerInputMgr.PlayerInputType.BasicAttack) && comboIndex < 3)
            {
                Controller.PlayAnim("BasicAttack", false);
                EnterAttackWait();
            }
            else
                Controller.ChangeState(Player.PlayerState.IdleState);
        }
    }

    private void AttackMove()
    {
        attackDurTimer -= Time.deltaTime;
        if (attackDurTimer >= 0)
        {
            Controller.AttackMove(comboIndex);
        }
        else
        {
            Controller.SetVelocity(0, 0);
        }
    }

    private void EnterAttackWait()
    {
        if (comboCo != null)
            MonoMgr.Instance.StopCoroutine(comboCo);
        comboCo = MonoMgr.Instance.StartCoroutine(AttackWaitDelayCo());
    }

    private IEnumerator AttackWaitDelayCo()
    {
        yield return new WaitForEndOfFrame();
        Controller.ChangeState(Player.PlayerState.BasicAttack);
    }

    private void AttackOverCheck(bool isOver)
    {
        this.isOver = isOver;
    }

}
