using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollviewTest : MonoBehaviour
{
    [SerializeField] WriteMission writeMission;

    public GameObject listItemPrefab;
    public Transform contents;

    public List<UIListItem> missions;


    [Space]
    [Header("ButtonAndText")]
    [SerializeField] Image allResivedButton;
    [SerializeField] Text textResived;

    [SerializeField] Image dailyButton;
    [SerializeField] Image infiniteButton;
    [SerializeField] Text dailyText;
    [SerializeField] Text infiniteText;
    [SerializeField] Sprite dailySelectedSprite;
    [SerializeField] Sprite dailyDeselectedSprite;
    [SerializeField] Sprite infiniteSelectedSprite;
    [SerializeField] Sprite infiniteDeselectedSprite;
    [SerializeField] Color selectedColor;
    [SerializeField] Color deSelectedColor;


    void Start()
    {
        missions = new List<UIListItem>();

        foreach (var pair in SaveManager.Inst.saveData.missionDatas)
        {
            var go = Instantiate<GameObject>(this.listItemPrefab, contents);
            var listItem = go.GetComponent<UIListItem>();
            listItem.Init(pair.id);
            missions.Add(listItem);
        }
        foreach (var mission in missions)
            mission.SetOrder();
    }

    public void ChangeRotateToDaily()
    {
        foreach (var mission in missions)
        {
            mission.MissionRotateActive(MissionRotation.DAILY);
        }
        foreach (var mission in missions)
            mission.SetOrder();

        dailyText.color = selectedColor;
        infiniteText.color = deSelectedColor;
        dailyButton.sprite = dailySelectedSprite;
        infiniteButton.sprite = infiniteDeselectedSprite;
    }
    public void ChangeRotateToInfinite()
    {
        foreach (var mission in missions)
        {
            mission.MissionRotateActive(MissionRotation.INFINITE);
        }
        foreach (var mission in missions)
            mission.SetOrder();

        dailyText.color = deSelectedColor;
        infiniteText.color = selectedColor;
        dailyButton.sprite = dailyDeselectedSprite;
        infiniteButton.sprite = infiniteSelectedSprite;
    }
}