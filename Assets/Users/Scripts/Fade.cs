using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    public static Fade Inst;
    void Awake() => Inst = this;

    [SerializeField] Image m_Image;

    void Start()
    {
        m_Image.enabled = true;
        if (SceneManager.GetActiveScene().name != "InGame")
            StartCoroutine(Fadeout());
    }
    public IEnumerator Fadeout()
    {
        m_Image.enabled = true;
        yield return new WaitForSecondsRealtime(0.3f);
        DOTween.Complete(m_Image);
        m_Image.DOFade(0, 1f).SetUpdate(true);
        if (GameManager.Inst)
        {
            if (GameManager.Inst.curPhase == 0)
                UIManager.Inst.SetPowerGrid(GameManager.Inst.maxPower);
            else
                UIManager.Inst.SetProperties(true);

            yield return YieldInstructionCache.WaitForSeconds(0.2f);
            GameManager.Inst.ChangeState(GameState.PLAY);
        }
    }
    public void Fadein(float time = 0.25f)
    {
        DOTween.Complete(m_Image);
        m_Image.DOFade(1, time).SetUpdate(true);
    }
}
