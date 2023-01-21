using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/Marketplace")]
[Serializable]
public class ResearchMarketplaceChanges : Research
{
    [SerializeField] MarketplaceChanges changes;

    [Serializable]
    public enum MarketplaceChanges
    {
        noCardBuying,
        noLevel0Cards
    }

    public override void Researched()
    {
        base.Researched();
        if(changes == MarketplaceChanges.noCardBuying)
        {
            MarketCardManager.instance.NoMoreCardBuying();

        }
        else if(changes == MarketplaceChanges.noLevel0Cards)
        {
            MarketCardManager.instance.RemoveLevel0Cards();
        }
    }
}
