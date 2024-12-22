using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyRunAway : EnemyDashSign
{
    EnemyBundle m_EnemyBundle;
    Transform playerTransform;

    void Start()
    {
        m_EnemyBundle = GetComponent<EnemyBundle>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public override void AfterDash()
    {
        float maxDistance = -100;
        var tiles = TileManager.Inst.tiles;
        var result = 0;
        for (int i = 0; i < 6; i++)
        {
            var index = SummonManager.Inst.FindTileIndex(false);
            var offset = (Vector2)playerTransform.position - tiles[index].position;
            var sqrLen = offset.sqrMagnitude;
            if (sqrLen > maxDistance)
            {
                maxDistance = sqrLen;
                result = index;
            }
        }

        DOTween.To(() => m_EnemyBundle.rigid.startPos, x => m_EnemyBundle.rigid.startPos = x, tiles[result].position, 0.6f);
        TileManager.Inst.TakeTile(m_EnemyBundle.defence.index, false);
        m_EnemyBundle.defence.index = result;
        TileManager.Inst.TakeTile(result, true);
    }
}
