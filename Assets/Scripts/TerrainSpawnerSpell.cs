using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawnerSpell : Spell
{
    [SerializeField] int terrainToSpawn;
    [SerializeField] ParticleSystem particles;

    public override void Activate()
    {
        Debug.Log("Activated");
        base.Activate();
        Tile myTile = TileManager.instance.GetClosestTile(transform.position);
        List<Spot> availableSpots = new List<Spot>();
        foreach (Spot spot in myTile.allSpots)
        {
            if (!spot.objBuilt && spot.terrainBonus == null && spot.isActiveAndEnabled)
            {
                availableSpots.Add(spot);
            }
        }

        for (int i = 0; i < terrainToSpawn; i++)
        {
            if (availableSpots.Count == 0)
            {
                return;
            }
            Spot randomSpot = availableSpots[Random.Range(0, availableSpots.Count)];
            TerrainPlacer.instance.GetTerrain(randomSpot, false);
            availableSpots.Remove(randomSpot);
        }

        particles.Stop();
    }
}