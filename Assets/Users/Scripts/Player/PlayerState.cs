using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Inst;
    void Awake() => Inst = this;

    [SerializeField] GameObject barrier;

    [Header("Reality")]
    public bool isInvincible;
    [Header("Inner")]
    public bool onItem;
    public bool onBegin;
    public void SetItem(bool value)
    {
        onItem = value;
        ActiveBarrier();
    }
    public void SetBegin(bool value)
    {
        onBegin = value;
        ActiveBarrier();
    }
    public void DisableBarrier()
    {
        onItem = false;
        onBegin = false;
        ActiveBarrier();
    }
    void ActiveBarrier()
    {
        var isInvincible = onItem || onBegin;
        this.isInvincible = isInvincible;
        barrier.SetActive(isInvincible);
    }
}
