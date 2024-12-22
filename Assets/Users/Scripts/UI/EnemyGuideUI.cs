using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGuideUI : MonoBehaviour
{
    [HideInInspector]
    public MonsterGuide monsterGuide;
    [Space]
    [SerializeField] Image profileImage;
    [SerializeField] Text nameText;
    [SerializeField] Text explainText;

    public int index;

    void Start()
    {
        var guide = monsterGuide.enemyInfos[index];
        profileImage.sprite = guide.enemySprite;
        profileImage.transform.localScale = new Vector2(guide.spriteSize, guide.spriteSize);
        nameText.text = guide.enemyName;
        explainText.text = guide.enemyExplain;
    }
}
