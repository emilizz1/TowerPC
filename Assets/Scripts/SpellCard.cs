using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using I2.Loc;

[CreateAssetMenu(menuName = "Spell Card")]
[Serializable]
public class SpellCard : Card
{
    public GameObject spellPrefab;
    public string advancedDescription;
    public LocalizedString staysFor;
    [SerializeField] int spellDuration;

    public override string GetDescription()
    {
        string correctStaysFor = "";
        if (spellDuration > 1)
        {
           correctStaysFor =  staysFor.ToString().Replace("#", spellDuration.ToString());
        }
        return base.GetDescription() + "/n" + correctStaysFor;
    }
}
