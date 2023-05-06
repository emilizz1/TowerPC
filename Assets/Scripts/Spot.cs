using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FirstGearGames.SmoothCameraShaker;

public class Spot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Tile myTile;
    public GameObject spawnPoint;
    public float terrainSpawnChance = 1f;

    internal List<TerrainBonus> terrainBonus = new List<TerrainBonus>();

    internal GameObject spotObj;
    internal Tower secondTerrainGetter;

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
            spotObj = Instantiate(TowerPlacer.towerToPlace, transform.position, RotationManager.instance.GetRotation(), spawnPoint.transform);
            spotObj.GetComponent<Tower>().currentLevel = TowerPlacer.startingLevel;
            Tower towerToPlace = spotObj.GetComponent<Tower>();
            towerToPlace.PrepareTower(this);
            if (terrainBonus.Count > 0)
            {
                if (spotObj.GetComponent<EnergyTower>())
                {
                    GatherBonusesFromAdjacentTiles(towerToPlace);
                }
                else
                {
                    TowerInfoWindow.instance.ShowInfoWithTerrain(towerToPlace, terrainBonus);
                }
            }
            else
            {
                if (spotObj.GetComponent<EnergyTower>())
                {
                    GatherBonusesFromAdjacentTiles(towerToPlace);
                }
                else
                {
                    TowerInfoWindow.instance.ShowInfo(towerToPlace);
                }
            }
        }
        else if (StructurePlacer.structureToPlace != null)
        {
            readyToBuild = true;
            StructurePlacer.structurePlaced = true;
            spotObj = Instantiate(StructurePlacer.structureToPlace, transform.position, RotationManager.instance.GetRotation(), spawnPoint.transform);
        }

    }

    private void GatherBonusesFromAdjacentTiles(Tower towerToPlace)
    {
        List<TerrainBonus> allBonuses = new List<TerrainBonus>(terrainBonus);
        foreach (Spot spot in myTile.GetAdjacentSpots(this, true))
        {
            foreach (TerrainBonus bonus in spot.terrainBonus)
            {
                if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(TowerType.Energy) == 1)
                {
                    allBonuses.Add(bonus);
                }
                   allBonuses.Add(bonus);
            }
        }

        foreach (Tile tile in TileManager.instance.GetAdjacentTiles(myTile.transform.position))
        {
            foreach (Spot spot in tile.GetAdjacentSpots(this, true))
            {
                foreach (TerrainBonus bonus in spot.terrainBonus)
                {
                    if (SecondTowerAbilityManager.instance.SecondSpecialUnlocked(TowerType.Energy) == 1)
                    {
                        allBonuses.Add(bonus);
                    }
                    allBonuses.Add(bonus);
                }
            }
        }

        TowerInfoWindow.instance.ShowInfoWithTerrain(towerToPlace, allBonuses);
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
            RotationManager.instance.Rotated();
            objBuilt = true;
            readyToBuild = false;
            TowerPlacer.allTowers.Add(tower);
            if (TowerPlacer.allTowers.Count >= 50)
            {
                AchievementManager.TowersOnMap();
            }
            Money.instance.UpdateIncome();
            CameraShakerHandler.Shake(CameraShake.instance.shakeData);
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
            RotationManager.instance.Rotated();
            objBuilt = true;
            readyToBuild = false;
            StructurePlacer.allStructures.Add(structure);
            CameraShakerHandler.Shake(CameraShake.instance.shakeData);
        }
    }

    public void DestroyTowerPreview()
    {
        if (!objBuilt && readyToBuild)
        {
            Destroy(spotObj);
        }
    }

    public void NewTerrainAdded(TerrainBonus terrain)
    {
        if(spotObj != null)
        {
            Tower spotTower = spotObj.GetComponent<Tower>();
            if (spotTower != null)
            {
                terrain.AddStats(spotTower);
            }
        }

        if(secondTerrainGetter != null)
        {
            if(SecondTowerAbilityManager.instance.SecondSpecialUnlocked(TowerType.Energy) == 1)
            {
                terrain.AddStats(secondTerrainGetter);
            }
            terrain.AddStats(secondTerrainGetter);
        }
    }

    public bool Empty()
    {
        return !objBuilt && terrainBonus.Count == 0;
    }

    public bool CornerSpot()
    {
        return myTile.GetAdjacentSpots(this).Count < 4;
    }
}
