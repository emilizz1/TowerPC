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

    public void Close()
    {
        animator.PerformTween(0);
    }

    public void ShowInfo(Tower tower)
    {
        animator.PerformTween(1);
        nameText.text = tower.towerName;
        fireRateText.text = "Fire Rate: " + tower.fireRate.ToString();
        damage0Text.text = "Health Damage: " + tower.damage.ToString();
        damage1Text.text = "Armor Damage: " + tower.damage.ToString();
        damage2Text.text = "Shield Damage: " + tower.damage.ToString();
        rangeText.text = "Range: " + tower.range.ToString();
        towerImage.sprite = tower.image;
    }
}
