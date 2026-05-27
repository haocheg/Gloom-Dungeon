using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class Platform : MonoBehaviour
{
    [SerializeField] private Vector3 pointA;
    [SerializeField] private Vector3 pointB;
    [SerializeField] private Vector3 targetPoint;
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        targetPoint = pointA;
        transform.position = pointA;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, pointB) < 0.1f) targetPoint = pointA;
        if (Vector2.Distance(transform.position, pointA) < 0.1f) targetPoint = pointB;
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pointA, pointB);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            PlayerService.Instance.KeepAcrossScenes();
        }
    }

}
