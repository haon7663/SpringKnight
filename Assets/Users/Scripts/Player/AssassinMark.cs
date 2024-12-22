using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinMark : MonoBehaviour
{
    public Vector2 randomMark;
    GameObject markObj;
    void Start()
    {
        var charType = Character.Inst.characters[(int)Character.Inst.playerType].skillObjPrf_2;
        markObj = Instantiate(charType, transform);
        var ran = Random.Range(0, 4);
        switch (ran)
        {
            case 0:
                randomMark = Vector2.right;
                markObj.transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case 1:
                randomMark = Vector2.down;
                markObj.transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            case 2:
                randomMark = Vector2.left;
                markObj.transform.eulerAngles = new Vector3(0, 0, 180);
                break;
            case 3:
                randomMark = Vector2.up;
                markObj.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
        }
    }


    public bool AssassinKill(Vector2 value, bool isSucces)
    {
        if(isSucces || randomMark == value) Destroy(markObj);
        return randomMark == value;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, randomMark * 3);
    }
}
