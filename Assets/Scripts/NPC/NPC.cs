using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class NPC : MonoBehaviour
{
    [SerializeField] private int NPCID;
    [SerializeField] private Hint hint;
    private bool isPlayerNear;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision).gameObject.CompareTag("Player"))
        {
            hint.ShowHint();
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision).gameObject.CompareTag("Player"))
        {
            hint.HideHint();
            isPlayerNear = false;
        }
    }

    private void Update()
    {
        if (isPlayerNear)
        {
            if (PlayerInputMgr.Instance.ListenPlayerInput(PlayerInputMgr.PlayerInputType.Interact))
            {
                NPCManager.Instance.Dialogue(NPCID);
            }
        }
    }


}
