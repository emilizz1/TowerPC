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
        Destroy,
        Research
    }

    public static TurnPhase currentPhase = TurnPhase.Drawing;
    public static int currentTurn;

    public static void FinishedDrawing()
    {
        if(currentPhase == TurnPhase.Drawing)
        {
            if (ResearchWindow.instance.shouldOpenWindow)
            {
                currentPhase = TurnPhase.Research;
                ResearchWindow.instance.IfNoResearchSelectedOpen();
            }
            else
            {
                currentPhase = TurnPhase.Preperation;
                TileManager.instance.ChangeButtonInteractability(true);

            }
            PhaseInfo.instance.PhaseChanged(currentPhase);
        }
    }

    public static void ResearchSelected()
    {
        if (currentPhase == TurnPhase.Research)
        {
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
            TowerPlacer.ClearTowerTargets();
            Hand.instance.DiscardHand();

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
            Money.instance.GetIncome();
            currentTurn++;
            PhaseInfo.instance.PhaseChanged(currentPhase);
            PlayerLife.instance.Regen();
            Hand.instance.DrawNewHand();
            ResearchWindow.instance.AdvanceResearch();
            Money.instance.UpdateIncome();
        }
    }
}
