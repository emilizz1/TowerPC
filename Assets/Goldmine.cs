using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goldmine : Structure
{
    [SerializeField] int goldPerWave;

    public override void Activate()
    {
        base.Activate();
        Taxes.instance.passiveIncome += goldPerWave;
    }
}
