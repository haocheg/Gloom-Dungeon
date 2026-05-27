using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class CheckPoint : MonoBehaviour
{
    [SerializeField] private string currScene;
    [SerializeField] private bool canBeTrigger = true;
    [SerializeField] private bool isPlayerInside = false;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private int pointIndex;

    private void OnValidate()
    {
        gameObject.name = "CheckPoint - " + currScene + " - " + pointIndex;
    }

    public Vector3 GetRespawnPoint()
    {
        return respawnPoint == null ? transform.position : respawnPoint.position;
    }

    public int GetPointIndex()
    {
        return pointIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeTrigger && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (canBeTrigger && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            isPlayerInside = false;
    }

    private void Update()
    {
        if (canBeTrigger && isPlayerInside)
        {
            if (PlayerInputMgr.Instance.ListenInput(PlayerInputMgr.PlayerInputType.Save))
            {
                LevelManager.Instance.SaveCheckPoint(pointIndex);
            }
        }
    }

}

