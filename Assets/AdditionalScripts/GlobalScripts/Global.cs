using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    public GameObject[] answers;
    public GameObject[] correctAnswer;

    public int initialNumberOfWrongChickens;
    public int initialNumberOfCorrectChickens;

    void Start()
    {
        this.initialNumberOfWrongChickens = 4;
        this.initialNumberOfCorrectChickens = 1;

        pickRandomQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        this.answers = GameObject.FindGameObjectsWithTag("Answer");
        this.correctAnswer = GameObject.FindGameObjectsWithTag("CorrectAnswer");


        if (answers.Length < initialNumberOfWrongChickens && correctAnswer.Length == initialNumberOfCorrectChickens) //killed wrong chicken
        {
            Debug.Log("YOU KILLED THE WRONG CHICKEN, FKKKKKKKK");

            GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "WRONG!";
            GameObject.FindGameObjectsWithTag("CorrectAnswer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "WRONG!";
            GameObject.FindGameObjectsWithTag("Answer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "WRONG!";
            GameObject.FindGameObjectsWithTag("Answer")[1].GetComponent<TMPro.TextMeshProUGUI>().text = "WRONG!";
            GameObject.FindGameObjectsWithTag("Answer")[2].GetComponent<TMPro.TextMeshProUGUI>().text = "WRONG!";
        }
        if (answers.Length == initialNumberOfWrongChickens && correctAnswer.Length < initialNumberOfCorrectChickens) //killed correct chicken
        {
            Debug.Log("YOU KILLED THE RIGHT CHICKEN, YIPPPPPPI");

            GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "CORRECT!";
            GameObject.FindGameObjectsWithTag("Answer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "CORRECT!";
            GameObject.FindGameObjectsWithTag("Answer")[1].GetComponent<TMPro.TextMeshProUGUI>().text = "CORRECT!";
            GameObject.FindGameObjectsWithTag("Answer")[2].GetComponent<TMPro.TextMeshProUGUI>().text = "CORRECT!";
            GameObject.FindGameObjectsWithTag("Answer")[3].GetComponent<TMPro.TextMeshProUGUI>().text = "CORRECT!";
        }
        
    }

    void pickRandomQuestion()
    {
        int randomNumber  = Random.Range(1,5);

        if(randomNumber == 1) {
            GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "Q1";
            GameObject.FindGameObjectsWithTag("CorrectAnswer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "R1";
            GameObject.FindGameObjectsWithTag("Answer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "W1.1";
            GameObject.FindGameObjectsWithTag("Answer")[1].GetComponent<TMPro.TextMeshProUGUI>().text = "W1.2";
            GameObject.FindGameObjectsWithTag("Answer")[2].GetComponent<TMPro.TextMeshProUGUI>().text = "W1.3";
            GameObject.FindGameObjectsWithTag("Answer")[3].GetComponent<TMPro.TextMeshProUGUI>().text = "W1.4";
        } else if (randomNumber == 2){
            GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "Q2";
            GameObject.FindGameObjectsWithTag("CorrectAnswer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "R2";
            GameObject.FindGameObjectsWithTag("Answer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "W2.1";
            GameObject.FindGameObjectsWithTag("Answer")[1].GetComponent<TMPro.TextMeshProUGUI>().text = "W2.2";
            GameObject.FindGameObjectsWithTag("Answer")[2].GetComponent<TMPro.TextMeshProUGUI>().text = "W2.3";
            GameObject.FindGameObjectsWithTag("Answer")[3].GetComponent<TMPro.TextMeshProUGUI>().text = "W2.4";
        } else if (randomNumber == 3){
            GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "Q3";
            GameObject.FindGameObjectsWithTag("CorrectAnswer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "R3";
            GameObject.FindGameObjectsWithTag("Answer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "W3.1";
            GameObject.FindGameObjectsWithTag("Answer")[1].GetComponent<TMPro.TextMeshProUGUI>().text = "W3.2";
            GameObject.FindGameObjectsWithTag("Answer")[2].GetComponent<TMPro.TextMeshProUGUI>().text = "W3.3";
            GameObject.FindGameObjectsWithTag("Answer")[3].GetComponent<TMPro.TextMeshProUGUI>().text = "W3.4";
        } else if (randomNumber == 4){
            GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "Q4";
            GameObject.FindGameObjectsWithTag("CorrectAnswer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "R4";
            GameObject.FindGameObjectsWithTag("Answer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "W4.1";
            GameObject.FindGameObjectsWithTag("Answer")[1].GetComponent<TMPro.TextMeshProUGUI>().text = "W4.2";
            GameObject.FindGameObjectsWithTag("Answer")[2].GetComponent<TMPro.TextMeshProUGUI>().text = "W4.3";
            GameObject.FindGameObjectsWithTag("Answer")[3].GetComponent<TMPro.TextMeshProUGUI>().text = "W4.4";
        } else if (randomNumber == 5){
            GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "Q5";
            GameObject.FindGameObjectsWithTag("CorrectAnswer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "R5";
            GameObject.FindGameObjectsWithTag("Answer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "W5.1";
            GameObject.FindGameObjectsWithTag("Answer")[1].GetComponent<TMPro.TextMeshProUGUI>().text = "W5.2";
            GameObject.FindGameObjectsWithTag("Answer")[2].GetComponent<TMPro.TextMeshProUGUI>().text = "W5.3";
            GameObject.FindGameObjectsWithTag("Answer")[3].GetComponent<TMPro.TextMeshProUGUI>().text = "W5.4";
        }
    }
}
