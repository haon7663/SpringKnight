using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIListItem : MonoBehaviour
{
    [Header("Progress")]
    [SerializeField] Image progressFilled;
    [SerializeField] Text gainText;
    [SerializeField] Color maxChargeColor;
    [SerializeField] Color nonChargeColor;

    [Space]
    [SerializeField] Text nameText;
    [SerializeField] Text explainText;

    [Space]
    [SerializeField] Text rewardText;

    [Space]
    [SerializeField] Image select;
    [SerializeField] Button selectButton;
    [SerializeField] Sprite onSelect;
    [SerializeField] Sprite deSelect;
    [SerializeField] Image selectedGray;

    MissionRotation curMissionRotation;
    int id;

    public void Init(int id)
    {
        this.id = id;
        var data = SaveManager.Inst.saveData.missionDatas[id];
        nameText.text = data.missionName;
        explainText.text = data.missionExplain;

        var rewardAmount = data.rewardAmount.ToString();

        Debug.Log(rewardAmount);
        if (data.prizeType == PrizeType.GOLD)
            rewardText.text = $"+ {rewardAmount}골드";
        else if (data.prizeType == PrizeType.CHEST)
            rewardText.text = $"1단계 특성상자 {rewardAmount}개";
        else if (data.prizeType == PrizeType.CHEST2)
            rewardText.text = $"2단계 특성상자 {rewardAmount}개";

        var chargeAmount = data.curProgress / data.maxProgress;
        var maxCharge = data.curProgress >= data.maxProgress;
        progressFilled.fillAmount = chargeAmount;
        select.sprite = maxCharge ? onSelect : deSelect;
        gainText.text = maxCharge ? "수령하기" : ((int)(chargeAmount * 100)).ToString() + "%";
        gainText.color = maxCharge ? maxChargeColor : nonChargeColor;
        selectButton.enabled = maxCharge;

        selectedGray.enabled = data.isReceived;
        gameObject.SetActive(data.missionRotation == curMissionRotation);
    }

    public void SetOrder()
    {
        if (SaveManager.Inst.saveData.missionDatas[id].isReceived) GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void GetReward()
    {
        var saveData = SaveManager.Inst.saveData;
        var data = saveData.missionDatas[id];
        if (data.prizeType == PrizeType.GOLD)
            saveData.gold += data.rewardAmount;

        LobbyManager.Inst.OpenItemActive(data.rewardAmount, data.prizeType);
        if (data.missionRotation == MissionRotation.DAILY)
        {
            data.isReceived = true;
            selectedGray.enabled = true;
            MissionManager.Inst.ClearQuest();
        }
        else if (data.missionRotation == MissionRotation.INFINITE)
        {
            data.curProgress -= data.maxProgress;
            Init(id);
        }

        SaveManager.Inst.Save();
    }

    public void MissionRotateActive(MissionRotation missionRotation)
    {
        gameObject.SetActive(SaveManager.Inst.saveData.missionDatas[id].missionRotation == missionRotation);
        curMissionRotation = missionRotation;
        GetComponent<RectTransform>().SetAsLastSibling();
    }
}