using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class WayPoint : MonoBehaviour
{
    [SerializeField] private string tranToScene;
    [SerializeField] private RespawnType waypointType;
    [SerializeField] private RespawnType connWaypoint;
    [SerializeField] private bool canBeTrigger = true;
    [SerializeField] private Transform respawnPoint;

    public RespawnType GetRespawnType()
    {
        return waypointType;
    }

    public Vector3 GetRespawnPoint()
    {
        return respawnPoint == null ? transform.position : respawnPoint.position;
    }


    private void OnValidate()
    {
        gameObject.name = "WayPoint - " + waypointType.ToString() + " - " + tranToScene;

        if (waypointType == RespawnType.Enter)
        {
            connWaypoint = RespawnType.Exit;
            canBeTrigger = false;
        }
        if (waypointType == RespawnType.Exit)
            connWaypoint = RespawnType.Enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeTrigger || collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        LevelManager.Instance.ChangeScene(tranToScene, connWaypoint);
    }

}

public enum RespawnType
{
    Enter,
    Exit,
    Death,
    New,
    None
}