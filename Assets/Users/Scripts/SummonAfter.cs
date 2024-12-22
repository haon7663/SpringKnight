using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SummonAfter : MonoBehaviour
{
    [SerializeField] bool playOnUpdate;
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    public GameObject afterImagePrf;
    [SerializeField] float afterDelay;
    float afterTimer = 1;

    IObjectPool<AfterImage> pool;

    void Awake()
    {
        pool = new ObjectPool<AfterImage>(CreateAfterImage, OnGetAfterImage, OnReleaseAfterImage, OnDestroyAfterImage);
    }

    void Update()
    {
        if (!playOnUpdate) return;

        afterTimer += Time.deltaTime;
        if (afterTimer > afterDelay)
        {
            SummonAfterImage();
            afterTimer = 0;
        }
    }

    public void SummonAfterImage()
    {
        AfterImage afterImage = pool.Get();
        afterImage.Get(m_SpriteRenderer.sprite);
        afterImage.transform.position = transform.position;
        afterImage.transform.localScale = transform.localScale;
        afterImage.GetComponent<SpriteRenderer>().flipX = m_SpriteRenderer.flipX;
    }

    AfterImage CreateAfterImage()
    {
        AfterImage afterImage = Instantiate(afterImagePrf).GetComponent<AfterImage>();
        afterImage.SetManagedPool(pool);
        return afterImage;
    }

    void OnGetAfterImage(AfterImage afterImage)
    {
        afterImage.gameObject.SetActive(true);
    }
    void OnReleaseAfterImage(AfterImage afterImage)
    {
        afterImage.gameObject.SetActive(false);
    }
    void OnDestroyAfterImage(AfterImage afterImage)
    {
        Destroy(afterImage.gameObject);
    }
}
