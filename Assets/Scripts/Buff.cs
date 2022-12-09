using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    internal BuffController.BuffInfo info;

    internal Tower myTower;
    internal int stacks;
    internal List<GameObject> buffParticles;

    internal virtual void Start()
    {
        buffParticles = new List<GameObject>();
        myTower = GetComponent<Tower>();
        AddStack();
    }

    public void AddStack()
    {
        stacks++;
        ApplyBuff();
    }

    internal virtual void ApplyBuff()
    {
        buffParticles.Add(Instantiate(info.particles, myTower.transform));
    }

    internal virtual void RemoveBuff()
    {
        foreach(GameObject particle in buffParticles)
        {
            Destroy(particle);
        }
    }
}
