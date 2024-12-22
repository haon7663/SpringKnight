using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectProperty : MonoBehaviour
{
    public int index;
    public void Select()
    {
        PropertiesManager.Inst.properties[index].selectEvent.Invoke();
        PropertiesManager.Inst.GetUsetype(index, true);
        UIManager.Inst.SetProperties(false);
        UIManager.Inst.SetPowerGrid(GameManager.Inst.maxPower);
    }
}
