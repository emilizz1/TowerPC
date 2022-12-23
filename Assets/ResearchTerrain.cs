using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Research/Terrain")]
[Serializable]
public class ResearchTerrain : Research
{
    [SerializeField] float extraTerrainSpawnChance;

    public override void Researched()
    {
        base.Researched();
        TerrainPlacer.instance.spawnChance += extraTerrainSpawnChance;
    }
}
