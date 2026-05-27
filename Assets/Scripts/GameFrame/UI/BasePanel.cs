using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public abstract class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 用于存储所有要用的UI控件，注意：不存储默认名字的控件
    /// </summary>
    protected Dictionary<string, UIBehaviour> controlDic = new();

    private static List<string> defaultNameList = new List<string>() { "Image",
    "Text (TMP)", "Text (Legacy)", "RawImage", "Panel", "Toggle", "Background",
    "Checkmark", "Label", "Slider", "Fill", "Handle", "Scrollbar", "Scrollbar Vertical",
    "Scroll View", "Viewport", "Scrollbar Horizontal", "Button", "Dropdown",
    "Arrow", "Template", "Item", "Item Background", "Item Checkmark", "Item Label",
    "InputField (TMP)", "Placeholder", "Text", "Canvas", "EventSystem", "Button (Legacy)",
    "Dropdown (Legacy)", "InputField (Legacy)"};

    protected virtual void Awake()
    {

    }

    /// <summary>
    /// 获取子对象中的控件，注意：会出现两种以上控件在同一对象的情况
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected void GetChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; i++)
        {
            if (!controlDic.ContainsKey(controls[i].name))
            {
                if (!defaultNameList.Contains(controls[i].name))
                    controlDic.Add(controls[i].gameObject.name, controls[i]);
            }
        }
    }

    /// <summary>
    /// 获取指定名字的指定类型组件，注意：组件的对象同名时请换类型在调用
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="name">组件名</param>
    /// <returns></returns>
    public T GetControl<T>(string name) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(name))
        {
            T control = controlDic[name] as T;
            if (control == null)
            {
                Debug.LogError($"不存在名字为{name}的{typeof(T)}组件");
                return null;
            }
            return control;
        }
        else
        {
            GetChildrenControl<T>();
            if (!controlDic.ContainsKey(name))
            {
                Debug.LogError($"不存在{name}组件");
                return null;
            }
            else
            {
                return controlDic[name] as T;
            }
        }
    }

    /// <summary>
    /// 不需要传值的控件
    /// </summary>
    /// <typeparam name="T">控件类型</typeparam>
    /// <param name="name">控件名</param>
    /// <param name="listener">监听函数</param>
    protected void AddListener<T>(string name, UnityAction listener) where T : UIBehaviour
    {
        T control = GetControl<T>(name);
        if (control is Button)
        {
            (control as Button).onClick.AddListener(listener);
        }
    }

    /// <summary>
    /// 需要传值的控件
    /// </summary>
    /// <typeparam name="T">控件的类型</typeparam>
    /// <typeparam name="K">需要传值的类型</typeparam>
    /// <param name="name">控件名</param>
    /// <param name="listener">监听函数</param>
    protected void AddListener<T, K>(string name, UnityAction<K> listener) where T : UIBehaviour
    {
        T control = GetControl<T>(name);
        if (control is Slider)
        {
            (control as Slider).onValueChanged.AddListener((listener as UnityAction<float>));
        }
        else if (control is Toggle)
        {
            (control as Toggle).onValueChanged.AddListener((listener as UnityAction<bool>));
        }
        else if (control is Dropdown)
        {
            (control as Dropdown).onValueChanged.AddListener((listener as UnityAction<int>));
        }
        else if (control is InputField)
        {
            (control as InputField).onValueChanged.AddListener((listener as UnityAction<string>));
        }
        else if (control is Scrollbar)
        {
            (control as Scrollbar).onValueChanged.AddListener((listener as UnityAction<float>));
        }
        else if (control is ScrollRect)
        {
            (control as ScrollRect).onValueChanged.AddListener((listener as UnityAction<Vector2>));
        }
    }

    public abstract void ShowMe();

    public abstract void HideMe();

}
