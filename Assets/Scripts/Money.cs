using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Money : MonoSingleton<Money>
{
    [SerializeField] TextMeshProUGUI amountText;
    //TODO this should come from heroes
    [SerializeField] TweenAnimator animator;

    int currentAmount;

    private void Start()
    {
        currentAmount = CharacterSelector.firstCharacter.startingMoney + CharacterSelector.secondCharacter.startingMoney;
        amountText.text = currentAmount.ToString();
    }

    public bool TryPaying(int amount)
    {
        if(amount <= currentAmount)
        {
            StartCoroutine(LowerCurrencyAmount(currentAmount, currentAmount - amount));
            currentAmount -= amount;
            return true;
        }
        return false;
    }

    public bool CheckAmount(int amount)
    {
        if(amount <= currentAmount)
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
}
