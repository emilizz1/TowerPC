using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.Loc;

public class MarketWindow : MonoSingleton<MarketWindow>
{

    [SerializeField] TweenAnimator animator;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI explanationText;
    [SerializeField] GameObject marketDecor;
    [SerializeField] GameObject forgeDecor;
    [SerializeField] GameObject graveyardDecor;
    [SerializeField] LocalizedString marketName;
    [SerializeField] LocalizedString marketDescription;
    [SerializeField] LocalizedString forgeName;
    [SerializeField] LocalizedString forgeDescription;
    [SerializeField] LocalizedString graveyardName;
    [SerializeField] LocalizedString graveyardDescription;

    internal bool market;
    internal bool forge;
    internal bool graveyard;

    public void Open(int whatToOpen)
    {
        Cover.cover = true;
        animator.PerformTween(1);

        switch (whatToOpen)
        {
            case (0):
                OpenMarket();
                break;
            case (1):
                OpenForge();
                break;
            case (2):
                OpenGraveyard();
                break;
        }

        TipsManager.instance.CheckForMarketTip();
    }

    private void OpenMarket()
    {
        market = true;
        forge = false;
        MarketCardManager.instance.DisplayNewMarket();
        nameText.text = marketName;
        explanationText.text = marketDescription;
        marketDecor.SetActive(true);
        forgeDecor.SetActive(false);
        graveyardDecor.SetActive(false);
    }

    private void OpenGraveyard()
    {
        graveyard = true;
        market = false;
        MarketCardManager.instance.DisplayNewGraveyard();
        nameText.text = graveyardName;
        explanationText.text = graveyardDescription;
        marketDecor.SetActive(false);
        forgeDecor.SetActive(false);
        graveyardDecor.SetActive(true);
    }

    private void OpenForge()
    {
        forge = true;
        market = false;
        MarketCardManager.instance.DisplayNewForge();
        nameText.text = forgeName;
        explanationText.text = forgeDescription;
        marketDecor.SetActive(false);
        forgeDecor.SetActive(true);
        graveyardDecor.SetActive(false);
    }

    public void Close()
    {
        Cover.cover = false;
        MarketCardManager.instance.CloseMarket();
        animator.PerformTween(0);
        TurnController.FinishedModifying();
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
