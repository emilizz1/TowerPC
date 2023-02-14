using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Money : MonoSingleton<Money>
{
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] TextMeshProUGUI incomeText;
    [SerializeField] TweenAnimator animator;
    [SerializeField] List<int> reductions;

    internal int currentAmount;
    internal int passiveIncome = 0;


    private void Start()
    {
        currentAmount = CharacterSelector.firstCharacter.startingMoney + CharacterSelector.secondCharacter.startingMoney;
        amountText.text = currentAmount.ToString();
        UpdateIncome();
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
        if (instant)
        {
            amountText.text = (currentAmount + amount).ToString();
        }
        else
        {
            StartCoroutine(IncreaseCurrencyAmount(currentAmount, currentAmount + amount));
        }
        currentAmount += amount;
        if(currentAmount > 120)
        {
            TipsManager.instance.CheckForTipUpgradeTower();
        }
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
            amountText.text = currentValue.ToString();
        }
        amountText.text = finishValue.ToString();
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
            amountText.text = currentValue.ToString();
        }
        amountText.text = finishValue.ToString();
    }

    public void GetIncome()
    {

        int towerTax = TowerPlacer.allTowers.Count;
        int reduction = reductions[TurnController.currentTurn-1] + passiveIncome;
        int finalAmount = reduction;// - towerTax;

        finalAmount = Mathf.Min(finalAmount, Money.instance.currentAmount);

        if (finalAmount < 0)
        {
            TryPaying(-finalAmount);
        }
        else
        {
            AddCurrency(finalAmount, false);
        }
    }

    public void UpdateIncome()
    {
        int reduction = reductions[TurnController.currentTurn-1] + passiveIncome;
        int finalAmount = reduction;// - towerTax;

        incomeText.text = (finalAmount >= 0 ? "+" : "") + finalAmount.ToString() + " per turn";
    }

    public void AddToPassiveIncome(int amount)
    {
        passiveIncome += amount;
        UpdateIncome();
    }
}
