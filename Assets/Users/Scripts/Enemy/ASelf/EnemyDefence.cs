using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyDefence : MonoBehaviour
{
    EnemyBundle m_EnemyBundle;
    AudioSource m_AudioSource;

    public int index;
    public int defence;
    [SerializeField] float defPositionY;
    [SerializeField] int minDefence;
    [SerializeField] int maxDefence;
    [SerializeField] RectTransform defenceParent;
    [SerializeField] GameObject hitParticle;

    RectTransform defenceBar;
    GameObject[] defences = new GameObject[8];

    void Awake()
    {
        if (TryGetComponent(out EnemyBundle bundle))
            m_EnemyBundle = bundle;
        if (TryGetComponent(out AudioSource audioSource))
            m_AudioSource = audioSource;
    }

    void Start()
    {
        defenceBar = Instantiate(defenceParent, DefenceManager.Inst.defenceBundle);

        if (maxDefence == 0) defence = minDefence;
        else defence = Random.Range(minDefence, maxDefence + 1);

        SetDefence();
    }

    public void SetDefence(int changeDef = 0)
    {
        defence += changeDef;
        if (defence < 1)
        {
            changeDef = 0;
            defence = 1;
        }
        var def = defence - 1;
        var perDef = def > 4 ? 4 : def;
        for (int i = 0; i < perDef; i++)
        {
            defences[i] = defenceBar.GetChild(i).gameObject;
            if (!defences[i].activeSelf) defences[i].SetActive(true);
            defences[i].GetComponent<Image>().enabled = true;

            var setPosition = DefenceManager.Inst.GetDefPosition(i, def) * 20 + new Vector2(0, defPositionY);
            if(perDef == 4)
                defences[i].GetComponent<Image>().sprite = DefenceManager.Inst.GetDefSprite(defence - i - 2);
            defences[i].GetComponent<RectTransform>().DOLocalMove(setPosition, 0.2f);
        }
        if (changeDef < 0)
        {
            for (int i = 0; i < -changeDef; i++)
            {
                var perDefIPer = (def + i) % 4;
                defences[perDefIPer].GetComponentInChildren<ParticleSystem>()?.Play();
                if(def + i < 4) defences[perDefIPer].GetComponent<Image>().enabled = false;
            }
        }
    }
    void LateUpdate()
    {
        if(defenceBar != null)
            defenceBar.position = transform.position + new Vector3(0, 1);
    }

    public int AttemptAttack(int damage)
    {
        return damage - defence;
    }

    public void OnDamage(Transform target, Vector3 velocity)
    {
        if (TryGetComponent(out EnemyNonDestroy nonDestroy))
        {
            bool isDeath = nonDestroy.AfterDamaged();
            StartCoroutine(m_EnemyBundle.sprite.GracePeriod());
            m_EnemyBundle.sprite.hitTimer = 0.1f;
            if (!isDeath) return;
        }

        if (GetComponent<EnemyDestroy>()) GetComponent<EnemyDestroy>().AfterDestroy();

        TileManager.Inst.TakeTile(index, false);
        SummonManager.Inst.RemoveEnemy(gameObject);
        GameManager.Inst.KillEvent(this);
        if(GameManager.Inst.m_GameMode != GameMode.TUTORIAL)
            MissionManager.Inst.DestroyEnemy(m_EnemyBundle.enemyRace, m_EnemyBundle.enemyClass);
        GameManager.Inst.AddScore(14, true);
        GameManager.Inst.AddGold(5);
        m_AudioSource.Play();
        print("Sound");

        if (TutorialManager.Inst)
            TutorialManager.Inst.enemyTrigger = true;

        Destroy(defenceBar.gameObject);
        if (TryGetComponent(out DemensionDestroyer demension))
            demension.PortalDestroy(0.15f);

        StartCoroutine(m_EnemyBundle.rigid.BouncedOff(target, Vector3.Magnitude(velocity)));
    }
}
