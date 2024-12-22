using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collison : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;

    [Space]
    [Header("Collisions")]
    [SerializeField] BoxCollider2D m_BoxCollider2D;
    public Vector3[] rayOffset;
    public bool onCollision;
    public bool onUp;
    public bool onDown;
    public bool onRight;
    public bool onLeft;

    [Space]
    [Header("Values")]
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset, topOffset;

    float timer = 0;
    int hitCount = 0;

    private void Update()
    {
        onUp = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, collisionRadius, groundLayer);
        onDown = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeft = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onCollision = onUp || onDown || onRight || onLeft;
        timer += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Movement.Inst.isIgnoreCollison || GameManager.Inst.onDeath) return;

        if (collision.transform.CompareTag("Enemy"))
        {
            StartCoroutine(CapsuleAble());
            Movement.Inst.CrashEnemy(collision);
        }

        else if (collision.transform.CompareTag("Wall"))
            Movement.Inst.CrashWall(collision);

        else if (collision.transform.CompareTag("Damage"))
        {
            StartCoroutine(Movement.Inst.Hit(collision));
        }
    }

    /*void OnCollisionStay2D(Collision2D collision)
    {
        if (Movement.Inst.isIgnoreCollison || GameManager.Inst.onDeath) return;

        if (collision.transform.CompareTag("Enemy"))
            Movement.Inst.CrashEnemy(collision);
    }*/

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position + topOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }

    public void SetBoxScale(bool isLarge)
    {
        if(isLarge)
        {
            m_BoxCollider2D.size = new Vector2(0.5f, 1.15f);
        }
        else
        {
            m_BoxCollider2D.size = new Vector2(0.5f, 1.15f) * 0.95f;
        }
    }

    public IEnumerator CapsuleAble()
    {
        m_BoxCollider2D.enabled = false;
        yield return YieldInstructionCache.WaitForFixedUpdate;
        yield return YieldInstructionCache.WaitForFixedUpdate;
        m_BoxCollider2D.enabled = true;
    }
}
