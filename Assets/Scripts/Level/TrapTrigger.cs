using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class TrapTrigger : MonoBehaviour
{
    public TrapType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleTrap(collision);
    }

    private void HandleTrap(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Entity entity))
            return;

        int damage = 0;
        switch (type)
        {
            case TrapType.Sprike:
                damage = entity.Stats.maxHp;
                break;
            default:
                break;
        }
        EntityStatsManager.Instance.TakeDamage(entity, damage, transform);
    }

}

[System.Serializable]
public enum TrapType
{
    Sprike,
}