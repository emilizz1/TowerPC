using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public Sprite image;
    public string structureName;

    [Header("Components")]
    [SerializeField] GameObject canvas;
    [SerializeField] AudioSource audioSource;

    bool active;

    public virtual void Activate(TerrainBonus terrain)
    {
        if (active)
        {
            Debug.Log("Returned");
            return;
        }
        active = true;
        SoundsController.instance.GetAudioClip("TowerPlaced");
        audioSource.Play();
    }
}
