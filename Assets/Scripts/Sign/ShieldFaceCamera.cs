using UnityEngine;

public class ShieldFaceCamera : MonoBehaviour
{
    public new GameObject camera;
    public GameObject shield;

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
