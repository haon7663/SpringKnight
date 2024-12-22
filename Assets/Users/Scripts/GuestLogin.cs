using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GuestLogin : MonoBehaviour
{
    [SerializeField] Text logText;
    void Start()
    {
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        yield return new WaitForSecondsRealtime(2);
        logText.text = "로그인 성공";
        yield return new WaitForSecondsRealtime(2);
        Fade.Inst.Fadein(0.5f);
        yield return new WaitForSecondsRealtime(0.6f);
        SceneManager.LoadScene("Lobby");
    }
}
