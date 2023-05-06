using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : Tower
{
    float rangeUpdateTime;
    int previouseActionCardsPlayed;

    public override void Update()
    {
        rangeUpdateTime += Time.deltaTime;
        if (rangeUpdateTime > 2f && SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 2)
        {
            rangeUpdateTime = 0f;
            if (previouseActionCardsPlayed != TurnController.actionsPlayed)
            {
                SetupRange();
                previouseActionCardsPlayed = TurnController.actionsPlayed;
            }
        }
        base.Update();
    }

    internal override List<float> GetDamageMultiplied()
    {
        if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 1)
        {
            List<float> finalDamage = base.GetDamageMultiplied();

            for (int i = 0; i < 3; i++)
            {
                finalDamage[i] += (Money.instance.currentAmount / 20f);
            }

            return finalDamage;
        }

        return base.GetDamageMultiplied();
    }

    public override void SetupRange(float additionalRange = 0)
    {
        if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(towerType) == 2)
        {
            additionalRange += TurnController.actionsPlayed * 0.05f;
        }
        base.SetupRange(additionalRange);
    }
}
