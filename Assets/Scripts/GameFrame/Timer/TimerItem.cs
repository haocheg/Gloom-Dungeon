using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class TimerItem : IPoolData
{
    private bool _needAutoCleanUp = false;
    public bool needAutoCleanUp
    {
        get => _needAutoCleanUp;
    }
    //唯一ID
    public int ID;
    //计时结束后调用的回调
    public UnityAction overCallBack;
    //间隔时间执行的回调
    public UnityAction callBack;
    //计时总时间 毫秒
    public int allTime;
    private int maxAllTime;
    //间隔时间 毫秒
    public int intervalTime;
    private int maxIntervalTime;
    //是否在进行计时
    public bool isRunning;


    /// <summary>
    /// 初始化计时器数据
    /// </summary>
    /// <param name="keyId">唯一ID</param>
    /// <param name="allTime">计时总时间</param>
    /// <param name="overCallBack">结束后回调</param>
    /// <param name="intervalTime">间隔执行时间</param>
    /// <param name="callBack">间隔执行回调</param>
    public void InitInfo(int keyId, int allTime, UnityAction overCallBack, int intervalTime = 0, UnityAction callBack = null)
    {
        this.ID = keyId;
        this.maxAllTime = this.allTime = allTime;
        this.overCallBack = overCallBack;
        this.maxIntervalTime = this.intervalTime = intervalTime;
        this.callBack = callBack;
        this.isRunning = true;
    }

    /// <summary>
    /// 重置计时器
    /// </summary>
    public void RestTimer()
    {
        this.allTime = this.maxAllTime;
        this.intervalTime = this.maxIntervalTime;
        this.isRunning = true;
    }

    public void RestIntervalTime()
    {
        this.intervalTime = this.maxIntervalTime;
    }


    public void ResetInfo()
    {
        overCallBack = null;
        callBack = null;
        isRunning = false;
    }
}
