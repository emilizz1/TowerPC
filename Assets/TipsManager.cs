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

    bool showedTipTower;
    bool showedTipTerrain;
    bool showedTipCards;

    int towersPlacedOnNotTerrain;

    private void Start()
    {
        showedTipTower = PlayerPrefs.GetInt("TipTower", 0) == 0;
        showedTipTerrain = PlayerPrefs.GetInt("TipTerrain", 0) == 0;
        showedTipCards = PlayerPrefs.GetInt("TipCards", 0) == 0;
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
}
