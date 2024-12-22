using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffect : MonoBehaviour
{
    public static CameraEffect Inst;
    void Awake() => Inst = this;

    public Volume m_Volume;
    Vignette m_Vignette;
    VolumeProfile volumeProfile;

    float sine;
    bool isDamaged;

    void Start()
    {
        volumeProfile = m_Volume.profile;
    }

    void LateUpdate()
    {
        if(HealthManager.Inst.curhp <= 1 && !isDamaged)
        {
            sine += Time.deltaTime * 2;

            if (volumeProfile.TryGet(out m_Vignette))
            {
                m_Vignette.intensity.value = Mathf.Lerp(m_Vignette.intensity.value, 0.35f * (GameManager.Inst.isBerserker ? 1.5f : 1) + Mathf.Sin(sine) * 0.1f, Time.deltaTime * 5);
            }
        }
        else
        {
            if (volumeProfile.TryGet(out m_Vignette))
            {
                m_Vignette.intensity.value = 0;
            }
        }
    }

    public IEnumerator OnDamage()
    {
        isDamaged = true;
        for (float i = 0; i < 0.2f; i += Time.deltaTime)
        {
            if(volumeProfile.TryGet(out m_Vignette))
            {
                m_Vignette.intensity.value = Mathf.Lerp(m_Vignette.intensity.value, 0.5f, Time.deltaTime * 25);
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        for (float i = 0; i < 0.5f; i += Time.deltaTime)
        {
            if (volumeProfile.TryGet(out m_Vignette))
            {
                m_Vignette.intensity.value = Mathf.Lerp(m_Vignette.intensity.value, 0, Time.deltaTime * 8);
            }
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
        isDamaged = false;
        m_Vignette.intensity.value = 0;
    }
}
