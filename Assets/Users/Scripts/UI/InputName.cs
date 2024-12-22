using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputName : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Image disable;

    void Update()
    {
        disable.enabled = !(nameText.text.Length >= 3 && nameText.text.Length <= 8);
    }

    public void SetName()
    {
        LobbyManager.Inst.SetName(nameText.text);
    }
}
