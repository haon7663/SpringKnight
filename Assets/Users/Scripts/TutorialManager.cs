using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Inst;
    void Awake() => Inst = this;

    [SerializeField] Transform OverlayCanvas;
    [Space]
    [Header("BaseTutorial")]
    [SerializeField] GameObject _FirstDrag;
    [SerializeField] GameObject _FirstPower;
    [SerializeField] GameObject _FirstEnemy;
    [SerializeField] GameObject _FirstScore;
    [SerializeField] GameObject _FirstTime;
    [SerializeField] GameObject _SecondEnemy;
    [SerializeField] GameObject _FirstBounce;
    [SerializeField] GameObject _FirstEnd;
    [Header("BaseTutorial_Prefab")]
    [SerializeField] GameObject Sloth_Def0;
    [SerializeField] GameObject Sloth_Def1;

    [HideInInspector] public bool dragTrigger;
    [HideInInspector] public bool touchTrigger;
    [HideInInspector] public bool enemyTrigger;
    [HideInInspector] public bool bounceTrigger;
    public bool isWaitTrigger;

    enum TutorialState { DRAG, TOUCH, TIME, ENEMY, BOUNCE };

    void Start()
    {
        StartCoroutine(BaseTutorial());
    }

    public void TouchTrigger()
    {
        touchTrigger = true;
    }

    private float touchTime;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchTime = 1f;
        }
        
        touchTime -= Time.deltaTime;
        if (touchTime > 0 && Input.GetMouseButtonUp(0))
            TouchTrigger();
    }

    IEnumerator BaseTutorial()
    {
        yield return new WaitForSeconds(2);

        yield return StartCoroutine(SummonTutorial(_FirstDrag, TutorialState.TOUCH, 2));
        yield return StartCoroutine(SummonTutorial(_FirstPower, TutorialState.TOUCH, 5));

        SummonManager.Inst.SummonReservedEnemy(Sloth_Def0, 15);
        yield return StartCoroutine(SummonTutorial(_FirstEnemy, TutorialState.TOUCH, 0));

        yield return StartCoroutine(WaitWhile(TutorialState.ENEMY));

        UIManager.Inst.OpenScore();
        UIManager.Inst.OpenTimer(120);
        yield return YieldInstructionCache.WaitForSeconds(3.5f);
        yield return StartCoroutine(SummonTutorial(_FirstScore, TutorialState.TOUCH, 2));
        yield return StartCoroutine(SummonTutorial(_FirstTime, TutorialState.TOUCH, 2));

        SummonManager.Inst.SummonReservedEnemy(Sloth_Def1, 15);
        yield return StartCoroutine(SummonTutorial(_SecondEnemy, TutorialState.TOUCH, 0));

        yield return StartCoroutine(WaitWhile(TutorialState.BOUNCE));
        yield return YieldInstructionCache.WaitForSeconds(0.02f);

        Movement.Inst.m_SetTimeScale.isBossTime = true;
        Movement.Inst.m_SetTimeScale.setTime = 0.001f;
        Time.timeScale = 0.001f;
        Time.fixedDeltaTime = 0.005f / 1000;
        yield return StartCoroutine(SummonTutorial(_FirstBounce, TutorialState.TOUCH, 0));
        Movement.Inst.m_SetTimeScale.isBossTime = false;
        Movement.Inst.m_SetTimeScale.setTime = 1;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.005f;

        yield return StartCoroutine(WaitWhile(TutorialState.ENEMY));

        yield return YieldInstructionCache.WaitForSeconds(1f);
        yield return StartCoroutine(SummonTutorial(_FirstEnd, TutorialState.TOUCH, 0.2f));

        SaveManager.Inst.gameMode = GameMode.STAGE;
        StartCoroutine(UIManager.Inst.ShowResultPanel(true));
    }

    IEnumerator SummonTutorial(GameObject prefab, TutorialState tutorialState, float endTime, float time = 2)
    {
        prefab.SetActive(true);
        Transform tutorial = prefab.transform;
        tutorial.SetParent(OverlayCanvas);
        tutorial.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //tutorial.SetAsFirstSibling();

        yield return StartCoroutine(WaitWhile(tutorialState, time));
        prefab.SetActive(false);

        yield return new WaitForSeconds(endTime);
    }
    IEnumerator WaitWhile(TutorialState tutorialState, float time = 2)
    {
        switch (tutorialState)
        {
            case TutorialState.DRAG:
                isWaitTrigger = true;
                dragTrigger = false;
                while (!dragTrigger)
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                dragTrigger = false;
                break;
            case TutorialState.TOUCH:
                isWaitTrigger = true;
                touchTrigger = false;
                while (!touchTrigger)
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                touchTrigger = false;
                break;
            case TutorialState.TIME:
                float timer = time;
                while (timer > 0)
                {
                    timer -= Time.deltaTime;
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                }
                break;
            case TutorialState.ENEMY:
                enemyTrigger = false;
                while (!enemyTrigger)
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                enemyTrigger = false;
                break;
            case TutorialState.BOUNCE:
                bounceTrigger = false;
                while (!bounceTrigger)
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                bounceTrigger = false;
                break;
        }

        isWaitTrigger = false;
    }
}
