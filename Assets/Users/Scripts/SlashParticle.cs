using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashParticle : MonoBehaviour
{
    [SerializeField] Transform enemyHit;

    public Transform[] particleTransform;
    public Transform[] spriteTransform;

    void Start()
    {
        Destroy(gameObject, 0.75f);
    }

    public void SetParticle(bool flipX, Transform enemyPos)
    {
        enemyHit.position = enemyPos.position;

        for (int i = 0; i < particleTransform.Length; i++)
        {
            var value = flipX ? -1 : 1;
            particleTransform[i].localScale = new Vector3(value, value, 1);
        }
        for (int i = 0; i < spriteTransform.Length; i++)
        {
            var value = flipX ? -1 : 1;
            spriteTransform[i].localScale = new Vector3(value, 1, 1);
        }
    }
}
