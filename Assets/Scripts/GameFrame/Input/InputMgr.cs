using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class InputMgr : Singleton<InputMgr>
{

    private Dictionary<E_TheEvent, InputInfo> inputDic = new Dictionary<E_TheEvent, InputInfo>();
    private InputInfo nowInputInfo;
    private UnityAction<InputInfo> anyKeyPressCallBack;
    private bool isGetInfo;

    private InputMgr()
    {
        MonoMgr.Instance.AddUpdateListener(InputUpdate);
    }

    public void GetInputInfo(UnityAction<InputInfo> callBack)
    {
        anyKeyPressCallBack = callBack;
        MonoMgr.Instance.RemoveUpdateListener(InputUpdate);
        MonoMgr.Instance.StartCoroutine(AtGetInputInfo());
    }

    private IEnumerator AtGetInputInfo()
    {
        yield return null;
        isGetInfo = true;
        MonoMgr.Instance.AddUpdateListener(GetInputInfoUpdate);
    }

    private void GetInputInfoUpdate()
    {
        if (isGetInfo)
        {
            if (Input.anyKeyDown)
            {
                InputInfo info = null;
                Array keys = Enum.GetValues(typeof(KeyCode));
                foreach (KeyCode key in keys)
                {
                    if (Input.GetKeyDown(key))
                    {
                        info = new InputInfo(InputInfo.E_InputType.Down, key);
                        break;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    if (Input.GetMouseButtonDown(i))
                    {
                        info = new InputInfo(InputInfo.E_InputType.Down, i);
                        break;
                    }
                }
                anyKeyPressCallBack?.Invoke(info);
                anyKeyPressCallBack = null;
                MonoMgr.Instance.RemoveUpdateListener(GetInputInfoUpdate);
                MonoMgr.Instance.AddUpdateListener(InputUpdate);
                isGetInfo = false;
            }
        }
    }

    private void InputUpdate()
    {
        foreach (E_TheEvent theEvent in inputDic.Keys)
        {
            nowInputInfo = inputDic[theEvent];
            switch (nowInputInfo.KOrM)
            {
                case InputInfo.E_KeyOrMouse.Key:
                    switch (nowInputInfo.type)
                    {
                        case InputInfo.E_InputType.Down:
                            if (Input.GetKeyDown(nowInputInfo.key))
                                EventCenter.Instance.EventTrigger(theEvent);
                            break;
                        case InputInfo.E_InputType.Up:
                            if (Input.GetKeyUp(nowInputInfo.key))
                                EventCenter.Instance.EventTrigger(theEvent);
                            break;
                        case InputInfo.E_InputType.Hold:
                            if (Input.GetKey(nowInputInfo.key))
                                EventCenter.Instance.EventTrigger(theEvent);
                            break;
                        default:
                            break;
                    }
                    break;
                case InputInfo.E_KeyOrMouse.Mouse:
                    switch (nowInputInfo.type)
                    {
                        case InputInfo.E_InputType.Down:
                            if (Input.GetMouseButtonDown(nowInputInfo.mouseID))
                                EventCenter.Instance.EventTrigger(theEvent);
                            break;
                        case InputInfo.E_InputType.Up:
                            if (Input.GetMouseButtonUp(nowInputInfo.mouseID))
                                EventCenter.Instance.EventTrigger(theEvent);
                            break;
                        case InputInfo.E_InputType.Hold:
                            if (Input.GetMouseButton(nowInputInfo.mouseID))
                                EventCenter.Instance.EventTrigger(theEvent);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void AddOrChangeKeyInputInfo(E_TheEvent theEvent, KeyCode key, InputInfo.E_InputType type)
    {
        if (inputDic.ContainsKey(theEvent))
        {
            inputDic[theEvent].KOrM = InputInfo.E_KeyOrMouse.Key;
            inputDic[theEvent].key = key;
            inputDic[theEvent].type = type;
        }
        else
        {
            inputDic.Add(theEvent, new InputInfo(type, key));
        }
    }

    public void AddOrChangeMouseInputInfo(E_TheEvent theEvent, int mouseID, InputInfo.E_InputType type)
    {
        if (inputDic.ContainsKey(theEvent))
        {
            inputDic[theEvent].KOrM = InputInfo.E_KeyOrMouse.Mouse;
            inputDic[theEvent].mouseID = mouseID;
            inputDic[theEvent].type = type;
        }
        else
        {
            inputDic.Add(theEvent, new InputInfo(type, mouseID));
        }
    }

    public void RemoveInputInfo(E_TheEvent theEvent)
    {
        if (inputDic.ContainsKey(theEvent))
            inputDic.Remove(theEvent);
    }

}
