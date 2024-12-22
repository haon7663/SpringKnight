using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{
    private SpriteRenderer mSpriteRenderer;

    private void Start()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        transform.DOMoveY(transform.position.y + 1, 1f).SetEase(Ease.OutCirc);
        mSpriteRenderer.DOFade(0, 1f).SetEase(Ease.InQuart);
        Destroy(gameObject, 1.01f);
    }
}
