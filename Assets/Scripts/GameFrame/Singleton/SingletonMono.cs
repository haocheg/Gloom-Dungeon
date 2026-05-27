using UnityEngine;


/// <summary>
/// Title: 继承Mono的单例模式基类
/// Description: 需要手动挂载到对象上
/// </summary>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject(typeof(T).ToString());
                instance = go.AddComponent<T>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    public static void DestroyGameObject()
    {
        if (instance != null)
            Destroy(instance.gameObject);
    }

    /// <summary>
    /// 可重写的Awake方法
    /// </summary>
    protected virtual void Awake()
    {
    }

}
