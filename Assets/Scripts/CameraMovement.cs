using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoSingleton<CameraMovement>
{
    [SerializeField] float movementSpeed;
    [SerializeField] float mouseSpeed;

    Camera myCamera;

    Vector3 change;
    
    private Vector3 prevPos;
    private Vector3 diference;
    bool dragging;


    private void Start()
    {
        myCamera = Camera.main;
    }

    void LateUpdate()
    {//TODO add zoom out
        change = transform.position;

        if (Input.GetMouseButton(1))
        {
            diference = prevPos - Input.mousePosition;

            change.x += diference.x * mouseSpeed;
            change.z += diference.y * mouseSpeed;
        }
        prevPos = Input.mousePosition;



        if (Input.GetKey(KeyCode.W))
        {
            change.z += movementSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            change.x -= movementSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            change.z -= movementSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            change.x += movementSpeed;
        }
        transform.position = change;



    }
}
