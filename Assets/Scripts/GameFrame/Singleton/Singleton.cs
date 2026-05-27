using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;


/// <summary>
/// Title:管理类基类
/// Description: 安全模式下子类必须有一个私有无参构造函数
/// </summary>
public abstract class Singleton<T> where T : class//, new()
{
    //安全加锁的对象
    protected static readonly object lockObject = new object();

    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        //instance = new T();
                        #region 安全模式
                        ConstructorInfo info = typeof(T).GetConstructor(
                            BindingFlags.Instance |
                            BindingFlags.NonPublic,
                            null,
                            Type.EmptyTypes,
                            null);
                        if (info != null)
                            instance = info.Invoke(null) as T;
                        else
                            Debug.LogError("没有获得对应的无参构造函数");
                        #endregion
                    }
                }
            }
            return instance;
        }
    }

    public static T GetInstance()
    {
        if (instance == null)
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    //instance = new T();
                    #region 安全模式
                    ConstructorInfo info = typeof(T).GetConstructor(
                        BindingFlags.Instance |
                        BindingFlags.NonPublic,
                        null,
                        Type.EmptyTypes,
                        null);
                    if (info != null)
                        instance = info.Invoke(null) as T;
                    else
                        Debug.LogError("没有获得对应的无参构造函数");
                    #endregion
                }
            }
        }
        return instance;
    }

}
