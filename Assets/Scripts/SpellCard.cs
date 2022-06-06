using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Spell Card")]
[Serializable]
public class SpellCard : Card
{
    public GameObject spellPrefab;
}
