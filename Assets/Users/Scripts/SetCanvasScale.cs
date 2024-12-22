using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCanvasScale : MonoBehaviour
{
    [SerializeField] float targetAspectRatio = 1440f / 3120;
    private void Awake()
    {
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;

        var canvas = GetComponent<CanvasScaler>();
        if (currentAspectRatio > targetAspectRatio) canvas.matchWidthOrHeight = 1;       
        else if (currentAspectRatio < targetAspectRatio) canvas.matchWidthOrHeight = 0;
    }
}
