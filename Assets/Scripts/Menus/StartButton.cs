using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

    /// <summary>
    /// This method loads the Game scene and loads the HUD over it. 
    /// </summary>
    public void LoadGame()
    {
        Global.isInitialized = false;
        SceneManager.LoadScene("Game");
        Debug.Log("loaded game scene");
        SceneManager.LoadScene("PlayerHUD",LoadSceneMode.Additive);
        Debug.Log("loaded player HUD scene");
    }

}
