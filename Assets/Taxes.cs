using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Taxes : MonoSingleton<Taxes>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI towerTaxText;
    [SerializeField] TextMeshProUGUI incomeText;
    [SerializeField] TextMeshProUGUI finalTaxText;

    [SerializeField] TweenAnimator animator;

    [SerializeField] List<int> reductions;

    internal int passiveIncome;

    public void PayTaxes()
    {
        animator.PerformTween(0);

        int towerTax = TowerPlacer.allTowers.Count;
        int reduction = reductions[TurnController.currentTurn] + passiveIncome;
        int finalAmount = reduction - towerTax;

        finalAmount = Mathf.Min(finalAmount, Money.instance.currentAmount);

        if (finalAmount < 0)
        {
            Money.instance.TryPaying(-finalAmount);
        }
        else
        {
            Money.instance.AddCurrency(finalAmount, false);
        }

        StartCoroutine(CloseAfterTime(5f));
    }

    IEnumerator CloseAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        animator.PerformTween(1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.PerformTween(0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.PerformTween(1);
    }

    public void UpdateInfo()
    {
        int towerTax = TowerPlacer.allTowers.Count;
        int reduction = reductions[TurnController.currentTurn] + passiveIncome;

        towerTaxText.text = towerTax.ToString();
        incomeText.text = reduction.ToString();

        int finalAmount = reduction - towerTax;

        finalTaxText.text = finalAmount.ToString();
    }
}
