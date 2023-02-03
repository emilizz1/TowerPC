using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReseter : MonoBehaviour
{
    void Start()
    {
        //TODO make this added to thee scene
        CostController.Reset();
        PasiveTowerStatsController.Reset();
        SpellPlacer.Reset();
        TowerPlacer.Reset();
        TurnController.Reset();
        ChargesController.Reset();
    }
}
