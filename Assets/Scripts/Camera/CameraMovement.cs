using System;
using UnityEngine;

/// <summary>
/// This script handles the camera movement. It checks if the camera is moved out of the define scope.
/// The 4 CameraBorder Variables define the designated scope, in which the player can move the camera.
/// The 2 Speed variables define how fast the camera movement speed.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    public float horizontalSpeed = 2.0f;
    public float verticalSpeed = 2.0f;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private float rightCameraBorder = 36f;
    private float leftCameraBorder = 11f;
    private float bottomCameraBorder = -45f;
    private float topCameraBorder = 45f;

    void Update()
    {
        MoveCamera();
    }

    /// <summary>
    /// This method checks if the Mouse was moved on the X and Y Axis and moves the camera accordingly.
    /// </summary>
    void MoveCamera()
    {
        float horizontal = horizontalSpeed * Input.GetAxis("Mouse X");
        float vertical = verticalSpeed * Input.GetAxis("Mouse Y");
        horizontalRotation += horizontal * Time.deltaTime * 100;
        verticalRotation -= vertical * Time.deltaTime * 100;
        ResetCamera();
        transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }

    /// <summary>
    /// This method blocks the camera from going out of the designated playing area.
    /// If the camera went out of the playing area it is reset to the border.
    /// </summary>
    void ResetCamera()
    {
        verticalRotation = Mathf.Clamp(verticalRotation, leftCameraBorder, rightCameraBorder);
        horizontalRotation = Mathf.Clamp(horizontalRotation, bottomCameraBorder, topCameraBorder);
    }

}
