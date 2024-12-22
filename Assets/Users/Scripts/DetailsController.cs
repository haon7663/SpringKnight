using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DetailsController : MonoBehaviour
{
    [SerializeField] Toggle onCameraFollowToggle;
    [SerializeField] Toggle onVibrationToggle;

    void Start()
    {
        onCameraFollowToggle.onValueChanged.AddListener(value => SettingManager.Inst.onCameraFollow = value);
        onVibrationToggle.onValueChanged.AddListener(value => SettingManager.Inst.onVibration = value);

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void PlayerSettingSave()
    {
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
