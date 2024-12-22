using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimeScale : MonoBehaviour
{
    Rigidbody2D m_Rigidbody2D;
    Collison m_Collison;

    [SerializeField] LayerMask enemyLayer;

    public bool isRigidTime = true;
    public bool isBossTime = false;
    [SerializeField] float defaultTimeScale;
    [SerializeField] float rigidTimeScale;
    public float setTime;

    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Collison = GetComponent<Collison>();
    }

    void LateUpdate()
    {
        if (!GameManager.Inst.onPlay && !GameManager.Inst.onDeath)
        {
            if(GameManager.Inst.onPause)
                Time.timeScale = 0;
            return;
        }

        if (!isRigidTime || isBossTime) return;

        setTime = defaultTimeScale;
        SetRigidTime();

        Time.timeScale = Mathf.Lerp(Time.timeScale, setTime, Time.deltaTime * 9);
    }

    void SetRigidTime()
    {
        if (GameManager.Inst.onDeath) return;

        var ray = m_Collison.rayOffset;
        for (int i = 0; i < ray.Length; i++)
            if (Physics2D.Raycast(transform.position + ray[i] * 0.95f, m_Rigidbody2D.velocity, 1, enemyLayer) && Movement.Inst.count > 0)
            {
               Time.timeScale = rigidTimeScale;
                setTime = rigidTimeScale;
            }
    }
}
