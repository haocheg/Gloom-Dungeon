using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class Entity : MonoBehaviour
{
    [Header("×é¼þ")]
    public Animator animator;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Collider2D cld;
    public EntityVFX VFX;
    public EntityStats Stats;
    public bool isDead = false;
    public StatsManager StatsManager { get; protected set; }

    protected virtual void Awake()
    {

    }


    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public virtual void Die()
    {
        isDead = true;
        Debug.Log("Entity ËÀÍö");
    }


}
