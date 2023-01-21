using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Base Level Up Descriptions")]
[Serializable]

public class BaseLevelUpDescriptions : ScriptableObject
{
    public List<Character.LevelUpDescription> levelUpDescriptions;
}
