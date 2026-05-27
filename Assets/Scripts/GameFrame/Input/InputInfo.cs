using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class InputInfo
{
    public enum E_KeyOrMouse
    {
        Key,
        Mouse
    }

    public enum E_InputType
    {
        Down,
        Up,
        Hold,
    }
    public E_KeyOrMouse KOrM;
    public E_InputType type;
    public KeyCode key;
    public int mouseID;

    public InputInfo(E_InputType type, KeyCode keycode)
    {
        KOrM = E_KeyOrMouse.Key;
        this.type = type;
        this.key = keycode;
    }

    public InputInfo(E_InputType type, int mouseID)
    {
        KOrM = E_KeyOrMouse.Mouse;
        this.type = type;
        this.mouseID = mouseID;
    }

}
