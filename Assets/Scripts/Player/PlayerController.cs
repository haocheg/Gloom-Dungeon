using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Title:
/// Description:
/// </summary>
public partial class PlayerController : EntityController
{
    public Player player;

    [Header("输入相关")]
    private Vector2 sliderInput;
    public PlayerInputSet input { get; private set; }
    public Vector2 moveInput
    {
        get { return input.Player.Movement.ReadValue<Vector2>(); }
    }

    [Header("速度相关")]
    public Vector2 moveVelocity;
    [SerializeField] private float dashSpeed = 20.0f;
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float jumpForce = 14.0f;
    [SerializeField]
    private float[] attackVelocity =
    {
        4.0f,
        3.0f,
        4.0f
    };
    private float gravityScale;

    [Header("状态相关")]
    private StateMachine stateMachine;
    private Dictionary<Player.PlayerState, EntityState> playerStates;

    [Header("时间相关")]
    private int dashTime = 1;
    private float dashCoolTime = 0.5f;
    private float ignorePlatformTimer = 0.0f;

    [Header("Jump")]
    [SerializeField] private int maxAirJumpCount = 1;
    private int remainingAirJumpCount;

    [Header("组件")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private EntityCombat combat;

    [Header("碰撞检测")]
    [SerializeField] private float groundCheckDis = 0.28f;
    [SerializeField] private float wallCheckDis = 0.50f;
    public Transform primaryWallCheck;
    public Transform secondaryWallCheck;
    public Transform thirdWallCheck;
    public Transform groundCheck;
    public Collider2D platformCollider;
    public bool isOnGround { get; private set; }
    public bool isTouchWall { get; private set; }
    public bool grabEdge { get; private set; }
    public bool isIgnoringPlatform { get; private set; }

    [Header("方向相关")]
    private static bool facingRight = true;
    public float facingDir
    {
        get { return facingRight ? 1.0f : -1.0f; }
    }
    private Vector2 wallJumpDir = new Vector2(10.0f, 12.0f);

    private LayerMask groundLayer;
    private LayerMask platformLayer;

    public bool canDash { get; private set; } = false;
    public bool canDoubleJump { get; private set; } = false;
    public bool canWallSlider { get; private set; } = false;
    public bool canEdgeHang { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();
        InitLayers();
        InitStateMachine();
        CacheComponents();
    }

    private void Start()
    {
        input = player.input;
    }

    public void Update()
    {
        moveVelocity = rb.velocity;
        stateMachine.UpdateActiveState();
        HandleDetec();
        dashCoolTime -= Time.deltaTime;
    }

    public void FixedUpdate()
    {
        RefreshPlatformPenetrate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDis));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDis * facingDir, 0));
        Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDis * facingDir, 0));
        Gizmos.DrawLine(thirdWallCheck.position, thirdWallCheck.position + new Vector3(wallCheckDis * facingDir, 0));
    }

    private void InitLayers()
    {
        groundLayer = LayerMask.GetMask("Ground");
        platformLayer = LayerMask.GetMask("Platform");
    }

    private void CacheComponents()
    {
        rb = player.rb;
        animator = player.animator;
        gravityScale = rb.gravityScale;
    }

    private void InitStateMachine()
    {
        stateMachine = new StateMachine();
        playerStates = new Dictionary<Player.PlayerState, EntityState>()
        {
            {Player.PlayerState.IdleState, new PlayerIdleState(stateMachine, "Idle") },
            {Player.PlayerState.MoveState, new PlayerMoveState(stateMachine, "Move") },
            {Player.PlayerState.JumpState, new PlayerJumpState(stateMachine, "Jump") },
            {Player.PlayerState.FallState, new PlayerFallState(stateMachine, "Fall") },
            {Player.PlayerState.WallSlider, new PlayerWallSliderState(stateMachine, "WallSlider") },
            {Player.PlayerState.WallJump, new PlayerWallJumpState(stateMachine, "WallJump") },
            {Player.PlayerState.DashState, new PlayerDashState(stateMachine, "DashState") },
            {Player.PlayerState.BasicAttack, new PlayerBasicAttackState(stateMachine, "BasicAttack") },
            {Player.PlayerState.JumpAttack, new PlayerJumpAttackState(stateMachine, "JumpAttack") },
            {Player.PlayerState.EdgeHang, new PlayerEdgeHangState(stateMachine, "EdgeHang") },
            {Player.PlayerState.DeadState, new PlayerDeadState(stateMachine, "Dead") },
        };

        stateMachine.Init(playerStates[Player.PlayerState.IdleState], player);
    }

    public void ChangeState(Player.PlayerState playerState)
    {
        if (playerStates.ContainsKey(playerState))
            stateMachine.ChangeState(playerStates[playerState]);
    }
}
