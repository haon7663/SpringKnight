using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum TimeType { ONE, COUNT, INFINITE }

[Serializable]
public struct Property
{
    public TimeType timeType;
    [DrawIf("timeType", TimeType.ONE)] public bool isUsed;
    [DrawIf("timeType", TimeType.COUNT)] public int maxCount;
    [DrawIf("timeType", TimeType.COUNT)] public int curCount;
    public bool isCalled;

    [Space]
    public Sprite sprite;
    public string explain;
    public string longExplain;
    [Space]
    public float value;
    [Space]
    public UnityEvent selectEvent;
}

public class PropertiesManager : MonoBehaviour
{
    public static PropertiesManager Inst;
    void Awake() => Inst = this;

    public Property[] properties;

    [Space]
    [SerializeField] RectTransform[] propertieTransforms;
    [SerializeField] RectTransform propertyData;
    [SerializeField] Transform selectPanel;
    [SerializeField] Sprite frame_One;
    [SerializeField] Sprite frame_Count;
    [SerializeField] Sprite frame_Inf;

    [Space]
    [SerializeField] SelectProperty m_SelectProperty;

    public void Start()
    {
        if (GameManager.Inst.saveProperty.Length != 0)
            properties = GameManager.Inst.saveProperty;
        else
            GameManager.Inst.saveProperty = properties;
        for (int i = 0; i < properties.Length; i++)
            properties[i].isCalled = false;
        for(int i = 0; i < 3; i++)
        {
            RectTransform data = Instantiate(propertyData, propertieTransforms[i]);
            var countData = data.GetComponent<PropertyData>();
            countData.index = GetPropertyIndex();
            countData.countIndex = i;
            data.anchoredPosition = Vector2.zero;
        }
    }
    public int GetPropertyIndex()
    {
        int ran = 0;
        do
        {
            ran = Random.Range(0, properties.Length);
        } while (GetUsetype(ran) || properties[ran].isCalled);

        properties[ran].isCalled = true;
        return ran;
    }
    public bool GetUsetype(int ran, bool isChange = false)
    {
        bool result;

        switch (properties[ran].timeType)
        {
            case TimeType.ONE:
                result = properties[ran].isUsed;
                if (!result && isChange) properties[ran].isUsed = true;
                break;
            case TimeType.COUNT:
                result = properties[ran].curCount >= properties[ran].maxCount;
                if(!result && isChange) properties[ran].curCount++;
                break;
            default:
                result = false;
                break;
        }
        if (!result && isChange)
        {
            GameManager.Inst.selectedPropertySprite.Add(properties[ran].sprite);
        }

        GameManager.Inst.saveProperty = properties;
        return result;
    }

    public Sprite GetFrame(int index)
    {
        Sprite sprite = properties[index].timeType switch
        {
            TimeType.ONE => frame_One,
            TimeType.COUNT => frame_Count,
            _ => frame_Inf,
        };
        return sprite;
    }
    public void SelectIndex(int index)
    {
        m_SelectProperty.index = index;
    }
}
