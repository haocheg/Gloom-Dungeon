using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EntityCombat : MonoBehaviour
{
    [SerializeField] private Collider2D[] collider2Ds;
    [Header("Ä¿±ê¼ì²â")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask targetLayer;

    public bool PerformAttack(int damage)
    {
        Collider2D[] targets = GetColliders();
        if (targets == null || targets.Length == 0)
            return false;
        foreach (Collider2D collider in targets)
        {
            Entity target = collider.GetComponent<Entity>();
            if (target == null) continue;
            EntityStatsManager.Instance.TakeDamage(target, damage, transform);
        }
        return true;
    }

    private Collider2D[] GetColliders()
    {
        collider2Ds = Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, targetLayer);
        return collider2Ds;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }

}
