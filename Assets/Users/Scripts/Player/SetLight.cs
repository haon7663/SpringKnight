using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class SetLight : MonoBehaviour
{
    [SerializeField] Light2D light2D;
    [SerializeField] float defaultIntensity;

    void Start()
    {
        light2D.intensity = defaultIntensity;
    }

    public void SuddenLight(float value, float endTime)
    {
        light2D.intensity = value;
        DOTween.To(() => light2D.intensity, x => light2D.intensity = x, defaultIntensity, endTime);
    }
}
