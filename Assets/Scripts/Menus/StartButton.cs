using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script handles starting the game from the main screen.
/// </summary>
public class StartButton : MonoBehaviour
{

    /// <summary>
    /// This method loads the Game scene and loads the HUD over it. 
    /// </summary>
    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("loaded game scene");
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("PlayerHUD", LoadSceneMode.Additive);
        Debug.Log("loaded player HUD scene");
    }

}
