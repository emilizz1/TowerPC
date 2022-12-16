using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TerrainBonus : MonoBehaviour
{
    public Tower.TowerStats statsMultiplayers;
    public TerrainType type;
    [SerializeField] AdjacentTerrain adjacentTerrain;
    
    [Serializable]
    public struct AdjacentTerrain
    {
        public List<GameObject> terrain;
        public List<float> influence;

        public GameObject GetTerrain()
        {
            float checkedPercentage = 0f;
            float percentage = UnityEngine.Random.Range(0f, 1f);
            for (int i = 0; i < terrain.Count; i++)
            {
                checkedPercentage += influence[i];
                if(percentage < checkedPercentage)
                {
                    return terrain[i];
                }
            }
            return null;
        }
    }

    public enum TerrainType
    {
        forrest,
        field,
        snow,
        mountain
    }

    Spot mySpot;

    void Start()
    {
        mySpot = GetComponentInParent<Spot>();
        mySpot.terrainBonus = this;

        foreach(Spot spot in mySpot.myTile.GetAdjacentSpots(mySpot))
        {
            GameObject newTerrrain = adjacentTerrain.GetTerrain();
            if(newTerrrain != null)
            {
                Instantiate(newTerrrain, spot.transform);
            }
        }
    }

    public void AddStats(Tower tower)
    {
        tower.statsMultiplayers.CombineStats(statsMultiplayers);
    }
}
