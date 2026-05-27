public partial class PlayerController
{
    public void AttackTrigger()
    {
        bool hasHit = combat.PerformAttack(player.Stats.Attack);
        string sfxName = hasHit ? "playerAttackHit" : "playerAttackMiss";
        AudioManager.Instance.PlaySFX(sfxName);
    }

    public void GetDamage()
    {
        player.VFX.OnHurtVFX();
    }
}