using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrizeType { GOLD, CHEST, CHEST2 }
public class PrizeDatas : MonoBehaviour
{
    [SerializeField] Sprite goldSprite;
    [SerializeField] Sprite chest1Sprite;
    [SerializeField] Sprite chest2Sprite;

    [SerializeField] float goldSize;
    [SerializeField] float chestSize;

    public Vector2 GetItemSize(PrizeType prizeType)
    {
        Vector2 size = prizeType switch
        {
            PrizeType.GOLD => new Vector2(goldSize, goldSize),
            PrizeType.CHEST => new Vector2(chestSize, chestSize),
            PrizeType.CHEST2 => new Vector2(chestSize, chestSize),
            _ => new Vector2(goldSize, goldSize),
        };
        return size;
    }
    public Sprite GetItemSprite(PrizeType prizeType)
    {
        Sprite sprite = prizeType switch
        {
            PrizeType.GOLD => goldSprite,
            PrizeType.CHEST => chest1Sprite,
            PrizeType.CHEST2 => chest2Sprite,
            _ => goldSprite,
        };
        return sprite;
    }

    public string GetItemTypeName(PrizeType prizeType)
    {
        string itemType = prizeType switch
        {
            PrizeType.GOLD => "골드",
            PrizeType.CHEST => "1단계 특성상자",
            PrizeType.CHEST2 => "2단계 특성상자",
            _ => "골드",
        };
        return itemType;
    }
}
