using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<ItemSpawnPoint> points;
    [SerializeField] private GameObject prefab;
    [SerializeField] private bool CanSpawn;

    private void Awake()
    {
        EventCenter.Instance.AddEventListener(E_TheEvent.E_ItemSpawn, SpawnItem);
    }

    public void SpawnItem()
    {
        foreach (ItemSpawnPoint point in points)
        {
            float start = point.startPoint.position.x;
            float end = point.endPoint.position.x;
            float y = point.startPoint.position.y;
            for (float i = start; i <= end; i += point.duration)
            {
                Instantiate(prefab, new Vector3(i, y), Quaternion.identity);
            }
        }
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener(E_TheEvent.E_ItemSpawn, SpawnItem);
    }
}

[System.Serializable]
public class ItemSpawnPoint
{
    public Transform startPoint;
    public Transform endPoint;
    public float duration;
}