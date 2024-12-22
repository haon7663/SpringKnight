using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySprite : MonoBehaviour
{
    EnemyBundle m_EnemyBundle;
    SpriteRenderer m_SpriteRenderer;
    SpriteOutline m_SpriteOutline;
    Transform playerTransform;

    [Header("Materials")]
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material whiteMaterial;
    [SerializeField] Material rayMaterial;

    public bool doFlip;
    public bool doReverse;

    public float hitTimer;
    float rayTimer;

    void Awake()
    {
        if (TryGetComponent(out SpriteRenderer sprite))
            m_SpriteRenderer = sprite;
        if (TryGetComponent(out SpriteOutline outline))
            m_SpriteOutline = outline;
        if (TryGetComponent(out EnemyBundle bundle))
            m_EnemyBundle = bundle;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        StartCoroutine(GracePeriod());
        m_SpriteRenderer.sortingOrder = m_EnemyBundle.defence.index;
    }

    void LateUpdate()
    {
        if(doFlip)
        {
            m_SpriteRenderer.flipX = playerTransform.position.x - transform.position.x > 0;
            if (doReverse)
                m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;
        }

        SetMaterial();
    }

    void SetMaterial()
    {
        bool isHit = rayTimer > 0;
        m_SpriteRenderer.material = isHit ? rayMaterial : hitTimer > 0 ? whiteMaterial : defaultMaterial;
        m_SpriteOutline.enabled = isHit;

        if (hitTimer > 0)
            hitTimer -= Time.deltaTime;
        if (rayTimer > 0)
            rayTimer -= Time.deltaTime;
    }

    public void HitRay()
    {
        rayTimer = Time.deltaTime;
    }

    public IEnumerator GracePeriod()
    {
        gameObject.layer = 9;
        for (int i = 0; i < 5; i++)
        {
            m_SpriteRenderer.color = new Color(1, 1, 1, m_SpriteRenderer.color.a == 0.4f ? 0.8f : 0.4f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        m_SpriteRenderer.color = new Color(1, 1, 1, 1f);
        gameObject.layer = 7;

        GameManager.Inst.SpawnEvent(m_EnemyBundle.defence);
        yield return null;
    }

    public IEnumerator DeathFade()
    {
        hitTimer = 0.1f;
        doFlip = false;

        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        m_SpriteRenderer.sortingOrder = 0;

        m_SpriteRenderer.DOColor(Color.gray, 0.1f).SetEase(Ease.Linear);
        yield return YieldInstructionCache.WaitForSeconds(1.4f);

        m_SpriteRenderer.DOFade(0, 0.5f).SetEase(Ease.Linear);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        yield return null;
    }
}
