using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBonus : MonoBehaviour
{
    [SerializeField] Tower.TowerStats statsMultiplayers;


    void Start()
    {
        GetComponentInParent<Spot>().terrainBonus = this;
    }

    public void AddStats(Tower tower)
    {
        tower.statsMultiplayers.fireRate += statsMultiplayers.fireRate;
        tower.statsMultiplayers.range += statsMultiplayers.range;
        for (int i = 0; i < statsMultiplayers.damage.Count; i++)
        {
            tower.statsMultiplayers.damage[i] += statsMultiplayers.damage[i];
        }
    }
}
