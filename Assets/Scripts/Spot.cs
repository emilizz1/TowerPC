using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject spawnPoint;

    GameObject spotTower;

    bool towerBuilt;
    bool readyToBuild;

    private void Start()
    {
        if(transform.childCount == 0)
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
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!towerBuilt)
        {
            if (TowerPlacer.towerToPlace != null)
            {
                readyToBuild = true;
                TowerPlacer.towerPlaced = true;
                spotTower = Instantiate(TowerPlacer.towerToPlace, transform.position, Quaternion.identity, spawnPoint.transform);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!towerBuilt)
        {
            if (TowerPlacer.towerToPlace != null)
            {
                readyToBuild = false;
                TowerPlacer.towerPlaced = false;
                Destroy(spotTower);
            }
        }
    }

    void BuildTower()
    {
        if (spotTower != null)
        {
            spotTower.GetComponent<Tower>().Activate();
            towerBuilt = true;
        }
    }
}
