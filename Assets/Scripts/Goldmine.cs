using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goldmine : Structure
{
    [SerializeField] int goldPerWave;
    [SerializeField] GameObject ruinTerrain;

    Spot mySpot;
    List<Spot> adjacentSpots;

    public override void Activate(TerrainBonus terrain)
    {
        base.Activate(terrain);
        Money.instance.AddToPassiveIncome(goldPerWave);
        if (terrain != null)
        {
            if (terrain.type == TerrainBonus.TerrainType.mountain)
            {
                Money.instance.AddToPassiveIncome(goldPerWave);
            }
            else if (terrain.type == TerrainBonus.TerrainType.ruin)
            {
                Money.instance.AddToPassiveIncome(-Mathf.RoundToInt(goldPerWave / 2f));
            }
        }

        mySpot = GetComponentInParent<Spot>();
        adjacentSpots = mySpot.myTile.GetAdjacentSpotsInTwoRange(mySpot);

        foreach (Tile tile in TileManager.instance.GetAdjacentTiles(mySpot.myTile.transform.position))
        {
            foreach (Spot spot in tile.GetAdjacentSpots(mySpot, true))
            {
                adjacentSpots.Add(spot);
            }
        }

        if (adjacentSpots.Count != 12)
        {
            TileManager.OnTilePlaced += NewTileAdded;
        }

        foreach (Spot spot in adjacentSpots)
        {
            AddBonus(spot);
        }
    }

    public void NewTileAdded(Tile newTile)
    {
        List<Spot> newAdjacentSpots = newTile.GetAdjacentSpots(mySpot, true);
        if (newAdjacentSpots.Count > 0)
        {
            adjacentSpots.Add(newAdjacentSpots[0]);
            AddBonus(newAdjacentSpots[0]);
            if (adjacentSpots.Count == 4)
            {
                TileManager.OnTilePlaced -= NewTileAdded;
            }
        }
    }


    void AddBonus(Spot spot)
    {
        Instantiate(ruinTerrain, spot.transform);
    }
}
