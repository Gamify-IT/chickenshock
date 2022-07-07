using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgainButtonScript : MonoBehaviour
{
    public void LoadGame()
    {
        Global.points = 0;
        Global.time = 30;
        SceneManager.LoadScene("Game");
        Debug.Log("loaded game scene");
        SceneManager.LoadScene("PlayerHUD", LoadSceneMode.Additive);
        Debug.Log("loaded player HUD scene");
        Global.allUnusedQuestions = null;
    }
}
