using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoSingleton<CameraMovement>
{
    [SerializeField] float movementSpeed;
    [SerializeField] float mouseSpeed;
    [SerializeField] float zoomSpeed;
    [SerializeField] float zoomMin;
    [SerializeField] float zoomMax;

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
    {
        change = transform.position;

        if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
        {
            diference = prevPos - Input.mousePosition;

            change.x += diference.x * mouseSpeed;
            change.z += diference.y * mouseSpeed;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            change.y -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            change.y = Mathf.Clamp(change.y, zoomMin, zoomMax);
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
