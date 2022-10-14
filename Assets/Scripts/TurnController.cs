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
        Market,
        Destroy
    }

    public static TurnPhase currentPhase = TurnPhase.Drawing;
    public static int currentTurn;

    public static void FinishedDrawing()
    {
        if(currentPhase == TurnPhase.Drawing)
        {
            ResearchWindow.instance.AdvanceResearch();
            currentPhase = TurnPhase.Preperation;
            PhaseInfo.instance.PhaseChanged(currentPhase);
            TileManager.instance.ChangeButtonInteractability(true);
        }
    }

    public static void StartedEnemyWave()
    {
        if (currentPhase == TurnPhase.Preperation)
        {
            Mana.instance.StartRegen();
            currentPhase = TurnPhase.EnemyWave;
            PhaseInfo.instance.PhaseChanged(currentPhase);
            TileManager.instance.ChangeButtonInteractability(false);
        }
    }

    public static void FinishedEnemyWave()
    {
        if (currentPhase == TurnPhase.EnemyWave)
        {
            Mana.instance.StopRegen();
            SpellPlacer.StopAllSpells();
            Hand.instance.DiscardHand();

            currentPhase = TurnPhase.Destroy;
            PhaseInfo.instance.PhaseChanged(currentPhase);
            DestroyCardWindow.instance.Open();
        }
    }

    public static void FinishedDestroying()
    {
        if(currentPhase == TurnPhase.Destroy)
        {
            currentPhase = TurnPhase.Market;
            PhaseInfo.instance.PhaseChanged(currentPhase);
            MarketWindow.instance.Open();
        }
    }

    public static void FinishedBuying()
    {

        if (currentPhase == TurnPhase.Market)
        {
            currentPhase = TurnPhase.Drawing;
            PhaseInfo.instance.PhaseChanged(currentPhase);
            PlayerLife.instance.Regen();
            Hand.instance.DrawNewHand();
        }
    }
}
