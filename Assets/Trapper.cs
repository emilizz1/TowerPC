using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapper : Structure
{
    [SerializeField] int radius;
    [SerializeField] float placementTime;
    [SerializeField] GameObject range;

    List<RoadSpot> trapSpots;
    Spot mySpot;

    float timePassed;

    private void Update()
    {
        if (active)
        {
            if (TurnController.currentPhase == TurnController.TurnPhase.EnemyWave)
            {
                timePassed += Time.deltaTime;
                if(timePassed >= placementTime)
                {
                    timePassed = 0f;
                    PlaceTrap();
                }
            }
        }
    }

    void PlaceTrap()
    {
        GameObject newTrap = ObjectPools.instance.GetPool(ObjectPools.PoolNames.trap).GetObject();
        int spotIndex = -1;
        for (int i = 0; i < trapSpots.Count; i++)
        {
            if (!trapSpots[i].taken)
            {
                spotIndex = i;
                break;
            }
        }
        if(spotIndex == -1)
        {
            return;
        }

        Vector3 trapPosition = trapSpots[spotIndex].transform.position;
        newTrap.transform.position = trapPosition;
        newTrap.GetComponent<Trap>().mySpot = trapSpots[spotIndex];
        newTrap.GetComponent<Trap>().Activate(this);
    }

    public override void Activate(TerrainBonus terrain)
    {
        base.Activate(terrain);
        mySpot = GetComponentInParent<Spot>();
        range.SetActive(false);
        GatherSpots();
    }

    void GatherSpots()
    {
        trapSpots = new List<RoadSpot>();
        foreach (RoadSpot emptySpaces in mySpot.myTile.GettAllRoadSpotsInTwoRange(mySpot))
        {
            trapSpots.Add(emptySpaces);
        }

        foreach (Tile tile in TileManager.instance.GetAdjacentTiles(mySpot.myTile.transform.position))
        {
            foreach (RoadSpot emptySpaces in tile.GettAllRoadSpotsInTwoRange(mySpot))
            {
                trapSpots.Add(emptySpaces);
            }
        }

        if (TileManager.instance.GetAdjacentTiles(mySpot.myTile.transform.position).Count < 4)
        {
            TileManager.OnTilePlaced += NewTileAdded;
        }
    }

    public void NewTileAdded(Tile newTile)
    {
        List<Tile> adjacentTiles = TileManager.instance.GetAdjacentTiles(mySpot.myTile.transform.position);
        foreach (Tile adjacentTile in adjacentTiles)
        {
            if(adjacentTile == newTile)
            {
                foreach (RoadSpot emptySpaces in mySpot.myTile.GettAllRoadSpotsInTwoRange(mySpot))
                {
                    trapSpots.Add(emptySpaces);
                }

                if (adjacentTiles.Count >= 4)
                {
                    TileManager.OnTilePlaced -= NewTileAdded;
                }
            }
        }
    }

    public void TrapSprung(Trap trap)
    {

    }
}
