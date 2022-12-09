using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Tile myTile;
    public GameObject spawnPoint;
    public float terrainSpawnChance = 1f;

    internal TerrainBonus terrainBonus;

    GameObject spotObj;

    internal bool objBuilt;
    bool readyToBuild;

    private void Start()
    {
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
            if (terrainBonus != null)
            {
                TowerInfoWindow.instance.ShowInfoWithTerrain(towerToPlace, terrainBonus);
                if (terrainBonus.statsMultiplayers.range > 0)
                {
                    towerToPlace.SetupRange(terrainBonus.statsMultiplayers.range);
                }
            }
            else
            {
                TowerInfoWindow.instance.ShowInfo(towerToPlace);
            }
            TowerInfoWindow.instance.OpenWindow();
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
            if (terrainBonus != null)
            {
                terrainBonus.AddStats(tower);
            }
            tower.Activate();
            objBuilt = true;
            readyToBuild = false;
            TowerPlacer.allTowers.Add(tower);
        }
    }

    void BuildStructure()
    {
        if (spotObj != null)
        {
            Structure structure = spotObj.GetComponent<Structure>();
            structure.Activate();
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
