using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class Item : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int count;
    [SerializeField] private bool canBeTrigger = false;

    private void Start()
    {
        animator.SetBool("Idle", true);
        StartCoroutine(OnTriggerTimer());
    }

    IEnumerator OnTriggerTimer()
    {
        gameObject.layer = LayerMask.NameToLayer("ItemNoTrigger");
        yield return new WaitForSeconds(0.5f);
        gameObject.layer = LayerMask.NameToLayer("Item");
        canBeTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canBeTrigger)
        {
            Destroy(gameObject);
            EventCenter.Instance.EventTrigger<int>(E_TheEvent.E_AddScore, count);
        }
    }

}
