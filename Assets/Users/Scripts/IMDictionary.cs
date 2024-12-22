using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMDictionary : MonoBehaviour
{
    public GameObject m_ItemSetting;
    public GameObject m_MonsterSetting;
    public void OnIMSetting(bool item)
    {
        m_ItemSetting.SetActive(item);
        m_MonsterSetting.SetActive(!item);
    }
}
