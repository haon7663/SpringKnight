using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberGoblin : EnemyDestroy
{
    public GameObject BombParticle;

    public override void AfterDestroy()
    {
        Instantiate(BombParticle, transform.position, Quaternion.identity);
        Movement player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        player.SetMultiSpeed(2);
        player.count += 3;
        UIManager.Inst.SetPower(player.count);
    }
}
