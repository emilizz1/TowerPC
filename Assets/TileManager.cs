using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileManager : MonoSingleton<TileManager>
{
    [SerializeField] Tile startingTile;
    [SerializeField] List<GameObject> tilePrefabs;
    [SerializeField] GameObject endTilePrefab;

    const float NEXT_TILE_PLACEMENT_DIFFERENCE = 11f;

    internal List<Tile> tiles;

    List<Vector2> reservedCoordinates;

    private void Start()
    {
        reservedCoordinates = new List<Vector2>();
        reservedCoordinates.Add(new Vector2(0,0));
        tiles = new List<Tile>();
        tiles.Add(startingTile);
        FillReservedCoordinates(startingTile, startingTile.coordinates);
        ChangeButtonInteractability(false);
    }

    public void PlaceNewTile(Vector2 from, Vector2 to, Lane prevLane)
    {
        Vector3 posToPlace = new Vector3();
        posToPlace.x = to.x * NEXT_TILE_PLACEMENT_DIFFERENCE;
        posToPlace.z = to.y * NEXT_TILE_PLACEMENT_DIFFERENCE;

        Vector3 rotation = new Vector3();
        if(from.x < to.x)
        {
            rotation.y += 90f;
        }
        else if(from.x > to.x)
        {
            rotation.y -= 90f;
        }
        else if(from.y > to.y)
        {
            rotation.y += 180f;
        }

        GameObject tileToPlace = GetTileToPlace(to, rotation);
        GameObject newObject = Instantiate(tileToPlace, posToPlace, Quaternion.Euler(rotation), transform);
        Tile newTile = newObject.GetComponent<Tile>();
        newTile.coordinates = to;
        foreach(Lane lane in newTile.lanes)
        {
            lane.prevLane = prevLane;
        }
        //FillReservedCoordinates(newTile);

        tiles.Add(newTile);
        EnemyManager.instance.SpawnNextWave();
        ActivateExpandButtons();
    }

    void FillReservedCoordinates(Tile tile, Vector2 startingCoordinate)
    {
        foreach(Vector2 coordinate in tile.GetAllLaneEnds())
        {
            reservedCoordinates.Add(coordinate + startingCoordinate);
        }
    }

    GameObject GetTileToPlace(Vector2 placementCoordinate, Vector3 rotation)
    {
        int startingIndex = UnityEngine.Random.Range(0, tilePrefabs.Count);

        for (int i = 0; i < tilePrefabs.Count; i++)
        {
            int index = startingIndex + i >= tilePrefabs.Count ? startingIndex + i - tilePrefabs.Count : startingIndex + i;
            tilePrefabs[index].transform.rotation = Quaternion.Euler(rotation);
            if (AreAllSpotsFree(tilePrefabs[index].GetComponent<Tile>().GetAllLaneEnds(), placementCoordinate))
            {
                FillReservedCoordinates(tilePrefabs[index].GetComponent<Tile>(), placementCoordinate);
                return tilePrefabs[index];
            }
        }

        return endTilePrefab;
    }

    bool AreAllSpotsFree(List <Vector2> checkSpots, Vector2 currentCoordinate)
    {
        foreach(Vector2 spot in checkSpots)
        {
            if (reservedCoordinates.Contains(spot + currentCoordinate))
            {
                return false;
            }
        }
        return true;
    }

    public void ActivateExpandButtons()
    {
        foreach (Tile tile in tiles)
        {
            tile.CheckExpandButtons();
        }
    }

    public void ChangeButtonInteractability(bool interactive)
    {
        foreach (Tile tile in tiles)
        {
            tile.SetButtonInteractive(interactive);
        }
    }

    public List<Lane> GetAllOpenLanes()
    {
        List<Lane> openLanes = new List<Lane>();
        foreach (Tile tile in tiles)
        {
            foreach (Lane lane in tile.lanes)
            {
                if (lane.open)
                {
                    openLanes.Add(lane);
                }
            }
        }
        return openLanes;
    }
}
