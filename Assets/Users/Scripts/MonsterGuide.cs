using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInfo
{
    public Sprite enemySprite;
    public float spriteSize = 1;
    public string enemyName;
    [TextArea] public string enemyExplain;
    public int minDefence;
    public int maxDefence;
}
public class MonsterGuide : MonoBehaviour
{
    public EnemyInfo[] enemyInfos;

    [SerializeField] Transform content;
    [SerializeField] EnemyGuideUI enemyGuideUI;

    void Start()
    {
        for(int i = 0; i < enemyInfos.Length; i++)
        {
            EnemyGuideUI info = Instantiate(enemyGuideUI, content);
            info.index = i;
            info.monsterGuide = this;
        }
    }
}
