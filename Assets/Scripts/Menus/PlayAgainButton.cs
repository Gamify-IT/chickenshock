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
        resetStats();
        SceneManager.LoadScene("Game");
        Debug.Log("loaded game scene");
        SceneManager.LoadScene("PlayerHUD", LoadSceneMode.Additive);
        Debug.Log("loaded player HUD scene");
    }

    private void resetStats()
    {
        Global.points = 0;
        Global.time = ChickenshockProperties.ingamePlaytime;
        Global.isInitialized = false;
        Global.allUnusedQuestions = null;
        Global.correctKillsCount = 0;
        Global.wrongKillsCount = 0;
        Global.shotCount = 0;
    }
}
