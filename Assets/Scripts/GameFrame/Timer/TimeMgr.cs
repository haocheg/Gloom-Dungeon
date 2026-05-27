using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class TimeMgr : Singleton<TimeMgr>
{
    //用于记录当前的唯一ID
    private static int TIMER_ID = 0;

    private Dictionary<int, TimerItem> timerDic =
        new Dictionary<int, TimerItem>();
    private Dictionary<int, TimerItem> realTimerDic =
        new Dictionary<int, TimerItem>();
    //待移除的列表
    private List<TimerItem> timerList = new List<TimerItem>();

    private Coroutine timer;
    private Coroutine realTimer;
    //间隔计时时间
    public static float intervalTime = 0.1f;

    private TimeMgr()
    {

    }

    public void StartTimer()
    {
        if (timer != null)
            return;
        timer = MonoMgr.Instance.StartCoroutine(AtStartTimer(false, timerDic));
    }

    public void StartRealTimer()
    {
        if (realTimer != null)
            return;
        realTimer = MonoMgr.Instance.StartCoroutine(AtStartTimer(true, realTimerDic));
    }

    public void Stop()
    {
        if (timer != null)
        {
            MonoMgr.Instance.StopCoroutine(timer);
            timer = null;
        }

        if (realTimer != null)
        {
            MonoMgr.Instance.StopCoroutine(realTimer);
            realTimer = null;
        }
    }

    WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(intervalTime);
    WaitForSeconds waitForSeconds = new WaitForSeconds(intervalTime);

    IEnumerator AtStartTimer(bool isRealTime, Dictionary<int, TimerItem> dic)
    {
        while (dic.Count > 0)
        {
            if (isRealTime)
                yield return waitForSecondsRealtime;
            else
                yield return waitForSeconds;
            foreach (TimerItem item in dic.Values)
            {
                if (!item.isRunning) continue;

                if (item.callBack != null)
                {
                    item.intervalTime -= (int)(intervalTime * 1000);
                    if (item.intervalTime <= 0)
                    {
                        item.callBack.Invoke();
                        item.RestIntervalTime();
                    }
                }
                item.allTime -= (int)(intervalTime * 1000);
                if (item.allTime <= 0)
                {
                    item.overCallBack?.Invoke();
                    timerList.Add(item);
                }
            }

            for (int i = 0; i < timerList.Count; i++)
            {
                dic.Remove(timerList[i].ID);
                PoolManager.Instance.PushPoolData(timerList[i]);
            }
            timerList.Clear();
        }

        if (isRealTime)
            realTimer = null;
        else
            timer = null;
    }

    /// <summary>
    /// 创建单个计时器
    /// </summary>
    /// <param name="isRealTime">是否受Time.timeScale影响</param>
    /// <param name="allTime">总时间</param>
    /// <param name="overCallBack">计时结束回调</param>
    /// <param name="intervalTime">间隔时间</param>
    /// <param name="callBack">间隔调用回调</param>
    /// <returns></returns>
    public int CreateTimer(int allTime, UnityAction overCallBack,
        int intervalTime = 0, UnityAction callBack = null, bool isRealTime = false)
    {
        int keyId = ++TIMER_ID;
        TimerItem item = PoolManager.Instance.GetPoolData<TimerItem>();
        item.InitInfo(keyId, allTime, overCallBack, intervalTime, callBack);
        if (isRealTime)
        {
            realTimerDic.Add(keyId, item);
            if (realTimer == null)
                StartRealTimer();
        }
        else
        {
            timerDic.Add(keyId, item);
            if (timer == null)
                StartTimer();
        }
        return keyId;
    }

    public void RemoveTimer(int timerId)
    {
        if (timerDic.ContainsKey(timerId))
        {
            PoolManager.Instance.PushPoolData(timerDic[timerId]);
            timerDic.Remove(timerId);
        }
        else if (realTimerDic.ContainsKey(timerId))
        {
            PoolManager.Instance.PushPoolData(realTimerDic[timerId]);
            realTimerDic.Remove(timerId);
        }
    }

    /// <summary>
    /// 重置单个计时器
    /// </summary>
    /// <param name="timerId"></param>
    public void ResetTimer(int timerId)
    {
        if (timerDic.ContainsKey(timerId))
        {
            timerDic[timerId].ResetInfo();
        }
        else if (realTimerDic.ContainsKey(timerId))
        {
            realTimerDic[timerId].ResetInfo();
        }
    }

    /// <summary>
    /// 开启单个计时器
    /// </summary>
    /// <param name="keyID"></param>
    public void StartTimer(int keyID)
    {
        if (timerDic.ContainsKey(keyID))
        {
            timerDic[keyID].isRunning = true;
        }
        else if (realTimerDic.ContainsKey(keyID))
        {
            realTimerDic[keyID].isRunning = true;
        }
    }

    /// <summary>
    /// 停止单个计时器
    /// </summary>
    /// <param name="keyID"></param>
    public void StopTimer(int keyID)
    {
        if (timerDic.ContainsKey(keyID))
        {
            timerDic[keyID].isRunning = false;
        }
        else if (realTimerDic.ContainsKey(keyID))
        {
            realTimerDic[keyID].isRunning = false;
        }
    }

}
