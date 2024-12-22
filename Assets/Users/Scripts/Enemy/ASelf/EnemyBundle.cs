using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyRace { GOBLIN, SLOTH, KNIGHT, MOSTROS }
public enum EnemyClass { NORMAL, BOSS }

public class EnemyBundle : MonoBehaviour
{
    public EnemyRigidbody rigid;
    public EnemyDefence defence;
    public EnemySprite sprite;

    [Space]
    [Header("Division")]
    public EnemyRace enemyRace;
    public EnemyClass enemyClass;
}
