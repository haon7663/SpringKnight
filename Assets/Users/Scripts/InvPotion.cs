using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvPotion : PrizeInformation
{
    bool isCalled;
    public override void UseItem()
    {
        if (isCalled) return;

        isCalled = true;
        PlayerState.Inst.SetItem(true);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, 1);
    }
}
