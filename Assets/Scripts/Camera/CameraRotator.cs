using UnityEngine;

/// <summary>
/// This script handles rotating the camera around the chickens in the Main and End Screen of the game
/// </summary>
public class CameraRotator : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
