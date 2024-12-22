using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public void GameStart()
    {
        GameManager.Inst.SetGame();
        StartCoroutine(MoveScene());
    }
    IEnumerator MoveScene()
    {
        Fade.Inst.Fadein();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Faze");
    }
}
