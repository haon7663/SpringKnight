using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPrince : EnemyDestroy
{
    public override void AfterDestroy()
    {
        Movement player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        player.TakeMirror();
    }
}