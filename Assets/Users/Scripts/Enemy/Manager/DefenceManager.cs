using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceManager : MonoBehaviour
{
    public static DefenceManager Inst { get; private set; }
    void Awake() => Inst = this;

    public Transform defenceBundle;
    [SerializeField] Sprite normalDefence;
    [SerializeField] Sprite upgradeDefence;
    public Vector2 GetDefPosition(int index, int defence)
    {
        var perDef = defence > 4 ? 4 : defence;
        var ind = index % 4;

        var setX = -perDef + ind * 2 + 1f;

        return new Vector2(setX, 0);
    }

    public Sprite GetDefSprite(int index)
    {
        return index >= 4 ? upgradeDefence : normalDefence;
    }
}
