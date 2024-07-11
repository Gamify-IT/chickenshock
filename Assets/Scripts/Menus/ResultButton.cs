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

    

    public void Start()
    {
        rewardsText.text = rewards.ToString() + "  " + "rewards";
        scoreText.text = score.ToString() + "  " + "scores";
        resultPanel.SetActive(true); 

      
    }
}
