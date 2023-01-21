using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Tile myTile;
    public GameObject spawnPoint;
    public float terrainSpawnChance = 1f;

    internal List<TerrainBonus> terrainBonus = new List<TerrainBonus>();

    internal GameObject spotObj;

    internal bool objBuilt;
    bool readyToBuild;

    private void Start()
    {
        terrainBonus = new List<TerrainBonus>();
        if (transform.childCount == 0)
        {
            GameObject child = Instantiate(new GameObject(), transform);
            spawnPoint = child;
            child.transform.localScale = new Vector3(1f, 2f, 1f);
        }
    }

    private void Update()
    {
        if (readyToBuild)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (TowerPlacer.towerToPlace != null)
                {
                    BuildTower();
                }
                else if (StructurePlacer.structureToPlace != null)
                {
                    BuildStructure();
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (objBuilt)
        {
            return;
        }
        if (TowerPlacer.towerToPlace != null)
        {
            readyToBuild = true;
            TowerPlacer.towerPlaced = true;
            spotObj = Instantiate(TowerPlacer.towerToPlace, transform.position, Quaternion.identity, spawnPoint.transform);
            spotObj.GetComponent<Tower>().currentLevel = TowerPlacer.startingLevel;
            Tower towerToPlace = spotObj.GetComponent<Tower>();
            towerToPlace.PrepareTower();
            if (terrainBonus.Count > 0)
            {
                foreach (TerrainBonus terrain in terrainBonus)
                {
                    TowerInfoWindow.instance.ShowInfoWithTerrain(towerToPlace, terrain);
                    if (terrain.statsMultiplayers.range > 0)
                    {
                        towerToPlace.SetupRange(terrain.statsMultiplayers.range);
                    }
                }
            }
            else
            {
                TowerInfoWindow.instance.ShowInfo(towerToPlace);
            }
        }
        else if (StructurePlacer.structureToPlace != null)
        {
            readyToBuild = true;
            StructurePlacer.structurePlaced = true;
            spotObj = Instantiate(StructurePlacer.structureToPlace, transform.position, Quaternion.identity, spawnPoint.transform);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (objBuilt)
        {
            return;
        }
        if (TowerPlacer.towerToPlace != null)
        {
            readyToBuild = false;
            TowerPlacer.towerPlaced = false;
            Destroy(spotObj);
        }
        else if (StructurePlacer.structureToPlace != null)
        {
            readyToBuild = false;
            StructurePlacer.structurePlaced = false;
            Destroy(spotObj);
        }

    }

    void BuildTower()
    {
        if (spotObj != null)
        {
            Tower tower = spotObj.GetComponent<Tower>();
            if (terrainBonus.Count > 0)
            {
                foreach (TerrainBonus terrain in terrainBonus)
                {
                    terrain.AddStats(tower);
                }
            }
            TipsManager.instance.CheckForTipTerrain(terrainBonus.Count > 0);
            tower.Activate();
            objBuilt = true;
            readyToBuild = false;
            TowerPlacer.allTowers.Add(tower);
            Money.instance.UpdateIncome();
        }
    }

    void BuildStructure()
    {
        if (spotObj != null)
        {
            Structure structure = spotObj.GetComponent<Structure>();
            if (terrainBonus.Count > 0)
            {
                foreach (TerrainBonus terrain in terrainBonus)
                {
                    structure.Activate(terrain);
                }
            }
            else
            {
                structure.Activate(null);
            }
            objBuilt = true;
            readyToBuild = false;
            StructurePlacer.allStructures.Add(structure);
        }
    }

    public void DestroyTowerPreview()
    {
        if (!objBuilt && readyToBuild)
        {
            Destroy(spotObj);
        }
    }
}
