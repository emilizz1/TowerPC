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
        CardModification,
        Destroy,
        Research,
        Event
    }

    public static TurnPhase currentPhase = TurnPhase.Drawing;
    public static int currentTurn= 1;
    public static int actionsPlayed;
    public static int cardsPlayed;

    static int eventTurn;

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
            Deck.instance.newCardsButton.interactable = true;
            actionsPlayed = 0;
            cardsPlayed = 0;
            EnemyManager.instance.PrepareNextWave();
            PlayerLife.instance.NewRound();
            ObjectPools.instance.NewTurn();
        }
    }

    public static void ResearchSelected()
    {
        if (currentPhase == TurnPhase.Research)
        {
            currentPhase = TurnPhase.Preperation;

            TileManager.instance.ChangeButtonInteractability(true);
            if (currentTurn == 1)
            {
                TipsManager.instance.CheckForTipPressingExpand();
            }
        }
    }

    public static void StartedEnemyWave()
    {
        if (currentPhase == TurnPhase.Preperation)
        {
            Mana.instance.StartRegen();
            Deck.instance.newCardsButton.interactable = false;
            currentPhase = TurnPhase.EnemyWave;
            Soundtrack.instance.BattleStart();
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
            TileManager.instance.CheckForMisplacedTowers();
            EnemyManager.instance.CheckIfGameWon();
            Soundtrack.instance.BattleEnd();

            currentPhase = TurnPhase.CardModification;

            if (TipsManager.instance.marketSkipped)
            {

                if (currentTurn % 2 == 0)
                {
                        EventWindow.instance.OpenEvent();                    
                }
                else
                {
                    EventWindow.instance.OpenBaseSelection();

                }
            }
            else
            {
                TipsManager.instance.MarketSkipped();
                FinishedModifying();
            }
        }
    }

    public static void FinishedModifying()
    {
        if (currentPhase == TurnPhase.CardModification)
        {
            currentPhase = TurnPhase.Drawing;
            CostController.currentTurnDiscount = 0;
            Money.instance.GetIncome();
            currentTurn++;
            WaveCounter.instance.DisplayCounter();
            PlayerLife.instance.Regen();
            Hand.instance.DrawNewHand();
            ResearchWindow.instance.AdvanceResearch();
            Money.instance.UpdateIncome();
        }
    }

    public static void Reset()
    {
        currentPhase = TurnPhase.Drawing;
        currentTurn = 1;
        eventTurn = 0;
    }
}
