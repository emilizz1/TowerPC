using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MarketWindow : MonoSingleton<MarketWindow>
{
    public CardHolder allCards;

    [SerializeField] TweenAnimator animator;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI explanationText;
    [SerializeField] GameObject marketDecor;
    [SerializeField] GameObject forgeDecor;
    [SerializeField] GameObject graveyardDecor;

    internal bool market;
    internal bool forge;
    internal bool graveyard;

    public void Open()
    {
        Cover.cover = true;
        animator.PerformTween(1);

        OpenCorrectShop();

        TipsManager.instance.CheckForMarketTip();
    }

    private void OpenCorrectShop()
    {
        if (market)
        {
            if (Random.Range(0f, 1f) <= 0.5f)
            {
                forge = true;
                market = false;
                MarketCardManager.instance.DisplayNewForge();
                nameText.text = "Forge";
                explanationText.text = "Upgrade your cards";
                marketDecor.SetActive(false);
                forgeDecor.SetActive(true);
                graveyardDecor.SetActive(false);
            }
            else
            {
                graveyard = true;
                market = false;
                MarketCardManager.instance.DisplayNewGraveyard();
                nameText.text = "Graveyard";
                explanationText.text = "Remove cards from your deck";
                marketDecor.SetActive(false);
                forgeDecor.SetActive(false);
                graveyardDecor.SetActive(true);
            }
        }
        else if (forge)
        {
            if (Random.Range(0f, 1f) <= 0.5f)
            {
                market = true;
                forge = false;
                MarketCardManager.instance.DisplayNewMarket();
                nameText.text = "Market";
                explanationText.text = "Buy new card";
                marketDecor.SetActive(true);
                forgeDecor.SetActive(false);
                graveyardDecor.SetActive(false);
            }
            else
            {
                graveyard = true;
                forge = false;
                MarketCardManager.instance.DisplayNewGraveyard();
                nameText.text = "Graveyard";
                explanationText.text = "Remove cards from your deck";
                marketDecor.SetActive(false);
                forgeDecor.SetActive(false);
                graveyardDecor.SetActive(true);
            }
        }
        else
        {
            if (Random.Range(0f, 1f) <= 0.5f)
            {
                forge = true;
                graveyard = false;
                MarketCardManager.instance.DisplayNewForge();
                nameText.text = "Forge";
                explanationText.text = "Upgrade your cards";
                marketDecor.SetActive(false);
                forgeDecor.SetActive(true);
                graveyardDecor.SetActive(false);
            }
            else
            {
                market = true;
                graveyard = false;
                MarketCardManager.instance.DisplayNewMarket();
                nameText.text = "Market";
                explanationText.text = "Buy new card";
                marketDecor.SetActive(true);
                forgeDecor.SetActive(false);
                graveyardDecor.SetActive(false);
            }
        }
    }

    public void Close()
    {
        Cover.cover = false;
        MarketCardManager.instance.CloseMarket();
        animator.PerformTween(0);
        TurnController.FinishedModifying();
    }

    public Card GetUpgradedCard(Card cardToUpgrade)
    {
        foreach (Card card in MarketWindow.instance.allCards.cardsCollection[0].cards)
        {
            if (card.cardName == cardToUpgrade.cardName)
            {
                if (card.cardLevel == cardToUpgrade.cardLevel + 1)
                {
                    return Instantiate(card);
                }
            }
        }
        return null;
    }

    public void CheckIfAllCardsUpgraded()
    {
        foreach (Card card in Deck.instance.deckCards)
        {
            if (card.cardLevel == 0)
            {
                return;
            }
        }

        foreach (Card card in Hand.instance.handCards)
        {
            if (card.cardLevel == 0)
            {
                return;
            }
        }

        foreach (Card card in Discard.instance.discardCards)
        {
            if (card.cardLevel == 0)
            {
                return;
            }
        }

        AchievementManager.AllDeckUpgraded();
    }
}
