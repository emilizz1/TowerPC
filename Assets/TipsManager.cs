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

    internal bool marketSkipped;

    bool showedTipTower;
    bool showedTipTerrain;
    bool showedTipCards;
    bool showedPressingExpand;

    int towersPlacedOnNotTerrain;

    private void Start()
    {
        showedTipTower = PlayerPrefs.GetInt("TipTower", 0) == 0;
        showedTipTerrain = PlayerPrefs.GetInt("TipTerrain", 0) == 0;
        showedTipCards = PlayerPrefs.GetInt("TipCards", 0) == 0;
        showedPressingExpand = PlayerPrefs.GetInt("PressingExpand", 0) == 0;
        marketSkipped = PlayerPrefs.GetInt("marketSkipped", 0) == 0;
    }


    public void CheckForTipUpgradeTower()
    {
        if (!showedTipTower)
        {
            animator.PerformTween(0);
            text.text = upgradeTowerText;
            PlayerPrefs.SetInt("TipTower", 1);
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
                if(towersPlacedOnNotTerrain > 4)
                {
                    animator.PerformTween(0);
                    text.text = terrainText;
                    PlayerPrefs.SetInt("TipTerrain", 1);
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
            PlayerPrefs.SetInt("TipCards", 1);
            showedTipCards = true;
        }
    }

    public void CheckForTipPressingExpand()
    {
        if (!showedPressingExpand)
        {
            animator.PerformTween(0);
            text.text = pressingExpandText;
            PlayerPrefs.SetInt("PressingExpand", 1);
            showedPressingExpand = true;
            StartCoroutine( Hand.instance.FirstTurnWaitToPlayACard());
        }
    }

    public void MarketSkipped()
    {
        PlayerPrefs.SetInt("marketSkipped", 1);
        marketSkipped = true;
    }
}
