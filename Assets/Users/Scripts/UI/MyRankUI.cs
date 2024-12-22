using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyRankUI : MonoBehaviour
{
    public Text nameText;
    public Text rankText;
    public Text scoreText;
    public Text levelText;
    public Image profileImage;

    void Start()
    {
        var saveData = SaveManager.Inst.saveData;
        nameText.text = saveData.id;
        scoreText.text = saveData.maxScore.ToString() + "Á¡";
        levelText.text = "Lv."+saveData.level.ToString();
    }
}
