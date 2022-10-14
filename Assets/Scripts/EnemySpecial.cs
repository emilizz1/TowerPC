using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecial : MonoBehaviour
{
    public float cooldown;
    public float useTime;

    internal bool currentlyInUse = false;

    public virtual void UseSpecial()
    {

        currentlyInUse = true;
        StartCoroutine(SpecialInUse());
    }

    IEnumerator SpecialInUse()
    {
        yield return new WaitForSeconds(useTime);
        SpecialFinished();

    }

    internal virtual void SpecialFinished()
    {
        currentlyInUse = false;
    }
}
