using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneTower : Tower
{
    internal override List<float> GetDamageMultiplied()
    {
        List<float> normal = base.GetDamageMultiplied();

        if (currentTarget.GetComponent<Debuff>())
        {
            for (int i = 0; i < normal.Count; i++)
            {
                normal[i] *= 2;
            }
        }

        return normal;
    }
}
