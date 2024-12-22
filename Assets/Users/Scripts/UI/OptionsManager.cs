using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public GameObject m_Right;
    public GameObject m_Left;

    public Text m_Explain;
    public Image m_Image;

    public GameObject m_Options;   
    public Sprite[] sprites;
    [TextArea]
    public string[] explain;
    private int count;

    public void OnRightLeft(int a)
    {
        count += a;
        m_Explain.text = explain[count];
        m_Image.sprite = sprites[count];
        m_Right.SetActive(count < 5);
        m_Left.SetActive(count > 0);
    }
    public void OnOptionSetting()
    {
        m_Options.SetActive(!m_Options.activeSelf);
    }
}
