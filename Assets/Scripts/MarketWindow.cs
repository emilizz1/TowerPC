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

    internal bool market;
    internal bool forge;
    internal bool graveyard;

    public void Open()
    {
        Cover.cover = true;
        animator.PerformTween(1);

        OpenCorrectShop();

        MarketCardManager.instance.DisplayNewMarket();
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
            }
            else
            {
                graveyard = true;
                market = false;
                MarketCardManager.instance.DisplayNewGraveyard();
                nameText.text = "Graveyard";
                explanationText.text = "Remove cards from your deck";
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
            }
            else
            {
                graveyard = true;
                forge = false;
                MarketCardManager.instance.DisplayNewGraveyard();
                nameText.text = "Graveyard";
                explanationText.text = "Remove cards from your deck";
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
            }
            else
            {
                market = true;
                graveyard = false;
                MarketCardManager.instance.DisplayNewMarket();
                nameText.text = "Market";
                explanationText.text = "Buy new card";
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
}
