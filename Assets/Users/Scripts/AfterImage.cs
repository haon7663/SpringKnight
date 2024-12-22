using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class AfterImage : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [HideInInspector] public Sprite sprite;

    [SerializeField] Color StartColor;
    [SerializeField] Color EndColor;
    [SerializeField] float time;

    IObjectPool<AfterImage> managedPool;

    public void Get(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = StartColor;
        spriteRenderer.DOColor(EndColor, time);
        Invoke(nameof(Release), time);
    }

    public void SetManagedPool(IObjectPool<AfterImage> pool)
    {
        managedPool = pool;
    }

    public void Release()
    {
        managedPool.Release(this);
    }
}
