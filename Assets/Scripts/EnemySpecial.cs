using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecial : MonoBehaviour
{
    public float cooldown;
    public float useTime;
    public GameObject particles;

    internal bool currentlyInUse = false;

    public virtual void UseSpecial()
    {

        currentlyInUse = true;
        particles.SetActive(true);
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
        particles.SetActive(false);
    }
}
