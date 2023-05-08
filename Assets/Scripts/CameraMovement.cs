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
    [SerializeField] float maxX;
    [SerializeField] float maxZ;

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
            if(Input.GetAxis("Mouse ScrollWheel") > 0 && change.y -1 > zoomMin)
            {
                change += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * transform.forward;

            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && change.y + 1 < zoomMax)
            {
                change += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * transform.forward;

            }
            change.y = Mathf.Clamp(change.y, zoomMin, zoomMax);
        }

        prevPos = Input.mousePosition;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y > Screen.height - 3f)
        {
            change.z += movementSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x < 3f )
        {
            change.x -= movementSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y < 3f)
        {
            change.z -= movementSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x > Screen.width - 3f)
        {
            change.x += movementSpeed;
        }

        change.x = Mathf.Clamp(change.x, -maxX, maxX);
        change.z = Mathf.Clamp(change.z, -maxZ, maxZ);

        transform.position = change;
    }
}
