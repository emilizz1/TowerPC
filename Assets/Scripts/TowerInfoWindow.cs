using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerInfoWindow : MonoSingleton<TowerInfoWindow>
{
    [SerializeField] TweenAnimator animator;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI fireRateText;
    [SerializeField] TextMeshProUGUI damage0Text;
    [SerializeField] TextMeshProUGUI damage1Text;
    [SerializeField] TextMeshProUGUI damage2Text;
    [SerializeField] TextMeshProUGUI rangeText;
    [SerializeField] Image towerImage;
    [SerializeField] TextMeshProUGUI targetSelectText;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image experienceBar;

    [SerializeField] List<string> targetingOptions;

    Tower currentTower;

    public void Close()
    {
        animator.PerformTween(0);
    }

    public void ShowInfo(Tower tower)
    {
        currentTower = tower;
        animator.PerformTween(1);
        nameText.text = currentTower.towerName;
        fireRateText.text = "Fire Rate: " + (currentTower.towerStats[currentTower.currentLevel].fireRate * currentTower.statsMultiplayers.fireRate).ToString();
        damage0Text.text = "Health Damage: " + (currentTower.towerStats[currentTower.currentLevel].damage[0] * currentTower.statsMultiplayers.damage[0]).ToString();
        damage1Text.text = "Armor Damage: " + (currentTower.towerStats[currentTower.currentLevel].damage[1] * currentTower.statsMultiplayers.damage[1]).ToString();
        damage2Text.text = "Shield Damage: " + (currentTower.towerStats[currentTower.currentLevel].damage[2] * currentTower.statsMultiplayers.damage[2]).ToString();
        rangeText.text = "Range: " + (currentTower.towerStats[currentTower.currentLevel].range * currentTower.statsMultiplayers.range).ToString();
        experienceText.text = "Exp: " + currentTower.experience + " / " + currentTower.experienceNeeded[currentTower.currentLevel];
        experienceBar.fillAmount = (float)currentTower.experience / currentTower.experienceNeeded[currentTower.currentLevel];
        towerImage.sprite = currentTower.image;
        targetSelectText.text = targetingOptions[(int)currentTower.targeting];
    }

    public void ChangedTargeting(bool right)
    {
        int currentTargeting = (int)currentTower.targeting;
        currentTargeting = right ? currentTargeting + 1 : currentTargeting - 1;
        if (currentTargeting >= System.Enum.GetValues(typeof(Tower.TargetSelectOptions)).Length)
        {
            currentTargeting = 0;
        }
        else if(currentTargeting < 0)
        {
            currentTargeting = System.Enum.GetValues(typeof(Tower.TargetSelectOptions)).Length - 1;
        }
        currentTower.targeting = (Tower.TargetSelectOptions)currentTargeting;
        targetSelectText.text = targetingOptions[currentTargeting];
    }

    public void UpdateInfo(Tower tower)
    {
        if(currentTower  == tower)
        {
            ShowInfo(tower);
        }
    }
}
