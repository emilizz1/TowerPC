using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.Loc;

public class TipsManager : MonoSingleton<TipsManager>
{
    [SerializeField] TweenAnimator animator;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] LocalizedString upgradeTowerText;
    [SerializeField] LocalizedString terrainText;
    [SerializeField] LocalizedString drawMoreCardsText;
    [SerializeField] LocalizedString pressingExpandText;
    [SerializeField] LocalizedString marketText;

    internal bool marketSkipped;

    bool showedTipTower;
    bool showedTipTerrain;
    bool showedTipCards;
    bool showedPressingExpand;
    bool marketTip;

    int towersPlacedOnNotTerrain;

    private void Start()
    {
        showedTipTower = SavedData.savesData.tipTower == 3; 
        showedTipTerrain = SavedData.savesData.tipTerrain == 5;
        showedTipCards = SavedData.savesData.tipCards == 2;
        showedPressingExpand = SavedData.savesData.tipPressingExpand == 2;
        marketSkipped = SavedData.savesData.marketSkipped == 2;
        marketTip = SavedData.savesData.tipMarket == 2;
    }


    public void CheckForTipUpgradeTower()
    {
        if (!showedTipTower)
        {
            animator.PerformTween(0);
            text.text = upgradeTowerText;
            SavedData.savesData.tipTower += 1;
            SavedData.Save();
            showedTipTower = true;
        }
    }

    public void CheckForTipTerrain(bool placedOnTerrain)
    {
        if (!showedTipTerrain)
        {
            if (placedOnTerrain)
            {
                towersPlacedOnNotTerrain = 0;
            }
            else
            {
                towersPlacedOnNotTerrain++;
                if(towersPlacedOnNotTerrain > 3)
                {
                    animator.PerformTween(0);
                    text.text = terrainText;
                    SavedData.savesData.tipTerrain += 1;
                    SavedData.Save();
                    showedTipTerrain = true;
                }
            }
        }
    }

    public void CheckForTipDrawMoreCards()
    {
        if (!showedTipCards)
        {
            animator.PerformTween(0);
            text.text = drawMoreCardsText;
            SavedData.savesData.tipCards += 1;
            SavedData.Save();
            showedTipCards = true;
        }
    }

    public void CheckForTipPressingExpand()
    {
        if (!showedPressingExpand)
        {
            animator.PerformTween(0);
            text.text = pressingExpandText;
            SavedData.savesData.tipPressingExpand += 1;
            SavedData.Save();
            showedPressingExpand = true;
            StartCoroutine( Hand.instance.FirstTurnWaitToPlayACard());
        }
    }

    public void MarketSkipped()
    {
        SavedData.savesData.marketSkipped += 1;
        SavedData.Save();
        marketSkipped = true;
    }

    public  void CheckForMarketTip()
    {
        if (!marketTip)
        {
            animator.PerformTween(0);
            text.text = marketText;
            SavedData.savesData.tipMarket += 1;
            SavedData.Save();
            marketTip = true;
        }
    }
}
