using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyData : MonoBehaviour
{
    public Image spriteImage;

    public TimeType timeType;
    string propertyName;
    string longExplain;
    public int index;
    public int countIndex;

    public void Start()
    {
        var property = PropertiesManager.Inst.properties[index];

        float value = property.value * (property.timeType == TimeType.COUNT ? property.curCount + 1 : 1);
        timeType = property.timeType;
        propertyName = property.explain.Replace("!", value.ToString());
        longExplain = property.longExplain.Replace("!", value.ToString());
        spriteImage.sprite = property.sprite;
    }

    public void ButtonDown()
    {
        string disposition = GetDisposition(timeType);

        UIManager.Inst.SetExplainPanel(propertyName, longExplain, disposition, spriteImage.sprite, PropertiesManager.Inst.GetFrame(index), countIndex);
        PropertiesManager.Inst.SelectIndex(index);
    }

    string GetDisposition(TimeType timeType)
    {
        string result = timeType switch
        {
            TimeType.COUNT => "",
            TimeType.INFINITE => "(ÈÖ¹ß¼º)",
            TimeType.ONE => "(ÀÏÈ¸¼º)",
            _ => "",
        };
        return result;
    }
}
