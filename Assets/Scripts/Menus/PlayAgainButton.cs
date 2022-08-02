using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgainButton : MonoBehaviour
{
    /// <summary>
    /// This Method is called when pressing the Play Again button in the end screen.
    /// It initializes the game again and resets the time, points and questions.
    /// </summary>
    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("loaded game scene");
        SceneManager.LoadScene("PlayerHUD", LoadSceneMode.Additive);
        Debug.Log("loaded player HUD scene");
    }
}
