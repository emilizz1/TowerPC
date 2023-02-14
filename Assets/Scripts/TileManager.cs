using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileManager : MonoSingleton<TileManager>
{
    [SerializeField] Tile startingTile1;
    [SerializeField] Tile startingTile2;
    [SerializeField] Tile startingTile3;
    [SerializeField] List<GameObject> tilePrefabs;
    [SerializeField] GameObject endTilePrefab;

    const float NEXT_TILE_PLACEMENT_DIFFERENCE = 9f;

    public static event Action<Tile> OnTilePlaced;

    internal List<Tile> tiles;

    List<Vector2> reservedCoordinates;

    private void Start()
    {
        reservedCoordinates = new List<Vector2>();
        reservedCoordinates.Add(new Vector2(0,0));
        tiles = new List<Tile>();

        Tile startingTile = null;

        if (PlayerPrefs.GetInt("Difficulty", 0) == 2)
        {
            startingTile1.gameObject.SetActive(false);
            startingTile2.gameObject.SetActive(false);
            startingTile = startingTile3;
        }
        else if (PlayerPrefs.GetInt("Difficulty", 0) == 1)
        {
            startingTile1.gameObject.SetActive(false);
            startingTile3.gameObject.SetActive(false);
            startingTile = startingTile2;

        }        
        else
        {
            startingTile2.gameObject.SetActive(false);
            startingTile3.gameObject.SetActive(false);
            startingTile = startingTile1;
        }


        tiles.Add(startingTile);
        FillReservedCoordinates(startingTile, startingTile.coordinates);
        ChangeButtonInteractability(false);
        OnTilePlaced = null;
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
        OnTilePlaced?.Invoke(newTile);
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

    public void CheckForMisplacedTowers()
    {
        foreach(Tile tile in tiles)
        {
            foreach(Spot spot in tile.allSpots)
            {
                spot.DestroyTowerPreview();
            }
        }
    }

    public Tile GetClosestTile(Vector3 pos)
    {
        Tile closest = tiles[0];
        float closestDist = Vector3.Distance(pos, tiles[0].transform.position);
        foreach (Tile tile in tiles)
        {
            if(Vector3.Distance(pos, tile.transform.position) < closestDist)
            {
                closest = tile;
                closestDist = Vector3.Distance(pos, tile.transform.position);
            }
        }
        return closest;
    }
}
