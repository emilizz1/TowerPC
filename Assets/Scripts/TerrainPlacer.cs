using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPlacer : MonoSingleton<TerrainPlacer>
{
    public float spawnChance = 0.01f;

    [SerializeField] List<GameObject> terrains;

    float currentSpawnChance;

    public void GetTerrain(Spot spot, bool random = true)
    {
        currentSpawnChance += spawnChance;
        if (!random || Random.Range(0f,1f) < currentSpawnChance * spot.terrainSpawnChance)
        {
            Instantiate(terrains[Random.Range(0, terrains.Count)], spot.transform);
            currentSpawnChance = 0f;
        }
    }
}
