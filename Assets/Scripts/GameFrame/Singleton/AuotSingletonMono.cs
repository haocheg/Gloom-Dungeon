using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class AuotSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
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
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void DestroyGameObject()
    {
        if (instance != null)
            Destroy(instance.gameObject);
    }

}
