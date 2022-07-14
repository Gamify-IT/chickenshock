using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("loaded game scene");
        SceneManager.LoadScene("PlayerHUD",LoadSceneMode.Additive);
        Debug.Log("loaded player HUD scene");
    }

}
