using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class Player : Entity
{
    public enum PlayerState
    {
        IdleState,
        MoveState,
        JumpState,
        FallState,
        WallSlider,
        WallJump,
        DashState,
        BasicAttack,
        JumpAttack,
        EdgeHang,
        DeadState,
    };

    public PlayerInputSet input { get; private set; }



    protected override void Awake()
    {
        input = new PlayerInputSet();
        DontDestroyOnLoad(gameObject);
        PlayerInputMgr.Instance.Init(this);
        StatsManager = new PlayerStatsMgr(this);
    }

    private void OnEnable()
    {
        input.Enable();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }


    private void OnDisable()
    {
        input.Disable();
    }

}
