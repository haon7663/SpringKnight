using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoldenGoblin : MonoBehaviour
{
    EnemyBundle m_EnemyBundle;
    float timer;
    bool isCalled;

    void Start()
    {
        m_EnemyBundle = GetComponent<EnemyBundle>();

        gameObject.layer = 9;
        InvokeRepeating("RandomMovePosition", Random.Range(4, 12), 0.7f);
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 12.5f && !isCalled)
        {
            isCalled = true;
            CancelInvoke("RandomMovePosition");
            var randValue = Random.Range(25, 35);
            DOTween.To(() => m_EnemyBundle.rigid.startPos, x => m_EnemyBundle.rigid.startPos = x, new Vector2(Random.Range(0, 2) == 0 ? -randValue : randValue, Random.Range(-25, 35)), 3);
        }
    }
    public void RandomMovePosition()
    {
        var tiles = TileManager.Inst.tiles;
        var index = SummonManager.Inst.FindTileIndex(false);

        DOTween.To(() => m_EnemyBundle.rigid.startPos, x => m_EnemyBundle.rigid.startPos = x, tiles[index].position, 0.6f);
        TileManager.Inst.TakeTile(m_EnemyBundle.defence.index, false);
        m_EnemyBundle.defence.index = index;
        TileManager.Inst.TakeTile(index, true);
    }
}
