using System.Collections;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class EntityVFX : MonoBehaviour
{
    public Entity entity;
    private SpriteRenderer sr;
    private Material orgMaterial;
    private Coroutine onHurtVFXCo;

    [Header(" ‹…À–ßπ˚")]
    [SerializeField] private Material onHurtMaterial;
    [SerializeField] private float onHurtDur = .2f;


    private void Awake()
    {
        sr = entity.sr;
        orgMaterial = sr.material;
    }

    public void OnHurtVFX()
    {
        if (onHurtVFXCo != null)
            StopCoroutine(onHurtVFXCo);
        onHurtVFXCo = StartCoroutine(OnHurtVFXCo());
    }

    private IEnumerator OnHurtVFXCo()
    {
        sr.material = onHurtMaterial;
        yield return new WaitForSeconds(onHurtDur);

        sr.material = orgMaterial;
    }

}
