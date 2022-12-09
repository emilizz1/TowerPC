using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public Sprite image;
    public string structureName;

    [Header("Components")]
    [SerializeField] GameObject canvas;

    bool active;

    public virtual void Activate()
    {

    }
}
