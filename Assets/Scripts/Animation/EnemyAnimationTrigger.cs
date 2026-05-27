using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EnemyAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private void AttackOver()
    {
        enemy.Controller.AttackOver();
    }

    private void AttackTrigger()
    {
        enemy.Controller.AttackTrigger();
    }

}
