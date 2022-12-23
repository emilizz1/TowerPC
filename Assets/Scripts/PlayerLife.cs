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
    int maxIgnoredEachRound;
    int currentIgnoredEachRound;

    Color fillNormalColor;

    private void Start()
    {
        maxHp = CharacterSelector.firstCharacter.startingMaxHealth + CharacterSelector.secondCharacter.startingMaxHealth;
        currentHP = maxHp;
        fillNormalColor = fill.color;
        UpdateHealth();
    }

    void UpdateHealth()
    {
        amountText.text = currentHP + " / " + maxHp;
        fill.fillAmount = (float)currentHP / maxHp;
        if(currentIgnoredEachRound > 0)
        {
            fill.color = Color.grey;
        }
    }

    public void ChangeHealthAmount(int change)
    {
        if(change < 0 && currentIgnoredEachRound > 0)
        {
            currentIgnoredEachRound--;
            SoundsController.instance.PlayOneShot("Mana");
            UpdateHealth();
            return;
        }
        currentHP += change;
        UpdateHealth();
        SoundsController.instance.PlayOneShot("Damaged");
        if(currentHP <= 0)
        {
            Lost();
        }
    }

    public void MaxHealthAddition(int amount)
    {
        maxHp += amount;
        currentHP = Mathf.Min(maxHp, currentHP + amount);
        UpdateHealth();
    }

    public void Regen()
    {
        currentHP = Mathf.Clamp(currentHP + regen, 0, maxHp);
        UpdateHealth();
    }

    void Lost()
    {
        SceneManager.LoadScene(SceneManager.LOST);
    }

    public void IncreaseIgnorAmount(int amount)
    {
        maxIgnoredEachRound += amount;
        currentIgnoredEachRound += amount;
        UpdateHealth();
    }

    public void NewRound()
    {
        currentIgnoredEachRound = maxIgnoredEachRound;
        UpdateHealth();
    }
}
