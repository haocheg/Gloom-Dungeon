using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class Terminus : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventCenter.Instance.EventTrigger(E_TheEvent.E_GameOver);
    }

}
