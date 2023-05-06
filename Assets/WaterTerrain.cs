using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTerrain : MonoBehaviour
{
    Spot mySpot;

    void Start()
    {
        mySpot = GetComponentInParent<Spot>();
        mySpot.spotObj = gameObject;
        mySpot.objBuilt = true;
        mySpot.GetComponent<MeshRenderer>().enabled = false;
    }

}
