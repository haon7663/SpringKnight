using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerSpriteRenderer : MonoBehaviour
{
    [Header("SpriteRenderers")]
    [SerializeField] SpriteRenderer m_SpriteRenderer;

    [Header("Components")]
    [SerializeField] Movement m_Movement;
    [SerializeField] Collison m_Collison;
    [SerializeField] Rigidbody2D m_Rigidbody2D;
    [SerializeField] SummonAfter m_SummonAfter;

    [Header("Materials")]
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material whiteMaterial;
 
    float hitTimer, afterTimer;

    void Update()
    {
        if (GameManager.Inst.isSetting) return;

        SetMaterial();

        if (gameObject.layer == 8) return;

        SetFlip();
        if (m_Movement.count > 0)
        {
            var bounceCount = m_Movement.bouncedCount;
            afterTimer += Time.deltaTime;
            if (afterTimer > 0.05f)
            {
                if (bounceCount > 4)
                {
                    m_SummonAfter.SummonAfterImage();
                    afterTimer = 0;
                }
            }
        }
    }

    void SetFlip()
    {
        if (m_Movement.count <= 0 && m_Collison.onCollision)
        {
            if (m_Collison.onRight)
                m_SpriteRenderer.flipX = true;
            else if (m_Collison.onLeft)
                m_SpriteRenderer.flipX = false;
        }
        else if (!m_Movement.isAttacking)
        {
            if (m_Rigidbody2D.velocity.x > 0)
                m_SpriteRenderer.flipX = false;
            else if (m_Rigidbody2D.velocity.x < 0)
                m_SpriteRenderer.flipX = true;
        }
    }
    public void SetTransformFlip(Transform target, Transform selfTarget = null)
    {
        m_SpriteRenderer.flipX = (selfTarget ? selfTarget.position.x : transform.position.x) > target.position.x;
    }
    public void SetVectorFlip(Vector2 target, Vector2 selfTarget, bool isFlipX)
    {
        if(isFlipX)
            m_SpriteRenderer.flipX = selfTarget.x > target.x;
    }
    public void SetColor(Color color, float time)
    {
        m_SpriteRenderer.DOColor(color, time);
    }
    void SetMaterial()
    {
        m_SpriteRenderer.material = hitTimer > 0 ? whiteMaterial : defaultMaterial;
        if (hitTimer > 0) hitTimer -= Time.deltaTime;
    }

    public IEnumerator GracePerioding()
    {
        hitTimer = 0.1f;
        for (int i = 0; i < 5; i++)
        {
            m_SpriteRenderer.color = new Color(1, 1, 1, m_SpriteRenderer.color.a == 0.4f ? 0.8f : 0.4f);
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
        m_SpriteRenderer.color = new Color(1, 1, 1, 1f);

        yield return null;
    }
}
