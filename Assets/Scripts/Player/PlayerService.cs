using UnityEngine;

public class PlayerService : Singleton<PlayerService>
{
    private PlayerService()
    {
    }

    private PlayerController Controller
    {
        get { return PlayerController.Instance; }
    }

    public Player CurrentPlayer
    {
        get { return Controller == null ? null : Controller.player; }
    }

    public Transform PlayerTransform
    {
        get { return CurrentPlayer == null ? null : CurrentPlayer.transform; }
    }

    public void InitPlayer()
    {
        Controller?.InitPlayer();
    }

    public void Teleport(Vector3 position)
    {
        Controller?.TeleportPlayer(position);
    }

    public void KeepAcrossScenes()
    {
        Controller?.KeepPlayer();
    }

    public void ShowOrHide(bool isShow)
    {
        Controller?.ShowOrHide(isShow);
    }

    public void EnableInput(bool isEnable)
    {
        Controller?.EnableOrDisableInput(isEnable);
    }

    public void Respawn()
    {
        Controller?.Respawn();
    }

    public void Restart()
    {
        Controller?.Restart();
    }

    public int GetHealthValue()
    {
        return Controller == null ? 0 : Controller.GetHealthValue();
    }

    public void ApplySkill(PlayerStats stats)
    {
        Controller?.GetSkill(stats);
    }

    public void OnDamageTaken()
    {
        Controller?.GetDamage();
    }

    public void Die()
    {
        Controller?.Die();
    }

    public void AttackTrigger()
    {
        Controller?.AttackTrigger();
    }

    public void DashOver()
    {
        Controller?.DashOver();
    }
}