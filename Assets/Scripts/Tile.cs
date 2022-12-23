using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Lane
{
    public Spot spawnPoint;
    public List<Spot> path;
    public GameObject expandCanvas;
    public Button button;
    public bool open = true;
    internal Tile myTile;
    internal Lane prevLane;
}

public class Tile : MonoBehaviour
{
    public List<Lane> lanes;
    public List<Spot> allSpots;

    internal Vector2 coordinates;

    private void Start()
    {
        foreach (Lane lane in lanes)
        {
            lane.myTile = this;
        }

        foreach (Spot spot in allSpots)
        {
            if (spot.gameObject.activeSelf && spot.terrainBonus == null)
            {
                TerrainPlacer.instance.GetTerrain(spot);
            }
        }
    }

    public void PressedExpand(int index)
    {
        lanes[index].open = false;

        SoundsController.instance.PlayOneShot("Click");

        Vector2 placementCoordinates = coordinates;

        if (transform.position.x + 1 < lanes[index].spawnPoint.transform.position.x)
        {
            placementCoordinates.x += 1;
        }
        else if (transform.position.x - 1 > lanes[index].spawnPoint.transform.position.x)
        {
            placementCoordinates.x -= 1;
        }
        else if (transform.position.z + 1 < lanes[index].spawnPoint.transform.position.z)
        {
            placementCoordinates.y += 1;
        }
        else if (transform.position.z - 1 > lanes[index].spawnPoint.transform.position.z)
        {
            placementCoordinates.y -= 1;
        }

        TileManager.instance.PlaceNewTile(coordinates, placementCoordinates, lanes[index]);
    }

    public void CheckExpandButtons()
    {
        foreach (Lane lane in lanes)
        {
            if (lane.expandCanvas != null)
            {
                lane.expandCanvas.SetActive(lane.open);
            }
        }
    }

    public void SetButtonInteractive(bool interactable)
    {
        foreach (Lane lane in lanes)
        {
            if (lane.expandCanvas != null)
            {
                if (lane.open)
                {
                    lane.button.interactable = interactable;
                }
            }
        }
    }

    public List<Vector2> GetAllLaneEnds()
    {
        List<Vector2> laneEnds = new List<Vector2>();

        foreach (Lane lane in lanes)
        {
            if (1 < lane.spawnPoint.transform.position.x)
            {
                laneEnds.Add(new Vector2(1, 0));
            }
            else if (-1 > lane.spawnPoint.transform.position.x)
            {
                laneEnds.Add(new Vector2(-1, 0));
            }
            else if (1 < lane.spawnPoint.transform.position.z)
            {
                laneEnds.Add(new Vector2(0, 1));
            }
            else if (-1 > lane.spawnPoint.transform.position.z)
            {
                laneEnds.Add(new Vector2(0, -1));
            }
        }
        return laneEnds;
    }

    public List<Spot> GetAdjacentSpots(Spot startingSpot, bool shouldIgnorTerrain = false)
    {
        List<Spot> adjacentSpots = new List<Spot>();
        Vector3 spotPos = startingSpot.transform.position;
        List<Vector3> lookingFor = new List<Vector3>();
        lookingFor.Add(new Vector3(spotPos.x + 1, spotPos.y, spotPos.z));
        lookingFor.Add(new Vector3(spotPos.x - 1, spotPos.y, spotPos.z));
        lookingFor.Add(new Vector3(spotPos.x, spotPos.y, spotPos.z + 1));
        lookingFor.Add(new Vector3(spotPos.x, spotPos.y, spotPos.z - 1));
        foreach (Spot spot in allSpots)
        {
            if (spot.gameObject.activeSelf)
            {
                if (shouldIgnorTerrain || spot.terrainBonus.Count == 0)
                {
                    foreach (Vector3 looking in lookingFor)
                    {
                        if (spot.transform.position == looking)
                        {
                            adjacentSpots.Add(spot);
                        }
                    }
                }
            }
        }
        return adjacentSpots;
    }

    public Spot GetClosestSpot(Vector3 pos)
    {
        Spot closest = allSpots[0];
        float closestDist = Vector3.Distance(pos, allSpots[0].transform.position);
        foreach (Spot spot in allSpots)
        {
            if (Vector3.Distance(pos, spot.transform.position) < closestDist)
            {
                closest = spot;
                closestDist = Vector3.Distance(pos, spot.transform.position);
            }
        }
        return closest;
    }
}
