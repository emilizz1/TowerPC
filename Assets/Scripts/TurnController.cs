using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TurnController
{
    public enum TurnPhase
    {
        Drawing,
        Preperation,
        EnemyWave,
        Market
    }

    public static TurnPhase currentPhase = TurnPhase.Drawing;
    public static int currentTurn;

    public static void FinishedDrawing()
    {
        if(currentPhase == TurnPhase.Drawing)
        {
            Debug.Log("Started preperation");
            currentPhase = TurnPhase.Preperation;
            TileManager.instance.ChangeButtonInteractability(true);
        }
    }

    public static void StartedEnemyWave()
    {
        if (currentPhase == TurnPhase.Preperation)
        {
            Debug.Log("Started EnemyWave");
            currentPhase = TurnPhase.EnemyWave;
            TileManager.instance.ChangeButtonInteractability(false);
        }
    }

    public static void FinishedEnemyWave()
    {
        if (currentPhase == TurnPhase.EnemyWave)
        {
            Debug.Log("Started Market");
            currentPhase = TurnPhase.Market;
            MarketWindow.instance.Open();
        }
    }

    public static void FinishedBuying()
    {

        if (currentPhase == TurnPhase.Market)
        {
            Debug.Log("Started Drawing");
            currentPhase = TurnPhase.Drawing;
            ResearchWindow.instance.AdvanceResearch();
            Hand.instance.DrawNewHand();
        }
    }
}
