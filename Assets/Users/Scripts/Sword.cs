using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    SetAnimation m_SetAnimation;

    [SerializeField] GameObject spinSword;
    [SerializeField] LayerMask enemyLayer;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SetAnimation = GetComponentInParent<SetAnimation>();
    }
    private void Update()
    {
        if (GameManager.Inst.isSetting || HealthManager.Inst.curhp <= 0) return;

        var movement = Movement.Inst;
        m_SpriteRenderer.enabled = movement.count > 0;
        /*if (movement.bouncedCount >= 10 && movement.count > 0)
        {
            if (!spinSword.activeSelf)
            {
                m_SetAnimation.Spin(true);
                spinSword.SetActive(true);
            }
            spinSword.transform.Rotate(0, 0, 2000 * Time.deltaTime);

            Collider2D enemy = Physics2D.OverlapCircle(transform.position, 2, enemyLayer);
            if (enemy)
            {
                CinemachineShake.Instance.ShakeCamera(7, 0.5f);
                enemy.transform.GetComponent<EnemyDefence>().OnDamage(Vector3.zero);
            }
        }
        else if (spinSword.activeSelf)
        {
            spinSword.SetActive(false);
            m_SetAnimation.Spin(false);
        }*/
    }
}
