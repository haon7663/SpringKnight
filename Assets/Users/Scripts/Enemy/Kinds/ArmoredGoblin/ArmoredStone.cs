using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredStone : MonoBehaviour
{
    public ArmoredGoblin armoredGoblin;
    [SerializeField] Vector3[] offset;
    Vector3 ranPos;

    void Start()
    {
        ChangeStonePos();
    }
    void ChangeStonePos()
    {
        ranPos = offset[Random.Range(0, offset.Length)];
        transform.position = ranPos * 2;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(armoredGoblin.Attack(ranPos));
        }    
    }
}
