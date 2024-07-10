using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ResultButton : MonoBehaviour
{
    public GameObject resultPanel;
    public TMP_Text rewardsText;
    public TMP_Text scoreText;
    public static int score;
    public static int rewards;

    

    public void OpenResultPanel()
    {
        rewardsText.text = "Rewards: " + rewards.ToString();
        scoreText.text = "Score: " + score.ToString();
        resultPanel.SetActive(true); 

      
    }
}
