using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntensityBlock : MonoBehaviour
{
    public LayerMask mPlayerLayer;
    private SpriteRenderer m_SpriteRenderer;

    public Vector2 pos = new Vector2(0, 0);
    public bool isTouch;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        isTouch = Physics2D.OverlapBox((Vector2)transform.position + pos, new Vector2(1.2f, 1.2f), 0,mPlayerLayer);
        if (isTouch && m_SpriteRenderer.color.a == 1)
        {
            m_SpriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
        else if(!isTouch && m_SpriteRenderer.color.a == 0.5f)
        {
            m_SpriteRenderer.color = new Color(1, 1, 1, 1f);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + pos, new Vector3(1, 1));
    }
}
