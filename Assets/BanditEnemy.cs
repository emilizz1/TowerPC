using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditEnemy : Enemy
{
    [SerializeField] int moneyStolen;

    public override void ReachedEnd()
    {
        base.ReachedEnd();
        if (!returning)
        {
            Money.instance.TryPaying(moneyStolen);
            audioSource.PlayOneShot(specialClips[Random.Range(0, specialClips.Count)]);

        }
    }
}
