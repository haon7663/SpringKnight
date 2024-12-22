using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class SummonManager : MonoBehaviour
{
    public static SummonManager Inst { get; private set; }
    void Awake() => Inst = this;


    [Serializable]
    public struct EnemyPersent
    {
        public GameObject prefab;
        public int persent;
    }
    [Serializable]
    public struct SpawnPersent
    {
        public EnemyPersent[] enemyPersents;
    }
    public SpawnPersent[] stagePersents;
    public GameObject armoredGoblin;
    public GameObject armoredStone;
    public GameObject goldGoblin;

    public List<GameObject> enemyList;
    public GameObject[] itemPrfs;

    public Transform enemyBundle;
    public Transform itemBundle;
    public GameObject SummonEnemy()
    {
        var index = FindTileIndex();
        GameObject enemy = Instantiate(GetRandomEnemy(), TileManager.Inst.tiles[index].position, Quaternion.identity);
        enemy.transform.SetParent(enemyBundle);
        var scriptBundle = enemy.GetComponent<EnemyBundle>();
        scriptBundle.defence.index = index;

        enemyList.Add(enemy);

        return enemy;
    }

    public void SummonGoldGoblin()
    {
        goldGoblin.SetActive(true);
    }

    public GameObject SummonArmoredGoblin()
    {
        GameObject enemy = Instantiate(armoredGoblin, Vector3.zero, Quaternion.identity);
        enemy.transform.SetParent(enemyBundle);
        var scriptBundle = enemy.GetComponent<EnemyBundle>();
        scriptBundle.defence.index = 100;

        ArmoredStone stone = Instantiate(armoredStone, Vector3.zero, Quaternion.identity).GetComponent<ArmoredStone>();
        stone.armoredGoblin = enemy.GetComponent<ArmoredGoblin>();
        enemy.GetComponent<ArmoredGoblin>().m_StoneAnimator = stone.GetComponent<Animator>();

        enemyList.Add(enemy);

        for(int i = 0; i < 20; i++)
        {
            TileManager.Inst.TakeTile(90+i, true);
        }

        return enemy;
    }

    public void InvokeSummonItem()
    {
        Invoke(nameof(SummonItem), 0.1f);
    }

    public void SummonItem()
    {
        var index = FindTileIndex();
        GameObject item = Instantiate(GetRandomItem(), TileManager.Inst.tiles[index].position, Quaternion.identity);
        item.transform.SetParent(itemBundle);
    }

    public GameObject SummonReservedEnemy(GameObject prefab, int index)
    {
        GameObject enemy = Instantiate(prefab, TileManager.Inst.tiles[index].position, Quaternion.identity);
        enemy.transform.SetParent(enemyBundle);
        var scriptBundle = enemy.GetComponent<EnemyBundle>();
        scriptBundle.defence.index = index;

        enemyList.Add(enemy);

        return enemy;
    }

    public int FindTileIndex(bool isTakeTile = true)
    {
        int preIndex;
        do
        {
            preIndex = Random.Range(0, TileManager.Inst.tiles.Length);
            print("길이: " + TileManager.Inst.tiles.Length + " / 현: " + preIndex);
        } while (TileManager.Inst.tiles[preIndex].onTile);

        if(isTakeTile)
            TileManager.Inst.TakeTile(preIndex, true);
        return preIndex;
    }

    public GameObject GetNearbyEnemy(Transform self)
    {
        float minDistance = 10000;
        GameObject result = null;
        for (int i = 0; i < enemyList.Count; i++)
        {
            var offset = self.position - enemyList[i].transform.position;
            var sqrLen = offset.sqrMagnitude;
            if (sqrLen < minDistance)
            {
                minDistance = sqrLen;
                result = enemyList[i];
            }
        }

        return result;
    }

    public GameObject GetRandomEnemy()
    {
        var enemy = stagePersents[GameManager.Inst.curPhase].enemyPersents;

        GameObject summon = null;
        int ran = Random.Range(0, 100);
        int temp = 0;

        for (int i = 0; i < enemy.Length; i++)
        {
            if (temp + enemy[i].persent > ran)
            {
                summon = enemy[i].prefab;
                break;
            }
            else
            {
                temp += enemy[i].persent;
            }
        }

        return summon;
    }

    public GameObject GetRandomItem()
    {
        GameObject summon = itemPrfs[Random.Range(0, itemPrfs.Length)];

        return summon;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if(enemyList.Contains(enemy))
            enemyList.Remove(enemy);
    }
}
