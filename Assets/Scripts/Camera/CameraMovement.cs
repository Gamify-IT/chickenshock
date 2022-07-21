using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float horizontalSpeed = 2.0f;
    public float verticalSpeed = 2.0f;
    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;

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
        horizontalRotation += horizontal;
        verticalRotation -= vertical;
        transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
        ResetCamera();
    }

    /// <summary>
    /// This method blocks the camera from going out of the designated playing area.
    /// If the camera went out of the playing area it is reset to the border.
    /// </summary>
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
