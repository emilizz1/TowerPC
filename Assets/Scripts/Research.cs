using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Research")]
[Serializable]
public class Research : ScriptableObject
{
    public Sprite sprite;
    public int timeToResearch;
    public string explanation;
}
