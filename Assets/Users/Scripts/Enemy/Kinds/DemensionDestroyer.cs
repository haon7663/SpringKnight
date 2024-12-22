using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DemensionDestroyer : EnemyDashSign
{
    [SerializeField] GameObject portalPrefab;
    [SerializeField] Vector2[] posOffset;

    public List<GameObject> portals = new List<GameObject>();
    public override void AfterDash()
    {
        PortalDestroy(0.5f);

        int ran = Random.Range(0, posOffset.Length);
        GameObject portal = Instantiate(portalPrefab, (Vector2)transform.position + posOffset[ran], Quaternion.Euler(0, 0, ran * 90));
        //portal.transform.SetParent(transform);
        portals.Add(portal);
    }

    public void PortalDestroy(float time)
    {
        for (int i = 0; i < portals.Count; i++)
        {
            portals[i].gameObject.layer = 9;
            portals[i].GetComponent<SpriteRenderer>().DOFade(0, time);
            Destroy(portals[i], time);
            portals.Remove(portals[i]);
        }
    }
}
