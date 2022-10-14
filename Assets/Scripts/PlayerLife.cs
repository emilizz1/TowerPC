using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLife : MonoSingleton<PlayerLife>
{
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] Image fill;

    internal int regen;

    int maxHp;
    int currentHP;

    private void Start()
    {
        maxHp = CharacterSelector.firstCharacter.startingMaxHealth + CharacterSelector.secondCharacter.startingMaxHealth;
        currentHP = maxHp;
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

    public void MaxHealthMultiplied(int amount)
    {
        currentHP += maxHp * (amount - 1);
        maxHp *= amount;
        UpdateHealth();
    }

    public void Regen()
    {
        currentHP = Mathf.Clamp(currentHP + regen, 0, maxHp);
        UpdateHealth();
    }
}
