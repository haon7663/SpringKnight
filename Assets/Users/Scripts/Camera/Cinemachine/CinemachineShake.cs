using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Inst { get; private set; }
    
    CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineConfiner2D _cinemachineConfiner2D;
    float shakeIntensity;
    float shakeTimer;

    Camera mainCamera;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    public float mulTime;
    public float mulIntensity;

    private void Awake()
    {
        Inst = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineConfiner2D = GetComponent<CinemachineConfiner2D>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        mainCamera = Camera.main;
        mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetShake(int index)
    {
        switch(index)
        {
            case 0:
                mulTime = 0.6f;
                mulIntensity = 0.5f;
                break;
            case 1:
                mulTime = 0.8f;
                mulIntensity = 0.75f;
                break;
            case 2:
                mulTime = 1f;
                mulIntensity = 1f;
                break;
            case 3:
                mulTime = 1.2f;
                mulIntensity = 1.4f;
                break;
            default:
                mulTime = 1f;
                mulIntensity = 1f;
                break;
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        shakeIntensity = intensity * mulIntensity;
        shakeTimer = time * mulTime;
    }
    private void LateUpdate()
    {
        if (shakeTimer > 0)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = shakeIntensity * shakeTimer * 4;
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
                mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        _cinemachineConfiner2D.InvalidateCache();
    }
}
