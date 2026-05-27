using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EntityController : MonoBehaviour
{
    protected static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<PlayerController>();
            if (instance == null)
                instance = gameObject.AddComponent<PlayerController>();
            //DontDestroyOnLoad(instance.gameObject);
        }
    }

    public static void DestroyGameObject()
    {
        if (instance != null)
            Destroy(instance.gameObject);
    }

}
