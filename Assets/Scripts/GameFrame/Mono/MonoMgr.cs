using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Title:公共Mono模块管理器
/// Description:
/// </summary>
public class MonoMgr : SingletonMono<MonoMgr>
{
    private event UnityAction updateEvent;
    private event UnityAction fixedUpdateEvent;
    private event UnityAction lateUpdateEvent;

    private List<DelayTask> delayTasks = new List<DelayTask>();

    private void Update()
    {
        updateEvent?.Invoke();
        
        // 处理延迟任务
        for (int i = delayTasks.Count - 1; i >= 0; i--)
        {
            delayTasks[i].remainingTime -= Time.deltaTime;
            if (delayTasks[i].remainingTime <= 0)
            {
                delayTasks[i].callback?.Invoke();
                delayTasks.RemoveAt(i);
            }
        }
    }

    private void FixedUpdate()
    {
        fixedUpdateEvent?.Invoke();
    }

    private void LateUpdate()
    {
        lateUpdateEvent?.Invoke();
    }

    /// <summary>
    /// 添加Update帧更新函数
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }

    /// <summary>
    /// 移除Update帧更新函数
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action)
    {
        updateEvent -= action;
    }

    /// <summary>
    /// 添加FixedUpdate帧更新函数
    /// </summary>
    /// <param name="action"></param>
    public void AddFixedUpdateListener(UnityAction action)
    {
        fixedUpdateEvent += action;
    }

    /// <summary>
    /// 移除FixedUpdate帧更新函数
    /// </summary>
    /// <param name="action"></param>
    public void RemoveFixedUpdateListener(UnityAction action)
    {
        fixedUpdateEvent -= action;
    }

    /// <summary>
    /// 添加LateUpdate帧更新函数
    /// </summary>
    /// <param name="action"></param>
    public void AddLateUpdateListener(UnityAction action)
    {
        lateUpdateEvent += action;
    }

    /// <summary>
    /// 移除LateUpdate帧更新函数
    /// </summary>
    /// <param name="action"></param>
    public void RemoveLateUpdateListener(UnityAction action)
    {
        lateUpdateEvent -= action;
    }

    /// <summary>
    /// 添加延迟任务
    /// </summary>
    /// <param name="delay">延迟时间（秒）</param>
    /// <param name="callback">回调函数</param>
    public void AddDelayTask(float delay, UnityAction callback)
    {
        DelayTask task = new DelayTask
        {
            remainingTime = delay,
            callback = callback
        };
        delayTasks.Add(task);
    }

    /// <summary>
    /// 清除所有延迟任务
    /// </summary>
    public void ClearDelayTasks()
    {
        delayTasks.Clear();
    }

    private class DelayTask
    {
        public float remainingTime;
        public UnityAction callback;
    }
}
