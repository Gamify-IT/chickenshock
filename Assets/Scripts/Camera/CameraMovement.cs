using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float horizontalSpeed = 2.0f;
    float verticalSpeed = 2.0f;
    public float verticalRotation = 0f;
    public float horizontalRotation = 0f;

    // Update is called once per frame

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        float horizontal = horizontalSpeed * Input.GetAxis("Mouse X");
        float vertical = verticalSpeed * Input.GetAxis("Mouse Y");
        horizontalRotation += horizontal;
        verticalRotation -= vertical;
        transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
        ResetCamera();
    }

    void ResetCamera()
    {
        if (verticalRotation > 36f)
        {
            verticalRotation = 36f;
        }
        if (verticalRotation < 11f)
        {
            verticalRotation = 11f;
        }
        if (horizontalRotation > 45f)
        {
            horizontalRotation = 45f;
        }
        if (horizontalRotation < -45f)
        {
            horizontalRotation = -45f;
        }
    }
}
