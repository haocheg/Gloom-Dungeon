using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EventCenter : Singleton<EventCenter>
{
    private Dictionary<E_TheEvent, EventInfoBase> eventDic = new();
    private EventCenter() { }

    public void EventTrigger(E_TheEvent e_event)
    {
        if (eventDic.ContainsKey(e_event))
        {
            EventInfo info = eventDic[e_event] as EventInfo;
            info.EventTrigger();
        }
    }

    public void EventTrigger<T>(E_TheEvent e_event, T t)
    {
        if (eventDic.ContainsKey(e_event))
        {
            EventInfo<T> info = eventDic[e_event] as EventInfo<T>;
            info.EventTrigger(t);
        }
    }

    public void AddEventListener(E_TheEvent e_event, UnityAction action)
    {
        if (eventDic.ContainsKey(e_event))
        {
            EventInfo info = eventDic[e_event] as EventInfo;
            info.AddEventListener(action);
        }
        else
        {
            EventInfo info = new EventInfo(action);
            eventDic.Add(e_event, info);
        }
    }

    public void AddEventListener<T>(E_TheEvent e_event, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(e_event))
        {
            EventInfo<T> info = eventDic[e_event] as EventInfo<T>;
            info.AddEventListener(action);
        }
        else
        {
            EventInfo<T> info = new EventInfo<T>(action);
            eventDic.Add(e_event, info);
        }
    }

    public void RemoveEventListener(E_TheEvent e_event, UnityAction action)
    {
        if (eventDic.ContainsKey(e_event))
        {
            (eventDic[e_event] as EventInfo).RemoveEventListener(action);
        }
    }

    public void RemoveEventListener<T>(E_TheEvent e_event, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(e_event))
        {
            (eventDic[e_event] as EventInfo<T>).RemoveEventListener(action);
        }
    }

    public void Clear()
    {
        eventDic.Clear();
    }

    public void Clear(E_TheEvent e_event)
    {
        if (eventDic.ContainsKey(e_event))
            eventDic.Remove(e_event);
    }
}

public abstract class EventInfoBase
{

}

public class EventInfo : EventInfoBase
{
    public event UnityAction Action;

    public EventInfo(UnityAction action)
    {
        Action += action;
    }

    public void AddEventListener(UnityAction action)
    {
        Action += action;
    }

    public void RemoveEventListener(UnityAction action)
    {
        Action -= action;
    }

    public void EventTrigger()
    {
        Action?.Invoke();
    }

}

public class EventInfo<T> : EventInfoBase
{
    public event UnityAction<T> Action;

    public EventInfo(UnityAction<T> action)
    {
        Action += action;
    }

    public void AddEventListener(UnityAction<T> action)
    {
        Action += action;
    }

    public void RemoveEventListener(UnityAction<T> action)
    {
        Action -= action;
    }

    public void EventTrigger(T t)
    {
        Action?.Invoke(t);
    }

}

