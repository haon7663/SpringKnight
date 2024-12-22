using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject BombParticle;
    private SpriteRenderer mSpriteRenderer;
    private void Start()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(Inv());
    }
    internal static class YieldInstructionCache
    {
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
        private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            WaitForSeconds wfs;
            if (!waitForSeconds.TryGetValue(seconds, out wfs))
                waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
            return wfs;
        }
    }
    private IEnumerator Inv()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.3f);
        for (int i = 0; i < 2; i++)
        {
            mSpriteRenderer.color = new Color(1, 1, 1, 0.4f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            mSpriteRenderer.color = new Color(1, 1, 1, 0.8f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        for (int i = 0; i < 6; i++)
        {
            mSpriteRenderer.color = new Color(1, 1, 1, 0.4f);
            yield return YieldInstructionCache.WaitForSeconds(0.025f);
            mSpriteRenderer.color = new Color(1, 1, 1, 0.8f);
            yield return YieldInstructionCache.WaitForSeconds(0.025f);
        }

        Instantiate(BombParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
        yield return null;
    }
}
