using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : PrizeInformation
{
    public bool isCalled;
    public override void UseItem()
    {
        if (isCalled) return;

        HealthManager.Inst.OnHealth(1);
        if (TryGetComponent(out Animator animator))
        {
            animator.SetTrigger("use");
            isCalled = true;

            if (TryGetComponent(out AudioSource audioSource))
                audioSource.Play();
        }
        else
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (isCalled)
            transform.position = Movement.Inst.transform.position;
    }

    public void RemoveItem()
    {
        Destroy(gameObject);
    }
}
