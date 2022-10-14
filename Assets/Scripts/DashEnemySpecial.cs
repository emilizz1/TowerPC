using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemySpecial : EnemySpecial
{
    [SerializeField] float additionalSpeed;

    public override void UseSpecial()
    {
        GetComponent<EnemyMovement>().additionalSpeed += additionalSpeed;
        base.UseSpecial();
    }

    internal override void SpecialFinished()
    {
        if (currentlyInUse)
        {
            GetComponent<EnemyMovement>().additionalSpeed -= additionalSpeed;
        }
        base.SpecialFinished();
    }
}
