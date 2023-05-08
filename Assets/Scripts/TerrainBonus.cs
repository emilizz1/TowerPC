using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TerrainBonus : MonoBehaviour
{
    public TowerStats statsMultiplayers;
    public TerrainType type;
    [SerializeField] AdjacentTerrain adjacentTerrain;
    [SerializeField] Color desiredColor;
    
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
        forest,
        field,
        snow,
        mountain,
        aura,
        water,
        ruin
    }

    Spot mySpot;

    void Start()
    {
        mySpot = GetComponentInParent<Spot>();
        mySpot.terrainBonus.Add(this);
        mySpot.NewTerrainAdded(this);

        foreach (Spot spot in mySpot.myTile.GetAdjacentSpots(mySpot))
        {
            GameObject newTerrrain = adjacentTerrain.GetTerrain();
            if (newTerrrain != null)
            {
                Instantiate(newTerrrain, spot.transform);
            }
        }

        DoubleBonuses();
    }

    public void DoubleBonuses()
    {
        if (GlobalConditionHolder.terrainBonusesIncreased)
        {
            statsMultiplayers.range *= 1.5f;
            statsMultiplayers.fireRate *= 1.5f;
            statsMultiplayers.damage[0] *= 1.5f;
            statsMultiplayers.damage[1] *= 1.5f;
            statsMultiplayers.damage[2] *= 1.5f;
        }
    }

    public void AddStats(Tower tower)
    {
        tower.statsMultiplayers.CombineStats(statsMultiplayers);
    }
}
