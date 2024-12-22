using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst { get; private set; }
    void Awake() => Inst = this;

    [Header("SetResult")]
    [SerializeField] Image resultGray;
    [SerializeField] Transform resultPanel;
    [SerializeField] Text resultSituationText;
    [SerializeField] Transform resultPropertyContent;
    [SerializeField] Image selectedProperty;
    [SerializeField] Text resultScoreText;
    [SerializeField] Text resultMaxScoreText;
    [SerializeField] Text resultKillScoreText;
    [SerializeField] Text resultGoldText;
    [SerializeField] ContentSizeFitter resultCsf;
    [SerializeField] GameObject result_Infinite;
    [SerializeField] GameObject result_Gold;
    [SerializeField] Image resultPlayer;

    [Space]
    [Header("SetPhase")]
    [SerializeField] Transform phaseBar;
    [SerializeField] Image phaseFilled;
    [SerializeField] RectTransform phaseGrid;
    [SerializeField] RectTransform phasePlayer;
    [SerializeField] GameObject openPhase;
    [SerializeField] Text openPhaseText;

    [Space]
    [Header("SetPower")]
    [SerializeField] Transform powerBar;
    [SerializeField] Image powerFilled;
    [SerializeField] Image powerWhiteFilled;
    [SerializeField] RectTransform powerGrid;
    [SerializeField] Text powerText;

    [Space]
    [Header("SetScore")]
    [SerializeField] Text scoreText;
    float showScore, scoreAccent;

    [Space]
    [Header("SetTimer")]
    [SerializeField] GameObject timeBundle;
    [SerializeField] Image timeFilled;
    [SerializeField] Text timeText;
    [SerializeField] Material whiteMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] float flashTime;
    float timer, flashTimer;

    [Space]
    [Header("SetProperties")]
    [SerializeField] Image propertiesWindow;
    [SerializeField] RectTransform propertiesPanel;
    [SerializeField] RectTransform longExplainPanel;
    [SerializeField] Text propertyName;
    [SerializeField] Text longExplain;
    [SerializeField] Text dispostion;
    [SerializeField] Image selectedImage;
    [SerializeField] Image frameImage;
    [SerializeField] Transform propertyContent;
    [SerializeField] Transform selectedPropertyPrf;

    [SerializeField] Image[] propertyFrames;
    [SerializeField] Sprite propertySelected;
    [SerializeField] Sprite propertyDeSelected;
    [SerializeField] GameObject disabledProperty;
    public bool onProperties;

    [Space]
    [Header("SetCombo")]
    [SerializeField] Transform comboPanel;
    [SerializeField] RectTransform comboPrefab;
    [SerializeField] Vector2 comboStartPos;
    [SerializeField] Vector2 comboOriginPos;

    Text saveComboText;
    RectTransform pazeRect, powerRect;

    void Start()
    {
        pazeRect = phaseBar.transform.parent.GetComponent<RectTransform>();
        powerRect = powerBar.transform.parent.GetComponent<RectTransform>();
        SwapUI(false, 0.4f);

        openPhase.SetActive(true);
        openPhaseText.text = "PHASE " + (GameManager.Inst.curPhase + 1).ToString();
        scoreText.text = ((int)GameManager.Inst.score).ToString();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            SetProperties(!onProperties);
        }

        if (timeFilled.gameObject.activeSelf && !GameManager.Inst.onPlay) return;

        scoreText.text = ((int)showScore).ToString();
        scoreAccent -= Time.deltaTime / 2.5f;

        if (timeBundle.activeSelf)
        {
            timeFilled.fillAmount = timer / GameManager.Inst.maxTimer;
            if (flashTimer > flashTime && timer < 10)
            {
                flashTimer = 0;
                timeFilled.material = timeFilled.material == defaultMaterial ? whiteMaterial : defaultMaterial;
            }
            timeText.text = timer.ToString("F1") + "sec";
            flashTimer += Time.deltaTime;
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                StartCoroutine(ShowResultPanel(false));
                timer = 0;
                timeText.text = timer.ToString("F1") + "sec";
            }
        }
    }

    public void OpenTimer(float time)
    {
        GameManager.Inst.maxTimer = time;
        timeBundle.SetActive(true);
        timer = time;
    }
    public void OpenScore()
    {
        scoreText.gameObject.SetActive(true);
    }
    public void SetScore(float time)
    {
        scoreAccent += 0.6f;
        if (scoreAccent > 1) scoreAccent = 1;
        Sequence sequence = DOTween.Sequence();
        sequence.Join(DOTween.To(() => showScore, x => showScore = x, GameManager.Inst.score, time));
        sequence.Join(scoreText.transform.DOScale(Vector2.one * 1.25f, time));
        sequence.Join(scoreText.DOColor(new Color(1, 1 - scoreAccent, 0, 1), time/1.5f));
        sequence.Append(scoreText.transform.DOScale(Vector2.one * 1, time/2));
        sequence.Join(scoreText.DOColor(Color.white, time));
    }

    public IEnumerator ShowResultPanel(bool isClear)
    {
        resultSituationText.text = isClear ? "클리어" : "게임오버";
        if (isClear) SaveManager.Inst.saveData.isFirstGame = true;
        var sprites = GameManager.Inst.selectedPropertySprite;
        resultPlayer.sprite = Character.Inst.characters[(int)Character.Inst.playerType].playerSprite;

        if (GameManager.Inst.m_GameMode == GameMode.GOLD)
            result_Infinite.SetActive(false);
        else
            result_Gold.SetActive(false);

        for (int i = 0; i < sprites.Count; i++)
        {
            Image property = Instantiate(selectedProperty, resultPropertyContent.GetChild(Mathf.FloorToInt(i / 3)), false);
            property.sprite = sprites[i];
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)resultCsf.transform);


        GameManager.Inst.AddGold(GameManager.Inst.goldCount * (GameManager.Inst.curPhase / 2));
        var saveData = SaveManager.Inst.saveData;
        if (GameManager.Inst.score > saveData.maxScore)
        {
            SaveManager.Inst.saveData.maxScore = (int)GameManager.Inst.score;
        }
        resultScoreText.text = GameManager.Inst.score.ToString();
        resultMaxScoreText.text = SaveManager.Inst.saveData.maxScore.ToString();
        resultKillScoreText.text = GameManager.Inst.killCount.ToString();
        resultGoldText.text = "x" + GameManager.Inst.goldCount.ToString();

        for (float i = 0; i < 1; i+= Time.deltaTime)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        GameManager.Inst.ChangeState(GameState.PAUSE);

        resultGray.gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(resultGray.DOFade(0.75f, 0.5f)).SetUpdate(true);
        sequence.Append(resultPanel.DOScaleX(1, 0.25f)).SetUpdate(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)resultCsf.transform);
    }
    public void SetProperties(bool onProperties)
    {
        this.onProperties = onProperties;
        Sequence sequence = DOTween.Sequence();
        if (onProperties)
        {
            GameManager.Inst.ChangeState(GameState.PAUSE);
            sequence.Append(propertiesWindow.DOFade(0.7f, 0.25f).SetUpdate(true)).SetUpdate(true);
            propertiesPanel.localScale = new Vector2(0, 1);
            sequence.Append(propertiesPanel.DOScaleX(1, 0.25f).SetUpdate(true)).SetUpdate(true);

            var sprites = GameManager.Inst.selectedPropertySprite;
            for (int i = 0; i < sprites.Count; i++)
            {
                Image property = Instantiate(selectedPropertyPrf, propertyContent).GetChild(0).GetComponent<Image>();
                property.sprite = sprites[i];
            }
            for (int i = 0; i < 3; i++)
                propertyFrames[i].sprite = propertyDeSelected;
            disabledProperty.SetActive(true);
        }
        else
        {
            GameManager.Inst.ChangeState(GameState.PLAY);
            sequence.Append(propertiesPanel.DOScaleX(0, 0.15f).SetUpdate(true)).SetUpdate(true);
            sequence.Append(propertiesWindow.DOFade(0, 0.25f).SetUpdate(true)).SetUpdate(true);
        }
    }
    public void SetExplainPanel(string propertyName, string longExplain, string dispostion, Sprite sprite, Sprite frameSprite, int countIndex)
    {
        this.propertyName.text = propertyName;
        this.longExplain.text = longExplain;
        this.dispostion.text = dispostion;
        selectedImage.sprite = sprite;
        frameImage.sprite = frameSprite;
        for(int i = 0; i < propertyFrames.Length; i++)
        {
            propertyFrames[i].sprite = countIndex == i ? propertySelected : propertyDeSelected;
        }
        disabledProperty.SetActive(false);
    }

    public void SetPhase(int curPaze, int maxPaze)
    {
        float pazeCount = maxPaze - 1;
        for(int i = 1; i <= pazeCount; i++)
        {
            RectTransform grid = Instantiate(phaseGrid, phaseBar);
            grid.anchoredPosition = new Vector3(i * (700 / (pazeCount+1)), 0);
        }

        phaseFilled.fillAmount = (curPaze) / (float)maxPaze;
        phasePlayer.anchoredPosition = new Vector2(58 + (curPaze - 1) * (700 / (pazeCount + 1)), phasePlayer.anchoredPosition.y);

        phaseFilled.DOFillAmount((curPaze+1) / (float)maxPaze, 0.75f);
        phasePlayer.DOAnchorPosX(58 + curPaze * (700 / (pazeCount + 1)), 0.75f);

    }

    public void SetPowerGrid(int maxPower)
    {
        float powerCount = maxPower - 2;
        for (int i = 1; i <= powerCount; i++)
        {
            RectTransform grid = Instantiate(powerGrid, powerBar);
            grid.anchoredPosition = new Vector3(i * (468 / (powerCount + 1)) + 216.5f, 0);
        }
    }
    public void SetPower(float curPower)
    {
        float maxPower = GameManager.Inst.maxPower - 1;
        var value = curPower - 1 - maxPower;
        var fillValue = (curPower - 1) / maxPower;
        if (value > 0)
        {
            DOTween.Kill(powerFilled);
            powerText.text = " +" + value.ToString();
            powerText.rectTransform.DOAnchorPosX(93f, 0.15f);
            powerText.DOFade(1, 0.1f);
            powerFilled.DOFillAmount(1, 0.01f);
        }
        else
        {
            DOTween.Kill(powerFilled);
            powerText.rectTransform.DOAnchorPosX(12, 0.15f);
            powerText.DOFade(0, 0.1f);
            powerFilled.DOFillAmount(fillValue, 0.03f);
            powerWhiteFilled.DOFillAmount(fillValue, 0.3f);
        }
    }
    public void SwapUI(bool onPower, float time)
    {
        if (GameManager.Inst.m_GameMode != GameMode.STAGE) return;

        pazeRect.gameObject.SetActive(!onPower);
        powerRect.gameObject.SetActive(onPower);
        pazeRect.DOAnchorPosY(onPower ? 1000 : 800, time);
        powerRect.DOAnchorPosX(onPower ? 0 : 880, time).SetEase(Ease.OutExpo);
    }

    public void AddCombo(int value)
    {
        DeleteCombo(0.5f);
        var combo = Instantiate(comboPrefab, comboPanel);
        combo.anchoredPosition = comboStartPos;
        combo.DOAnchorPos(comboOriginPos, 0.5f).SetEase(Ease.OutQuint);
        saveComboText = combo.GetComponent<Text>();
        saveComboText.text = value.ToString() + " COMBO";
        saveComboText.DOFade(0.8f, 0.25f);
    }
    public void DeleteCombo(float time)
    {
        if (saveComboText == null) return;
        saveComboText.DOFade(0, time);
        Destroy(saveComboText.gameObject, time);
    }
}
