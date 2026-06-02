using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerInputMgr : Singleton<PlayerInputMgr>
{
    public enum PlayerInputType
    {
        Move,
        Jump,
        WallJump,
        Dash,
        BasicAttack,
        WallSlider,
        DownPlatform,
        Interact,
    }

    public enum UIInputType
    {
        Interact,
    }

    private struct PreInput
    {
        public PlayerInputType InputType;
        public float Time;
        public float duration;

        public PreInput(PlayerInputType type, float time, float dur)
        {
            this.InputType = type;
            this.Time = time;
            this.duration = dur;
        }

    }

    PlayerInputSet input;
    Player player;
    private Vector2 moveInput;
    private Vector2 sliderInput;

    private Dictionary<PlayerInputType, PreInput> inputMap = new Dictionary<PlayerInputType, PreInput>();
    private List<PlayerInputType> disList = new List<PlayerInputType>();

    private PlayerInputMgr()
    {
        MonoMgr.Instance.AddUpdateListener(Update);
    }

    public void Init(Player player)
    {
        this.player = player;
        input = player.input;
    }

    public bool ListenPlayerInput(PlayerInputType type)
    {
        switch (type)
        {
            case PlayerInputType.Move:
                moveInput = input.Player.Movement.ReadValue<Vector2>();
                if (moveInput.x == 0)
                {
                    return false;
                }
                return true;
            case PlayerInputType.Jump:
                if (input.Player.Jump.WasPressedThisFrame())
                {
                    return true;
                }
                return false;
            case PlayerInputType.WallJump:
                if (input.Player.Jump.WasPressedThisFrame())
                {
                    return true;
                }
                return false;
            case PlayerInputType.Dash:
                if (input.Player.Dash.WasPressedThisFrame())
                {
                    return true;
                }
                return false;
            case PlayerInputType.BasicAttack:
                if (input.Player.Attack.WasPressedThisFrame())
                    return true;
                return false;
            case PlayerInputType.WallSlider:
                sliderInput = input.Player.WallSlider.ReadValue<Vector2>();
                if (sliderInput.y < 0)
                    return true;
                return false;
            case PlayerInputType.DownPlatform:
                if (input.Player.Down.WasPressedThisFrame())
                {
                    return true;
                }
                return false;
            case PlayerInputType.Interact:
                if (input.Player.Interact.WasPressedThisFrame())
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    public bool ListenUIInput(UIInputType type)
    {
        switch (type)
        {
            case UIInputType.Interact:
                if (input.UI.DialogueInteract.WasPressedThisFrame())
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    public Vector2 GetMoveInput()
    {
        moveInput = input.Player.Movement.ReadValue<Vector2>();
        return moveInput;
    }

    public void RecordInput(PlayerInputType type, float dur = 0.1f)
    {
        if (ListenPlayerInput(type))
        {
            inputMap[type] = new PreInput(type, Time.time, dur);
        }
    }

    public bool CheckInput(PlayerInputType type)
    {
        if (inputMap.ContainsKey(type))
        {
            PreInput pInput = inputMap[type];
            if (pInput.Time + pInput.duration > Time.time)
                return true;
        }
        return false;
    }

    private void Update()
    {
        if (inputMap.Count > 0)
        {
            foreach (PlayerInputType type in inputMap.Keys)
            {
                PreInput pInput = inputMap[type];
                if (pInput.Time + pInput.duration < Time.time)
                {
                    disList.Add(type);
                }
            }
        }
        if (disList.Count > 0)
        {
            for (int i = 0; i < disList.Count; i++)
            {
                inputMap.Remove(disList[i]);
            }
            disList.Clear();
        }


    }

}
