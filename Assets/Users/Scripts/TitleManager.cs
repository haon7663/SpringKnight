using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    bool isCalled;

    private void Awake()
    {
        Screen.SetResolution(1080, 1920, true);
    }
    private void Start()
    {
        GameManager.Inst.SetGame();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !isCalled)
        {
            Fade.Inst.Fadein();
            Invoke(nameof(GameStart), 0.5f);
            isCalled = true;
        }
    }
    private void GameStart()
    {
        SceneManager.LoadScene("Stage");
    }
}
