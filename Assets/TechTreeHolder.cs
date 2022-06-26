using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Tech Tree Holder")]
[Serializable]
public class TechTreeHolder : ScriptableObject
{
    public List<Research> researches;
}
