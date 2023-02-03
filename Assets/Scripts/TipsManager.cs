using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TipsManager : MonoSingleton<TipsManager>
{
    [SerializeField] TweenAnimator animator;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] string upgradeTowerText;
    [SerializeField] string terrainText;
    [SerializeField] string drawMoreCardsText;
    [SerializeField] string pressingExpandText;
    [SerializeField] string marketText;

    internal bool marketSkipped;

    bool showedTipTower;
    bool showedTipTerrain;
    bool showedTipCards;
    bool showedPressingExpand;
    bool marketTip;

    int towersPlacedOnNotTerrain;

    private void Start()
    {
        showedTipTower = PlayerPrefs.GetInt("TipTower", 0) == 2;
        showedTipTerrain = PlayerPrefs.GetInt("TipTerrain", 0) == 2;
        showedTipCards = PlayerPrefs.GetInt("TipCards", 0) == 0;
        showedPressingExpand = PlayerPrefs.GetInt("PressingExpand", 0) == 2;
        marketSkipped = PlayerPrefs.GetInt("marketSkipped", 0) == 2;
        marketTip = PlayerPrefs.GetInt("marketTip", 0) == 2;
    }


    public void CheckForTipUpgradeTower()
    {
        if (!showedTipTower)
        {
            animator.PerformTween(0);
            text.text = upgradeTowerText;
            PlayerPrefs.SetInt("TipTower", PlayerPrefs.GetInt("TipTower", 0) + 1);
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
                    PlayerPrefs.SetInt("TipTerrain", PlayerPrefs.GetInt("TipTerrain", 0) + 1 );
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
            PlayerPrefs.SetInt("TipCards", PlayerPrefs.GetInt("TipCards", 0) + 1);
            showedTipCards = true;
        }
    }

    public void CheckForTipPressingExpand()
    {
        if (!showedPressingExpand)
        {
            animator.PerformTween(0);
            text.text = pressingExpandText;
            PlayerPrefs.SetInt("PressingExpand", PlayerPrefs.GetInt("PressingExpand", 0) + 1);
            showedPressingExpand = true;
            StartCoroutine( Hand.instance.FirstTurnWaitToPlayACard());
        }
    }

    public void MarketSkipped()
    {
        PlayerPrefs.SetInt("marketSkipped", PlayerPrefs.GetInt("marketSkipped", 0) + 1);
        marketSkipped = true;
    }

    public  void CheckForMarketTip()
    {
        if (!marketTip)
        {
            animator.PerformTween(0);
            text.text = marketText;
            PlayerPrefs.SetInt("marketTip", PlayerPrefs.GetInt("marketTip", 0) + 1);
            marketTip = true;
        }
    }
}
