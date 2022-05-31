using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketWindow : MonoSingleton<MarketWindow>
{
    [SerializeField] TweenAnimator animator;

    public void Open()
    {
        animator.PerformTween(1);
        MarketCardManager.instance.DisplayNewMarket();
    }

    public void Close()
    {
        MarketCardManager.instance.CloseMarket();
        animator.PerformTween(0);
        TurnController.FinishedBuying();
    }
}
