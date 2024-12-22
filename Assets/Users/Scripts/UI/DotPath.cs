using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotPath : MonoBehaviour
{
    public Vector2 direction;

    public void GetRay(Vector2 position, Vector2 inputDir)
    {
        Debug.DrawRay(transform.position, inputDir, Color.yellow);
    }
}
