using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.Loc;

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
    [SerializeField] LocalizedString levelText;
    [SerializeField] LocalizedString upgradeText;

    [SerializeField] List<LocalizedString> targetingOptions;

    Tower currentTower;
    bool open;
    Camera myCamera;

    private void Start()
    {
        myCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (open)
            {
                Close();
            }
        }
        if (TowerPlacer.towerToPlace != null) 
        {
            if (TowerPlacer.towerPlaced)
            {
                if (!open && myCamera.ScreenToViewportPoint(Input.mousePosition).x > 0.15f)
                {
                    OpenWindow();
                }
            }
            else
            {
                if (open) 
                {
                    Close(); 
                }
            }
        }
    }

    public void Close()
    {
        if (currentTower != null)
        {
            currentTower.rangeSprite.gameObject.SetActive(false);
        }
        open = false;
        animator.PerformTween(0);
        SoundsController.instance.PlayOnce("Close");
    }

    public void ShowInfo(Tower tower)
    {
        if (currentTower != null && currentTower != tower)
        {
            currentTower.rangeSprite.gameObject.SetActive(false);
        }
        currentTower = tower;

        nameText.text = currentTower.towerName;
        fireRateText.text = "<sprite=1> " + (1f / (currentTower.towerStats[currentTower.currentLevel].fireRate * currentTower.statsMultiplayers.fireRate)).ToString("F1") + "/s";
        damage0Text.text = "<sprite=4> " + (currentTower.towerStats[currentTower.currentLevel].damage[0] * currentTower.statsMultiplayers.damage[0]).ToString("F1");
        damage1Text.text = "<sprite=3> " + (currentTower.towerStats[currentTower.currentLevel].damage[1] * currentTower.statsMultiplayers.damage[1]).ToString("F1");
        damage2Text.text = "<sprite=2> " + (currentTower.towerStats[currentTower.currentLevel].damage[2] * currentTower.statsMultiplayers.damage[2]).ToString("F1");
        rangeText.text = "<sprite=0> " + (currentTower.towerStats[currentTower.currentLevel].range * currentTower.statsMultiplayers.range).ToString("F1");
        level.text = levelText + " " + (currentTower.currentLevel + 1).ToString();
        upgradeCost.transform.parent.parent.parent.gameObject.SetActive(!currentTower.MaxLevel());
        if (currentTower.MaxLevel())
        {
            experienceBar.transform.parent.parent.gameObject.SetActive(false);
        }
        else
        {
            experienceBar.transform.parent.parent.gameObject.SetActive(true);
            experienceText.text = "Exp: " + currentTower.experience + " / " + currentTower.experienceNeeded[currentTower.currentLevel];
            experienceBar.fillAmount = (float)currentTower.experience / currentTower.experienceNeeded[currentTower.currentLevel];
            upgradeCost.text = upgradeText + " " + (currentTower.experienceNeeded[currentTower.currentLevel] - currentTower.experience).ToString();    
        }


        if (string.IsNullOrEmpty(currentTower.specialText))
        {
            specialText.gameObject.SetActive(false);
        }
        else
        {
            specialText.gameObject.SetActive(true);
            specialText.text = currentTower.specialText;
            switch (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(currentTower.towerType))
            {
                case (1):
                    specialText.text = currentTower.specialFirstText;
                    break;
                case (2):
                    specialText.text = currentTower.specialSecondText;
                    break;
            }
        }

        towerImage.sprite = currentTower.image;
        targetSelectText.text = targetingOptions[(int)currentTower.targeting];
    }

    public void OpenWindow()
    {
        if(currentTower == null)
        {
            return;
        }
        currentTower.rangeSprite.gameObject.SetActive(true);
        if (!open)
        {
            animator.PerformTween(1);
            SoundsController.instance.PlayOnce("Open");
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

    public void ShowInfoWithTerrain(Tower tower, List<TerrainBonus> terrains)
    {
        char quata = '"';
        string colorString = "<color=" + quata + "green" + quata + ">";
        ShowInfo(tower);
        TowerStats combinedStats = new TowerStats();
        combinedStats.fireRate = 0f;
        combinedStats.range = 0f;
        combinedStats.damage = new List<float>();
        for (int i = 0; i < 3; i++)
        {
            combinedStats.damage.Add(0f);
        }

        foreach(TerrainBonus terrain in terrains)
        {
            combinedStats.CombineStats(terrain.statsMultiplayers);
        }

        if (combinedStats.range > 0)
        {
            rangeText.text = "<sprite=0> " + colorString +
                (currentTower.towerStats[currentTower.currentLevel].range * (currentTower.statsMultiplayers.range + combinedStats.range)).ToString("F1")
                + " (+" + (combinedStats.range * 100).ToString() + "%)";
            tower.SetupRange(combinedStats.range);
        }
        if (combinedStats.fireRate != 0f)
        {// 1 / 0.33 * (1 - 0) 3.03       1 / 0.33 * (1 - 0) 3.03
            fireRateText.text = "<sprite=1> " + colorString +
                  ( 1f / (currentTower.towerStats[currentTower.currentLevel].fireRate *  (currentTower.statsMultiplayers.fireRate + combinedStats.fireRate))).ToString("F1")
                  + " (+" + ((Mathf.Abs( combinedStats.fireRate)) * 100).ToString() + "%)";
        }
        if (combinedStats.damage[0] > 0)
        {
            damage0Text.text = "<sprite=4> " + colorString +
                (currentTower.towerStats[currentTower.currentLevel].damage[0] * (currentTower.statsMultiplayers.damage[0] + combinedStats.damage[0])).ToString("F1")
                + " (+" + (combinedStats.damage[0] * 100).ToString() + "%)";
        }
        if (combinedStats.damage[1] > 0)
        {
            damage1Text.text = "<sprite=3> " + colorString +
                (currentTower.towerStats[currentTower.currentLevel].damage[1] * (currentTower.statsMultiplayers.damage[1] + combinedStats.damage[1])).ToString("F1")
                + " (+" + (combinedStats.damage[1] * 100).ToString() + "%)";
        }
        if (combinedStats.damage[2] > 0)
        {
            damage2Text.text = "<sprite=2> " + colorString +
                (currentTower.towerStats[currentTower.currentLevel].damage[2] * (currentTower.statsMultiplayers.damage[2] + combinedStats.damage[2])).ToString("F1")
                + " (+" + (combinedStats.damage[2] * 100).ToString() + "%)";
        }
    }

    public void UpgradeTower()
    {
        if (currentTower.MaxLevel())
        {
            return;
        }

        if(Money.instance.TryPaying(currentTower.experienceNeeded[currentTower.currentLevel] - currentTower.experience))
        {
            currentTower.UpgradeTower();
        }
    }
}
