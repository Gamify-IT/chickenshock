using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    public GameObject[] answers;
    public GameObject[] correctAnswer;

    // Update is called once per frame
    void Update()
    {
        GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "123";

        this.answers = GameObject.FindGameObjectsWithTag("Answer");
        this.correctAnswer = GameObject.FindGameObjectsWithTag("CorrectAnswer");


        if (answers.Length <= 3 && correctAnswer.Length == 1) //killed wrong chicken
        {
            Debug.Log("YOU KILLTED THE WRONG CHICKEN, FKKKKKKKK");
        }
        if (answers.Length == 4 && correctAnswer.Length < 1) //killed correct chicken
        {
            Debug.Log("YOU KILLTED THE RIGHT CHICKEN, YIPPPPPPI");
        }
        
    }
}
