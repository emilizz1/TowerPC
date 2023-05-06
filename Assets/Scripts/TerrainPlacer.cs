using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPlacer : MonoSingleton<TerrainPlacer>
{
    public float spawnChance = 0.01f;

    [SerializeField] List<GameObject> terrains;
    [SerializeField] GameObject fungusTerrain;
    [SerializeField] GameObject waterSpotsTerrain;
    [SerializeField] GameObject spikyPlantTerrain;

    float currentSpawnChance;
    Vector3 offset = new Vector3(0f, 0.5f, 0f);

    public void GetTerrain(Spot spot, bool random = true)
    {
        currentSpawnChance += spawnChance;
        if (!random || Random.Range(0f,1f) < currentSpawnChance * spot.terrainSpawnChance)
        {
            Instantiate(terrains[Random.Range(0, terrains.Count)], spot.transform.position + offset, RotationManager.instance.GetRotation(), spot.transform);
            RotationManager.instance.Rotated();
            currentSpawnChance = 0f;
        }
    }

    public void DoubleBonuses()
    {
        foreach(GameObject terrain in terrains)
        {
            terrain.GetComponent<TerrainBonus>().DoubleBonuses();
        }
    }

    public void PopulateFungus()
    {
        foreach(Tile tile in TileManager.instance.tiles)
        {
            if(Random.Range(0f, 1f) < 0.5f)
            {
                List<Spot> freeSpots = new List<Spot>();
                foreach (Spot spot in tile.allSpots)
                {
                    if (spot.Empty())
                    {
                        freeSpots.Add(spot);
                    }
                }
                Spot chosenSpot = freeSpots[Random.Range(0, freeSpots.Count)];
                Instantiate(fungusTerrain, chosenSpot.transform.position + offset, RotationManager.instance.GetRotation(), chosenSpot.transform);
                RotationManager.instance.Rotated();
            }
        }
    }

    public void PopulateWaterSpots(Tile tile)
    {
        List<Spot> freeSpots = new List<Spot>();
        foreach (Spot spot in tile.allSpots)
        {
            if (spot.Empty() && !spot.CornerSpot())
            {
                freeSpots.Add(spot);
            }
        }
        int waterSpotsCount = Random.Range(3, 6);
        Spot chosenSpot;
        for (int i = 0; i < waterSpotsCount; i++)
        {
            chosenSpot = freeSpots[Random.Range(0, freeSpots.Count)];
            freeSpots.Remove(chosenSpot);
            Instantiate(waterSpotsTerrain, chosenSpot.transform.position + offset, Quaternion.identity, chosenSpot.transform);
        }
    }

    public void PopulateSpikyPlant(Tile tile)
    {
        foreach(RoadSpot roadSpot in tile.roadSpots)
        {
            if(Random.Range(0f, 1f) <= 0.1f)
            {
                Instantiate(spikyPlantTerrain, roadSpot.transform.position, Quaternion.identity, roadSpot.transform);
            }
        }
    }
}
