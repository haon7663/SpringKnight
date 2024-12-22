using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSize : MonoBehaviour
{
    private Camera m_MainCamera;

    private void Start()
    {
        m_MainCamera = Camera.main;
        GetComponent<BoxCollider2D>().size = new Vector2(m_MainCamera.orthographicSize * 0.5625f * 2, m_MainCamera.orthographicSize * 2);
    }
}
