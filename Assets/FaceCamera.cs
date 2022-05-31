using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FaceCamera : MonoBehaviour
{
    Camera myCamera;

    void Start()
    {
        myCamera = Camera.main;
    }

    void Update()
    {
        transform.forward = myCamera.transform.forward;

    }
}
