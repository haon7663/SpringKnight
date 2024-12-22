using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRigidbody : MonoBehaviour
{
    public Vector2 startPos;

    EnemyBundle m_EnemyBundle;
    Rigidbody2D m_Rigidbody2D;

    void Awake()
    {
        if (TryGetComponent(out EnemyBundle bundle))
            m_EnemyBundle = bundle;
        if (TryGetComponent(out Rigidbody2D rigid))
            m_Rigidbody2D = rigid;

        startPos = transform.position;
    }

    public IEnumerator BouncedOff(Transform target, float distance)
    {
        if (TryGetComponent(out PeriodicMovement periodic))
            periodic.enabled = false;

        gameObject.layer = 9;

        m_Rigidbody2D.mass = 1;
        m_Rigidbody2D.drag = 0;
        Vector3 direction = (target.position - transform.position).normalized;
        m_Rigidbody2D.velocity = new Vector2(direction.x, direction.y) * -distance;

        yield return StartCoroutine(m_EnemyBundle.sprite.DeathFade());

        Destroy(gameObject);
    }
}
