using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.Loc;

public class Mana : MonoSingleton<Mana>
{
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] TextMeshProUGUI incomeText;
    [SerializeField] TweenAnimator animator;
    [SerializeField] int regenAmount;
    [SerializeField] float timeToRegen;
    [SerializeField] LocalizedString baseIncomeText;
    [SerializeField] GameObject manaShield;


    Coroutine regen;
    internal int maxAmount;
    internal int currentAmount;
    internal bool shouldUseManaShield;

    private void Start()
    {
        shouldUseManaShield = CharacterSelector.firstCharacter.characterName == "Mage" || CharacterSelector.secondCharacter.characterName == "Mage";
        manaShield.SetActive(shouldUseManaShield);

        maxAmount = CharacterSelector.firstCharacter.startingMana + CharacterSelector.secondCharacter.startingMana;
        currentAmount = maxAmount;
        incomeText.text = "+" + regenAmount + " " + baseIncomeText;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        amountText.text = currentAmount.ToString() + "/" + maxAmount.ToString();
        manaShield.SetActive(currentAmount >= 50);
    }

    public bool TryPaying(int amount)
    {
        if (amount <= currentAmount)
        {
            StartCoroutine(LowerCurrencyAmount(currentAmount, currentAmount - amount));
            currentAmount -= amount;
            return true;
        }
        return false;
    }
    public bool CheckAmount(int amount)
    {
        if (amount <= currentAmount)
        {
            return true;
        }
        else
        {
            animator.PerformTween(0);
            return false;
        }
    }

    public void AddCurrency(int amount, bool instant)
    {
        if (!instant)
        {
            StartCoroutine(IncreaseCurrencyAmount(currentAmount, Mathf.Clamp(currentAmount + amount, 0, maxAmount)));
        }
        currentAmount = Mathf.Clamp(currentAmount + amount, 0, maxAmount); 
        UpdateInfo();
    }

    IEnumerator IncreaseCurrencyAmount(int startingValue, int finishValue)
    {
        int currentValue = startingValue;
        float timePassed = 0f;
        //Play sound
        //yield return new WaitForSeconds(0.5f);
        while (currentValue < finishValue)
        {
            yield return null;
            timePassed += Time.deltaTime;
            yield return null;
            timePassed += Time.deltaTime;
            currentValue = Mathf.FloorToInt(startingValue + ((finishValue - startingValue) * timePassed));
            amountText.text = currentValue.ToString() + "/" + maxAmount.ToString();
        }
        UpdateInfo();
    }

    IEnumerator LowerCurrencyAmount(int startingValue, int finishValue)
    {
        int currentValue = startingValue;
        float timePassed = 0f;
        while (currentValue > finishValue)
        {
            yield return null;
            timePassed += Time.deltaTime;
            yield return null;
            timePassed += Time.deltaTime;
            currentValue = Mathf.FloorToInt(startingValue - ((startingValue - finishValue) * timePassed));
            amountText.text = currentValue.ToString() + "/" + maxAmount.ToString();
        }
        UpdateInfo();
    }

    public void StartRegen()
    {
        regen = StartCoroutine(Regen());
    }

    public void StopRegen()
    {
        StopCoroutine(regen);
    }

    IEnumerator Regen()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToRegen);
            AddCurrency(regenAmount, true);
        }
    }

    public void AddRegen(int newAmount)
    {
        regenAmount += newAmount;
        incomeText.text = "+" + regenAmount + " " + baseIncomeText;
    }

    public void IncreaseMaxMana(int amount)
    {
        maxAmount += amount;
        currentAmount = Mathf.Min(maxAmount, currentAmount + amount);
        UpdateInfo();
    }

    public void DecreaseMaxMana(int amount)
    {
        maxAmount -= amount;
        currentAmount = Mathf.Min(maxAmount, currentAmount);
        UpdateInfo();
    }

    public void MaxManaAddition(int amount)
    {
        maxAmount += amount;
        currentAmount = Mathf.Min(maxAmount, currentAmount + amount);
        UpdateInfo();
    }
}
