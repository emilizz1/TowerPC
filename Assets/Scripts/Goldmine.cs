using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goldmine : Structure
{
    [SerializeField] int goldPerWave;

    public override void Activate(TerrainBonus terrain)
    {
        base.Activate(terrain);
        Money.instance.AddToPassiveIncome(goldPerWave);
        if(terrain != null)
        {
            if(terrain.type == TerrainBonus.TerrainType.mountain)
            {
                Money.instance.AddToPassiveIncome(goldPerWave);
            }
        }
    }
}
