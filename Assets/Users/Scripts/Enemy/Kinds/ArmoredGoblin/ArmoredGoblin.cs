using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArmoredGoblin : MonoBehaviour
{
    [SerializeField] EnemyBundle m_EnemyBundle;
    [SerializeField] Animator m_Animator;
    public Animator m_StoneAnimator;

    bool isAttack;
    public IEnumerator Attack(Vector2 ranPos)
    {
        if (isAttack) yield break;

        gameObject.layer = 9;
        isAttack = true;
        var setTimeScale = Movement.Inst.m_SetTimeScale;
        setTimeScale.isBossTime = true;
        CinemachineManager.Inst.highlightTransform = transform;
        CinemachineManager.Inst.isHighLight = true;
        Time.fixedDeltaTime = 0.005f / 100;
        setTimeScale.setTime = 0.01f;
        Time.timeScale = 0.01f;

        m_Animator.SetTrigger("move");
        DOTween.To(() => m_EnemyBundle.rigid.startPos, x => m_EnemyBundle.rigid.startPos = x, ranPos, 1.2f).SetEase(Ease.OutQuint).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(Movement.Inst.TrasnformHit(transform));

        m_Animator.SetTrigger("attack");
        m_StoneAnimator.SetTrigger("attack");

        Time.fixedDeltaTime = 0.005f;
        Time.timeScale = 0.2f;
        setTimeScale.setTime = 0.2f;
        setTimeScale.isBossTime = false;
        CinemachineManager.Inst.isHighLight = false;

        yield return new WaitForSeconds(0.5f);
        DOTween.To(() => m_EnemyBundle.rigid.startPos, x => m_EnemyBundle.rigid.startPos = x, Vector2.zero, 0.3f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.3f);

        gameObject.layer = 7;
        isAttack = false;
    }
}
