using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TowerPlacer
{
    static internal List<Tower> allTowers = new List<Tower>();

    static internal CastleTower castleTower;
    static internal CastleTowerSecond castleTowerSecond;
    static internal GameObject towerToPlace;
    static internal int startingLevel;
    static internal bool towerPlaced;

    public static void ClearTowerTargets()
    {
        foreach(Tower tower in allTowers)
        {
            tower.ResetTargets();
        }
    }

    public static void Reset()
    {
        allTowers = new List<Tower>();
        towerToPlace = null;
        startingLevel = 0;
        towerPlaced = false;
    }

    public static void SecondCastleTowerUnlocked()
    {
        castleTowerSecond.gameObject.SetActive(true);
        castleTowerSecond.PrepareTower(null);
        castleTowerSecond.Activate();
        castleTowerSecond.rangeSprite.gameObject.SetActive(false);
    }
}
