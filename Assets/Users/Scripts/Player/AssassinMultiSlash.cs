using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinMultiSlash : MonoBehaviour
{
    public Movement movement;

    [SerializeField] SummonAfter summonAfter;
    public void AssassinTrigger()
    {
        summonAfter.SummonAfterImage();
        movement.AssassinTrigger();
    }
}
