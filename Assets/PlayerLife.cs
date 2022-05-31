using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLife : MonoSingleton<PlayerLife>
{
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] Image fill;

    [SerializeField] int startingHealth;

    int maxHp;
    int currentHP;

    private void Start()
    {
        maxHp = startingHealth;
        currentHP = startingHealth;
        UpdateHealth();
    }

    void UpdateHealth()
    {
        amountText.text = currentHP + " / " + maxHp;
        fill.fillAmount = (float)currentHP / maxHp;
    }

    public void ChangeHealthAmount(int change)
    {
        currentHP += change;
        UpdateHealth();
    }
}
