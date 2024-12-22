using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum GameMode { STAGE, INFINITE, GOLD, TUTORIAL }
public enum GameState { LOADING, PAUSE, PLAY, DEATH, PRODUCTION }
public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; set; }

    public PlayerController m_PlayerController;
    public Character m_Character;

    public GameMode m_GameMode;
    public GameState m_GameState;

    public bool isInGame;
    public bool isLoadScene;
    public bool isSetting;

    public int gold;

    [Space]
    [Header("State")]
    public bool onPlay;
    public bool onPause;
    public bool onDeath;

    [Space]
    [Header("Stats")]
    public int curStage;
    public int curPhase;
    public int maxPhase;
    public int manageHealth = 3;
    public int maxHealth = 3;
    public int maxPower = 3;
    public int managePower = 3;
    public int summonCount = 3;
    public float enemySummonCount;

    [Space]
    [Header("Time")]
    public float maxTimer = 45;

    [Space]
    [Header("Score")]
    public float score;
    public int killCount;

    [Space]
    [Header("Gold")]
    public int goldCount;

    [Space]
    [Header("Property")]
    public Property[] saveProperty;
    public List<Sprite> selectedPropertySprite;
    public delegate void KillEnemyAction(EnemyDefence enemy);
    public static event KillEnemyAction KillEnemy;
    public delegate void SpawnEnemyAction(EnemyDefence enemy);
    public static event SpawnEnemyAction SpawnEnemy;

    [SerializeField] GameObject tutorialManager;

    void Awake()
    {
        Application.targetFrameRate = 60;

        var obj = FindObjectsOfType<GameManager>();
        if (obj.Length == 1 && SceneManager.GetActiveScene().name == "InGame")
        {
            DontDestroyOnLoad(gameObject);
            if (SaveManager.Inst.gameMode == GameMode.GOLD)
                SetGoldGame();
            else
                SetGame();
        }
        else
        {
            Destroy(gameObject);
        }
        Inst = this;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ChangeMode(GameMode gameMode)
    {
        m_GameMode = gameMode;
        switch (m_GameMode)
        {
            case GameMode.STAGE:
                break;
            case GameMode.INFINITE:
                UIManager.Inst.OpenScore();
                break;
            case GameMode.GOLD:
                UIManager.Inst.OpenTimer(40 - curPhase*4f);
                break;
            case GameMode.TUTORIAL:
                break;
        }
    }
    public void ChangeState(GameState gameState)
    {
        m_GameState = gameState;
        switch(m_GameState)
        {
            case GameState.PLAY:
                Time.timeScale = 1;
                onPlay = true;
                onPause = false;
                break;
            case GameState.LOADING:
                Time.timeScale = 1;
                onPlay = false;
                onPause = false;
                break;
            case GameState.PAUSE:
                Time.timeScale = 0;
                onPlay = false;
                onPause = true;
                break;
            case GameState.DEATH:
                onDeath = true;
                onPlay = false;
                onPause = false;
                break;
            case GameState.PRODUCTION:
                Time.timeScale = 1;
                onPlay = false;
                onPause = false;
                break;
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Inst = this;
        Time.timeScale = 1;

        if (SceneManager.GetActiveScene().name != "InGame") return;
         
        ChangeMode(!SaveManager.Inst.saveData.isFirstGame ? GameMode.TUTORIAL : (SaveManager.Inst.gameMode == GameMode.STAGE ? GameMode.INFINITE : SaveManager.Inst.gameMode));
        if (m_GameMode == GameMode.TUTORIAL) tutorialManager.SetActive(true);

        m_Character = Character.Inst;
        m_PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_PlayerController.maxPower = managePower;
        maxPower = managePower;
        maxPhase = TileManager.Inst.stageTileMaps[curStage].tileMaps.Length;

        HealthManager.Inst.SetHealth(maxHealth);
        HealthManager.Inst.curhp = manageHealth;
        HealthManager.Inst.OnHealth(0);

        UIManager.Inst.SetPhase(curPhase, maxPhase);
        UIManager.Inst.SetScore(0.25f);

        if(isSummonItems)
            for (int i = 0; i < summonItemValue; i++)
                SummonManager.Inst.InvokeSummonItem();

        isSetting = false;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (!isInGame) return;

        if (Input.GetKeyDown(KeyCode.Escape) && m_GameState == GameState.PLAY)
        {
            Application.Quit();
        }

        if (isBerserker && HealthManager.Inst.curhp == 1)
            Movement.Inst.atk = 1;
        else
            Movement.Inst.atk = 0;

        if (SummonManager.Inst.enemyList.Count <= 0 && m_GameMode != GameMode.TUTORIAL)
        {
            if (summonCount <= 0 && !isLoadScene)
            {
                if(curPhase == maxPhase - 1)
                {
                    StartCoroutine(UIManager.Inst.ShowResultPanel(true));
                    return;
                }
                isLoadScene = true;
                StartCoroutine(MoveScene());
            }
            else if (summonCount > 0)
            {
                summonCount--;

                var isAssassin = m_Character.playerType == PlayerType.ASSASSIN;

                if(m_GameMode == GameMode.GOLD)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var enemy = SummonManager.Inst.SummonEnemy();
                        if (isAssassin && (i == 0 || i == 1))
                            enemy.AddComponent<AssassinMark>();
                    }
                }
                else
                {
                    if(curPhase >= 8 && curPhase % 2 == 0)
                    {
                        var enemy = SummonManager.Inst.SummonArmoredGoblin();
                        if (isAssassin)
                            enemy.AddComponent<AssassinMark>();
                    }

                    for (int i = 0; i < (int)enemySummonCount; i++)
                    {
                        var enemy = SummonManager.Inst.SummonEnemy();
                        if (isAssassin && i == 0)
                            enemy.AddComponent<AssassinMark>();
                    }
                }
                //SummonManager.Inst.SummonItem();

                isLoadScene = false;
            }
        }
    }

    void SetModeGame()
    {

    }
    public void SetGame()
    {
        curPhase = 0;
        managePower = 3;
        maxHealth = 3;
        manageHealth = 3;
        enemySummonCount = 3;
        saveProperty = new Property[0];
        selectedPropertySprite = new List<Sprite>();

        ResetProperty();
    }

    public void SetGoldGame()
    {
        curPhase = 0;
        managePower = 4;
        maxPower = 4;
        maxHealth = 3;
        manageHealth = 3;
        enemySummonCount = 3;
        saveProperty = new Property[0];
        selectedPropertySprite = new List<Sprite>();
        SaveManager.Inst.saveData.goldDungeonCount--;

        Invoke("SummonGoldGoblin", 0.1f);

        ResetProperty();
    }
    void SummonGoldGoblin()
    {
        SummonManager.Inst.SummonGoldGoblin();
    }

    public void ResetLoad(string sceneName)
    {
        Destroy(gameObject);
        SceneManager.LoadScene(sceneName);
    }
    
    IEnumerator MoveScene()
    {
        ChangeState(GameState.LOADING);
        Time.timeScale = 0.15f;
        Movement.Inst.gameObject.layer = 8;
        yield return new WaitForSecondsRealtime(1);
        Fade.Inst.Fadein();
        yield return new WaitForSecondsRealtime(0.3f);

        manageHealth = HealthManager.Inst.curhp;
        enemySummonCount += 0.75f;
        managePower += 1;
        summonCount = 3;
        curPhase++;

        SceneManager.LoadScene("InGame");
    }

    public void AddScore(float value, bool isKill)
    {
        score += value;
        UIManager.Inst.SetScore(0.25f);
        if (isKill) killCount++;
    }

    public void AddGold(int gold)
    {
        goldCount += gold;
        SaveManager.Inst.saveData.gold += gold;
    }

    #region PropertyEffects
    bool isSummonItems;
    int summonItemValue, killGoldPersent, decreaseDefPersent, spawnDecDefPersent, maxBeginInvinsible;
    [HideInInspector]
    public int beginInvinsible;
    [HideInInspector]
    public bool isBerserker;

    void ResetProperty()
    {
        isSummonItems = false;
        isBerserker = false;
        summonItemValue = 0;
        killGoldPersent = 0;
        decreaseDefPersent = 0;
        spawnDecDefPersent = 0;

        KillEnemy = null;
        SpawnEnemy = null;
    }
    public void OneTimePowerUp(int value)
    {
        maxPower = managePower + value;
        m_PlayerController.maxPower += value;
        Debug.Log(m_PlayerController.maxPower);
    }
    public void Health(int value)
    {
        HealthManager.Inst.OnHealth(value);
    }    
    public void IncreaseMaxHealth()
    {
        maxHealth++;
        HealthManager.Inst.SetHealth(maxHealth);
        HealthManager.Inst.OnHealth(1);
    }
    public void IncreaseMaxPower()
    {
        managePower++;
        maxPower = managePower;
        m_PlayerController.maxPower = managePower;
    }
    public void IncreaseBeginInv()
    {
        maxBeginInvinsible++;
    }
    public void AddDownHp_UpAtk()
    {
        isBerserker = true;
    }
    public bool SetBeginInv(bool value)
    {
        if(value)
            beginInvinsible = maxBeginInvinsible;
        else
            beginInvinsible--;
        return beginInvinsible > 0;
    }
    public void SummonItem(int value)
    {
        isSummonItems = true;
        summonItemValue += value;
        SummonManager.Inst.SummonItem();
    }
    #region KillEvent
    public void KillEvent(EnemyDefence enemy)
    {
        KillEnemy?.Invoke(enemy);
    }
    public void AddKillGold(int persent)
    {
        if(KillEnemy != KillGold)
        {
            KillEnemy += KillGold;
            Debug.Log("ADDKILLGOLD");
        }
        killGoldPersent += persent;
    }
    public void AddKillDecDef(int value)
    {
        if (KillEnemy != KillDecDef)
        {
            KillEnemy += KillDecDef;
            Debug.Log("ADDDECDEF");
        }
        decreaseDefPersent += value;
    }
    void KillGold(EnemyDefence enemy)
    {
        if (Calculate(killGoldPersent))
            Debug.Log("Gold");
    }
    void KillDecDef(EnemyDefence enemy)
    {
        GameObject nearEnemy = SummonManager.Inst.GetNearbyEnemy(enemy.transform);
        if (nearEnemy && Calculate(decreaseDefPersent))
            nearEnemy.GetComponent<EnemyDefence>().SetDefence(-1);
    }
    #endregion
    #region SpawnEvent

    public void SpawnEvent(EnemyDefence enemy)
    {
        SpawnEnemy?.Invoke(enemy);
    }
    public void AddSpawnDecDef(int persent)
    {
        if (SpawnEnemy != SpawnDecDef)
        {
            SpawnEnemy += SpawnDecDef;
            Debug.Log("ADDSPAWNDECDEF");
        }
        spawnDecDefPersent += persent;
    }
    void SpawnDecDef(EnemyDefence enemy)
    {
        if (Calculate(spawnDecDefPersent))
        {
            enemy.SetDefence(-1);
        }
    }

    #endregion

    bool Calculate(int value)
    {
        float ran = Random.Range(0, 100f);
        return ran <= value;
    }

    #endregion
}

internal static class YieldInstructionCache
{
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        WaitForSeconds wfs;
        if (!waitForSeconds.TryGetValue(seconds, out wfs))
            waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
        return wfs;
    }
}
