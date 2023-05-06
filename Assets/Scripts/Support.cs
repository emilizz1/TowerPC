using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support : Structure
{
    [SerializeField] GameObject auraTerrain;

    Spot mySpot;
    List<Spot> adjacentSpots;

    public override void Activate(TerrainBonus terrain)
    {
        base.Activate(terrain);

        mySpot = GetComponentInParent<Spot>();
        adjacentSpots = mySpot.myTile.GetAdjacentSpots(mySpot, true);

        foreach(Tile tile in TileManager.instance.GetAdjacentTiles(mySpot.myTile.transform.position))
        {
            foreach(Spot spot in tile.GetAdjacentSpots(mySpot, true))
            {
                adjacentSpots.Add(spot);
            }
        }

        if(adjacentSpots.Count != 4)
        {
            TileManager.OnTilePlaced += NewTileAdded;
        }

        foreach(Spot spot in adjacentSpots)
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
        Instantiate(auraTerrain, spot.transform);
    }
}
