using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TowerCanvas : MonoBehaviour
{
    [SerializeField] Tower tower; 

    public void PressedOnTower()
    {
        TowerInfoWindow.instance.ShowInfo(tower);
        TowerInfoWindow.instance.OpenWindow();
    }
}
