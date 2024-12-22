using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DefenceKnight : EnemyDashSign
{
    EnemyDefence m_EnemyDefence;

    public int maxPower;
    public int maxStack;

    int curStack;

    private void Start()
    {
        m_EnemyDefence = GetComponent<EnemyDefence>();
        maxPower = GameManager.Inst.maxPower;
    }
    public override void AfterDash()
    {
        if(m_EnemyDefence.defence < maxPower)
        {
            curStack++;
            if(curStack >= maxStack)
            {
                curStack = 0;

                m_EnemyDefence.defence++;
                m_EnemyDefence.SetDefence();

                transform.DOScale(transform.localScale*1.1f, 0.4f).SetEase(Ease.OutCubic);
            } 
        }
    }
}
