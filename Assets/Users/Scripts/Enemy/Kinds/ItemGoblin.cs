using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGoblin : EnemyDestroy
{
    public override void AfterDestroy()
    {
        Instantiate(SummonManager.Inst.GetRandomItem(), transform.position, Quaternion.identity);
    }
}
