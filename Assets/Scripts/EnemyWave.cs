using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "EnemyWave")]
[Serializable]
public class EnemyWave : ScriptableObject
{
    public List<EnemyWaveData> enemies;
    public List<int> enemiesSkippedForFirstPlays;

    [Serializable]
    public struct EnemyWaveData
    {
        public ObjectPools.PoolNames type;
        public int amount;
    }
}
