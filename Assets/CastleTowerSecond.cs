using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleTowerSecond : CastleTower
{
    public override void Start()
    {
        TowerPlacer.castleTowerSecond = this;
        gameObject.SetActive(false);
    }

    public override void SetupRange(float additionalRange = 0)
    {
        statsMultiplayers = TowerPlacer.castleTower.statsMultiplayers;
        base.SetupRange(additionalRange);
    }

    internal override float GetTimeToNextShot()
    {
        statsMultiplayers = TowerPlacer.castleTower.statsMultiplayers;
        return base.GetTimeToNextShot();
    }

    internal override List<float> GetDamageMultiplied()
    {
        statsMultiplayers = TowerPlacer.castleTower.statsMultiplayers;
        return base.GetDamageMultiplied();
    }
}
