using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ResultButton : MonoBehaviour
{
    public GameObject resultPanel;
    public TMP_Text scoreText;
    public static int score;
    public static int rewards;

    

    public void Start()
    {
        scoreText.text = score.ToString() + "  " + "scores"  + "  " + "and"  +"  " + rewards.ToString() + "  " + "coins";
        resultPanel.SetActive(true); 

      
    }
}
