using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mana : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI amountText;
    //TODO this should come from heroes
    [SerializeField] int startingAmount;

    int currentAmount;

    private void Start()
    {
        currentAmount = startingAmount;
        amountText.text = currentAmount.ToString();
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

    public void AddMoney(int amount, bool instant)
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
        SoundsController.instance.PlayOneShot("RM-buy-currency-premium");
        yield return new WaitForSeconds(0.5f);
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
