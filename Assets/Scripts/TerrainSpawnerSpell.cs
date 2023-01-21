using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawnerSpell : Spell
{
    [SerializeField] int terrainToSpawn;
    [SerializeField] ParticleSystem particles;
    [SerializeField] GameObject rangeIndicator;

    public override void Activate()
    {
        base.Activate();
        Tile myTile = TileManager.instance.GetClosestTile(transform.position);
        List<Spot> availableSpots = new List<Spot>();
        foreach (Spot spot in myTile.allSpots)
        {
            if (!spot.objBuilt && spot.terrainBonus.Count == 0 && spot.isActiveAndEnabled)
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

        rangeIndicator.gameObject.SetActive(false);

        particles.Stop();
    }
}
