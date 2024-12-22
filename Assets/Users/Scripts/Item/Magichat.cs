using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magichat : PrizeInformation
{
    public override void UseItem()
    {
        Movement.Inst.bouncedCount *= 2;
        Destroy(gameObject);
    }
}
