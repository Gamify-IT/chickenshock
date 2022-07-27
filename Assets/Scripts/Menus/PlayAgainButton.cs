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
        Global.points = 0;
        Global.time = float.Parse(Properties.get("ingame.playtime"));
        Global.isInitialized = false;
        Global.allUnusedQuestions = null;
        SceneManager.LoadScene("Game");
        Debug.Log("loaded game scene");
        SceneManager.LoadScene("PlayerHUD", LoadSceneMode.Additive);
        Debug.Log("loaded player HUD scene");
    }
}
