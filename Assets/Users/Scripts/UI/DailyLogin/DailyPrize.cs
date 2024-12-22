using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyPrize : MonoBehaviour
{
    [SerializeField] PrizeDatas dailyLogin;
    Button button;

    [Space]
    [SerializeField] Image[] itemImage;
    [SerializeField] Text[] amountText;
    [SerializeField] Text[] itemTypeText;

    [Space]
    [SerializeField] Text dailyText;
    [SerializeField] Image disableImage;
    [SerializeField] Image enableImage;

    [Space]
    [Header("Value")]
    [SerializeField] PrizeType[] prizeType;
    [SerializeField] int[] prizeAmount;
    [SerializeField] int day;

    void Start()
    {
        SetButton();
    }

    public void SetButton()
    {
        button = GetComponent<Button>();

        var isBefore = SaveManager.Inst.saveData.dailyCount + 1 > day;
        var isToday = SaveManager.Inst.saveData.dailyCount + 1 == day;
        var canResive = isToday && !SaveManager.Inst.saveData.isConnect;
        button.enabled = canResive;
        enableImage.enabled = canResive;
        dailyText.text = canResive ? "수령 가능" : isBefore ? "수령 완료" : day.ToString() + "일차";

        for (int i = 0; i < itemImage.Length; i++)
        {
            amountText[i].text = "x" + prizeAmount[i].ToString();
            itemTypeText[i].text = dailyLogin.GetItemTypeName(prizeType[i]);
            itemImage[i].sprite = dailyLogin.GetItemSprite(prizeType[i]);
            itemImage[i].transform.localScale = dailyLogin.GetItemSize(prizeType[i]);
        }
        SetAbled();
    }

    public void SetAbled()
    {
        disableImage.enabled = day <= SaveManager.Inst.saveData.dailyCount;
    }

    public void GetReward()
    {
        var saveData = SaveManager.Inst.saveData;
        if (saveData.dailyCount + 1 != day || SaveManager.Inst.saveData.isConnect) return;

        for (int i = 0; i < itemImage.Length; i++)
        {
            if (prizeType[i] == PrizeType.GOLD)
                saveData.gold += prizeAmount[i];
            else if (prizeType[i] == PrizeType.CHEST)
                saveData.chest += prizeAmount[i];
            else if (prizeType[i] == PrizeType.CHEST2)
                saveData.chest2 += prizeAmount[i];

            LobbyManager.Inst.OpenItemActive(prizeAmount[i], prizeType[i]);
        }

        saveData.dailyCount++;
        saveData.isConnect = true;
        button.enabled = false;
        enableImage.enabled = false;
        dailyText.text = "수령 완료";
        SetAbled();


        SaveManager.Inst.Save();
    }

}
