using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayer : MonoBehaviour
{
    public PlayerType playerType;

    [Serializable]
    public struct PlayerInfo
    {
        public RuntimeAnimatorController idleAnimator;
        public string name;
        [TextArea] public string explain;
        public string skillName;
        [TextArea] public string skillExplain;

        public int hp;
        public int atk;
        public int bounce;
        public float speed;
    }

    [SerializeField] GameObject statusFrame;
    [SerializeField] GameObject skillFrame;

    [SerializeField] Animator playerStand_Home;
    [SerializeField] Animator playerStand_Character;
    [SerializeField] Text playerName;
    [SerializeField] Text playerExplain;
    [SerializeField] Text skillName;
    [SerializeField] Text skillExplain;

    [SerializeField] PlayerInfo[] playerInfos;

    [SerializeField] OnOffButton statusButton;
    [SerializeField] OnOffButton explainButton;

    void Start()
    {
        ChangePlayer((int)SaveManager.Inst.saveData.playerType);
    }
    public void ChangePlayer(int index)
    {
        playerType = (PlayerType)index;
        SaveManager.Inst.saveData.playerType = playerType;
        SaveManager.Inst.Save();

        var info = playerInfos[index];
        playerStand_Character.runtimeAnimatorController = info.idleAnimator;
        playerStand_Home.runtimeAnimatorController = info.idleAnimator;
        playerName.text = info.name;
        playerExplain.text = info.explain;
        skillName.text = info.skillName;
        skillExplain.text = info.skillExplain;
    }

    public void SetStatus(bool value)
    {
        statusFrame.SetActive(value);
        if (value) //ON
        {
            statusButton.Button.sprite = statusButton.SpriteOn;
            statusButton.ButtonText.color = statusButton.TextOn;
        }
        else
        {
            statusButton.Button.sprite = statusButton.SpriteOff;
            statusButton.ButtonText.color = statusButton.TextOff;
        }
    }
    public void SetSkill(bool value)
    {
        skillFrame.SetActive(value);
        if (value) //ON
        {
            explainButton.Button.sprite = explainButton.SpriteOn;
            explainButton.ButtonText.color = explainButton.TextOn;
        }
        else
        {
            explainButton.Button.sprite = explainButton.SpriteOff;
            explainButton.ButtonText.color = explainButton.TextOff;
        }
    }
}
