using UnityEngine;
public enum MissionRotation { DAILY, WEEKLY, INFINITE }

[System.Serializable]
public class MissionData
{
    public string missionName;
    public string missionExplain;
    public PrizeType prizeType;

    [Space]
    public int id;

    [Space]
    public MissionRotation missionRotation;

    [Space]
    public int rewardAmount;

    [Space]
    public float maxProgress;
    public float curProgress;

    [Space]
    public bool isReceived;
}

public class WriteMission : MonoBehaviour
{
    public MissionData[] missionDatas;

    void Start()
    {
        if (SaveManager.Inst.saveData.missionDatas.Length == 0)
        {
            SaveManager.Inst.saveData.missionDatas = missionDatas;
            SaveManager.Inst.Save();
            SaveManager.Inst.Load();
        }
        SaveManager.Inst.Save();
    }
}