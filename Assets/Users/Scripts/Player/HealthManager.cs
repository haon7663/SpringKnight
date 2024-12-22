using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] Transform player;

    [Header("Stats")]
    public int maxhp;
    public int curhp;

    [Header("Health UI")]
    [SerializeField] RectTransform healthPrefab;
    [SerializeField] Transform helathParent;
    [SerializeField] Image[] healths;
    [SerializeField] Image[] healthParents;

    [SerializeField] Sprite disabledHeart;
    [SerializeField] Sprite abledHeart;
    [SerializeField] Sprite berserkHeart;

    void LateUpdate()
    {
        helathParent.position = (player.position + new Vector3(0, player.position.y > 4.3f ? -1 : 1));
    }
    public void SetHealth(int maxhp)
    {
        this.maxhp = maxhp;
        Array.Resize(ref healthParents, maxhp);
        Array.Resize(ref healths, maxhp);
        for (int i = 0; i < maxhp; i++)
        {
            var health = healths[i] == null ? Instantiate(healthPrefab, helathParent) : healths[i].transform.parent.GetComponent<RectTransform>();
            health.localPosition = new Vector2(((maxhp-1) * -30) + (60 * i), 0);
            healthParents[i] = health.GetComponent<Image>();
            healths[i] = health.GetChild(0).GetComponent<Image>();
        }
    }
    public void OnFade(bool isFadeIn)
    {
        if (healths.Length == 0)
            SetHealth(maxhp);

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < maxhp; i++)
        {
            sequence.Join(healths[i].DOFade(isFadeIn ? 1 : 0, 0.25f))
                .Join(healthParents[i].DOFade(isFadeIn ? 1 : 0, 0.25f));
        }
        sequence.AppendInterval(0.26f);
        if (curhp <= 0)
        {
            for (int i = 0; i < maxhp; i++)
            {
                sequence.Join(healths[i].DOFade(0, 0.25f))
                    .Join(healthParents[i].DOFade(0, 0.25f));
            }
        }
    }
    public void OnDamage(int damage = -1)
    {
        ManageHealth(damage);
        StartCoroutine(CameraEffect.Inst.OnDamage());
    }
    public void OnHealth(int health = 0)
    {
        ManageHealth(health);
        Debug.Log("체력 " + health + " 회복");
    }

    public void ManageHealth(int value)
    {
        curhp += value;
        if (curhp > maxhp) curhp = maxhp;
        for (int i = 0; i < maxhp; i++)
        {
            healths[i].sprite = disabledHeart;
        }
        for (int i = 0; i < curhp; i++)
        {
            healths[i].sprite = abledHeart;
            if (curhp == 1 && GameManager.Inst.isBerserker)
                healths[i].sprite = berserkHeart;
        }
        OnFade(true);
    }
}
