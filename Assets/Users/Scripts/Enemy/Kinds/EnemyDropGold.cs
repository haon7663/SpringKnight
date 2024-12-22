using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropGold : EnemyNonDestroy
{
    [SerializeField] int gold;
    public override bool AfterDamaged()
    {
        GameManager.Inst.AddGold(gold);
        SaveManager.Inst.Save();

        gold /= 2;
        if (gold < 100) gold = 100;

        return false;
    }
}
