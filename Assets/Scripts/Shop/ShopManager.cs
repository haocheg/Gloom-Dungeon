using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class ShopManager : MonoBehaviour
{
    private static ShopManager instance;
    public static ShopManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<ShopManager>();
            if (instance == null)
                instance = gameObject.AddComponent<ShopManager>();
            DontDestroyOnLoad(instance.gameObject);
        }
    }

}
