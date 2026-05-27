using Cinemachine;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private PolygonCollider2D confinerCollider;
    [SerializeField] private CinemachineConfiner2D confiner;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (virtualCamera == null || PlayerService.Instance.PlayerTransform == null)
        {
            Debug.LogError("PlayerCamera missing virtual camera or player.");
            return;
        }

        Transform playerTransform = PlayerService.Instance.PlayerTransform;
        virtualCamera.Follow = playerTransform;
        virtualCamera.LookAt = playerTransform;
        confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
        GetCollider();
    }

    public void GetCollider()
    {
        GameObject confinerObject = GameObject.FindGameObjectWithTag("Confiner");
        if (confinerObject == null || !confinerObject.TryGetComponent(out confinerCollider))
        {
            Debug.LogWarning("Confiner collider not found in current scene.");
            return;
        }

        if (confiner == null && virtualCamera != null)
            confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();

        if (confiner != null)
            confiner.m_BoundingShape2D = confinerCollider;
    }

}
