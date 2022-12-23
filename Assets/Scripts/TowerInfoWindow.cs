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
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI specialText;
    [SerializeField] TextMeshProUGUI upgradeCost;

    [SerializeField] List<string> targetingOptions;

    Tower currentTower;
    bool open;

    public void Close()
    {
        if (currentTower != null)
        {
            currentTower.rangeSprite.gameObject.SetActive(false);
        }
        open = false;
        animator.PerformTween(0);
    }

    public void ShowInfo(Tower tower)
    {
        if (currentTower != null && currentTower != tower)
        {
            currentTower.rangeSprite.gameObject.SetActive(false);
        }
        currentTower = tower;

        nameText.text = currentTower.towerName;
        fireRateText.text = "Fire Rate: " + ((1 - (currentTower.towerStats[currentTower.currentLevel].fireRate * (1 - currentTower.statsMultiplayers.fireRate))) * 10).ToString("F1");
        damage0Text.text = "Health Damage: " + (currentTower.towerStats[currentTower.currentLevel].damage[0] * currentTower.statsMultiplayers.damage[0]).ToString("F1");
        damage1Text.text = "Armor Damage: " + (currentTower.towerStats[currentTower.currentLevel].damage[1] * currentTower.statsMultiplayers.damage[1]).ToString("F1");
        damage2Text.text = "Shield Damage: " + (currentTower.towerStats[currentTower.currentLevel].damage[2] * currentTower.statsMultiplayers.damage[2]).ToString("F1");
        rangeText.text = "Range: " + (currentTower.towerStats[currentTower.currentLevel].range * currentTower.statsMultiplayers.range).ToString("F1");
        level.text = "Level:" + (currentTower.currentLevel + 1).ToString();
        if (tower.currentLevel == tower.experienceNeeded.Count)
        {
            experienceText.gameObject.SetActive(false);
            experienceBar.gameObject.SetActive(false);
        }
        else
        {
            experienceText.gameObject.SetActive(true);
            experienceBar.gameObject.SetActive(true);
            experienceText.text = "Exp: " + currentTower.experience + " / " + currentTower.experienceNeeded[currentTower.currentLevel];
            experienceBar.fillAmount = (float)currentTower.experience / currentTower.experienceNeeded[currentTower.currentLevel];
        }

        upgradeCost.text = "Upgrade " + (currentTower.experienceNeeded[currentTower.currentLevel] - currentTower.experience).ToString();
        specialText.text = currentTower.specialText;

        towerImage.sprite = currentTower.image;
        targetSelectText.text = targetingOptions[(int)currentTower.targeting];
    }

    public void OpenWindow()
    {
        if (!open)
        {
            currentTower.rangeSprite.gameObject.SetActive(true);
            animator.PerformTween(1);
            open = true;
        }
    }

    public void ChangedTargeting(bool right)
    {
        int currentTargeting = (int)currentTower.targeting;
        currentTargeting = right ? currentTargeting + 1 : currentTargeting - 1;
        if (currentTargeting >= System.Enum.GetValues(typeof(Tower.TargetSelectOptions)).Length)
        {
            currentTargeting = 0;
        }
        else if (currentTargeting < 0)
        {
            currentTargeting = System.Enum.GetValues(typeof(Tower.TargetSelectOptions)).Length - 1;
        }
        currentTower.targeting = (Tower.TargetSelectOptions)currentTargeting;
        targetSelectText.text = targetingOptions[currentTargeting];
    }

    public void UpdateInfo(Tower tower)
    {
        if (currentTower == tower)
        {
            ShowInfo(tower);
        }
    }

    public void ShowInfoWithTerrain(Tower tower, TerrainBonus terrain)
    {
        char quata = '"';
        string colorString = "<color=" + quata + "green" + quata + ">";
        ShowInfo(tower);
        if (terrain.statsMultiplayers.range > 0)
        {
            rangeText.text = "Range: " + colorString +
                (currentTower.towerStats[currentTower.currentLevel].range * (currentTower.statsMultiplayers.range + terrain.statsMultiplayers.range)).ToString("F1")
                + " (+" + (terrain.statsMultiplayers.range * 100).ToString() + "%)";
        }
        if (terrain.statsMultiplayers.fireRate > 0)
        {
            fireRateText.text = "Fire Rate: " + colorString +
                  ((1 - (currentTower.towerStats[currentTower.currentLevel].fireRate * (1 - (currentTower.statsMultiplayers.fireRate + terrain.statsMultiplayers.fireRate)))) * 10f).ToString("F1")
                  + " (+" + (terrain.statsMultiplayers.fireRate * 100).ToString() + "%)";
        }
        if (terrain.statsMultiplayers.damage[0] > 0)
        {
            damage0Text.text = "Health Damage: " + colorString +
                (currentTower.towerStats[currentTower.currentLevel].damage[0] * (currentTower.statsMultiplayers.damage[0] + terrain.statsMultiplayers.damage[0])).ToString("F1")
                + " (+" + (terrain.statsMultiplayers.damage[0] * 100).ToString() + "%)";
        }
        if (terrain.statsMultiplayers.damage[1] > 0)
        {
            damage1Text.text = "Armor Damage: " + colorString +
                (currentTower.towerStats[currentTower.currentLevel].damage[1] * (currentTower.statsMultiplayers.damage[1] + terrain.statsMultiplayers.damage[1])).ToString("F1")
                + " (+" + (terrain.statsMultiplayers.damage[1] * 100).ToString() + "%)";
        }
        if (terrain.statsMultiplayers.damage[2] > 0)
        {
            damage2Text.text = "Shield Damage: " + colorString +
                (currentTower.towerStats[currentTower.currentLevel].damage[2] * (currentTower.statsMultiplayers.damage[2] + terrain.statsMultiplayers.damage[2])).ToString("F1")
                + " (+" + (terrain.statsMultiplayers.damage[2] * 100).ToString() + "%)";
        }
    }

    public void UpgradeTower()
    {
        if(Money.instance.TryPaying(currentTower.experienceNeeded[currentTower.currentLevel] - currentTower.experience))
        {
            currentTower.UpgradeTower();
        }
    }
}
