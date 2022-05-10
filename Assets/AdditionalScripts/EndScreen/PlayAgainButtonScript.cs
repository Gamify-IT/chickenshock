using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgainButtonScript : MonoBehaviour
{
    public void loadGame(string sceneName)
    {
        Global.points = 0;
        Global.time = 30;
        SceneManager.LoadScene(sceneName);
    }

}
