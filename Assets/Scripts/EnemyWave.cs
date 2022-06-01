using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "EnemyWave")]
[Serializable]
public class EnemyWave : ScriptableObject
{
    public List<GameObject> enemies;
}
