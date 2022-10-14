using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    internal BuffController.BuffInfo info;

    internal Tower myTower;
    internal int stacks;

    internal virtual void Start()
    {
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

    }

    internal virtual void RemoveBuff()
    {

    }
}
