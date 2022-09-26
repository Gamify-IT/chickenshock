using UnityEngine;

/// <summary>
/// This script rotates the signs the chickens are carrying so that they always face the camera. 
/// This is necessary so that the player can always read the signs no matter where the chickens are walking to.
/// </summary>
public class ShieldFaceCamera : MonoBehaviour
{
    private new GameObject camera;
    public GameObject shield;

    private void Start()
    {
        this.camera = GameObject.Find("Main Camera");
    }

    void Update()
    {
        AlignShield();
    }

    /// <summary>
    /// This method aligns the shield so that it always is perfectly visible for the player.
    /// </summary>
    private void AlignShield()
    {
        shield.transform.LookAt(camera.transform);
    }
}
