using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TowerPlacer
{
    static internal List<Tower> allTowers = new List<Tower>();

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
}
