using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    [SerializeField] Slider[] slider;
    [SerializeField] Text[] sliderValueText;

    SoundManager soundManager;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        slider[0].onValueChanged.AddListener(value => soundManager.AudioControl("Master", slider[0]));
        slider[1].onValueChanged.AddListener(value => soundManager.AudioControl("BGM", slider[1]));
        slider[2].onValueChanged.AddListener(value => soundManager.AudioControl("SFX", slider[2]));

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void Update()
    {
        for (var i = 0; i < slider.Length; i++)
        {
            sliderValueText[i].text = ((int)(slider[i].value * 2.5f + 100)).ToString();
        }
    }

    public void UIInitializing()
    {
        for (var i = 0; i < slider.Length; i++)
        {
            slider[i].value = SaveManager.Inst.saveData.volume[i];
        }
    }

    void PlayerSettingSave()
    {
        for (var i = 0; i < slider.Length; i++)
        {
            SaveManager.Inst.saveData.volume[i] = slider[i].value;
        }
        SaveManager.Inst.Save();
    }

    void OnSceneUnloaded(Scene current)
    {
        PlayerSettingSave();
    }

    void OnApplicationQuit()
    {
        PlayerSettingSave();
    }
}