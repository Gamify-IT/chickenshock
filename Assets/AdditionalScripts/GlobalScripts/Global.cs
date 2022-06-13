using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{

    public GameObject[] answers;
    public GameObject[] correctAnswer;

    public int initialNumberOfWrongChickens;
    public int initialNumberOfCorrectChickens;

    public static int points;
    public bool killedAChicken = false;

    public static float time = 30; //in seconds

    void Start()
    {
        this.initialNumberOfWrongChickens = 4;
        this.initialNumberOfCorrectChickens = 1;

        PickRandomQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        if (time <= 0)
        {
            PointScript.points = points;
            Cursor.visible = true;
            SceneManager.LoadScene("EndScreen");
        }

        if (!killedAChicken)
        {
            this.CheckKilledChickens();
            this.UpdatePoints();
        }

        UpdateTimer();
    }

    void UpdatePoints()
    {
        GameObject.FindGameObjectWithTag("Point Overlay").GetComponent<TMPro.TextMeshProUGUI>().text = points.ToString();
    }

    void CheckKilledChickens()
    {
        this.answers = GameObject.FindGameObjectsWithTag("Answer");
        this.correctAnswer = GameObject.FindGameObjectsWithTag("CorrectAnswer");

        if (answers.Length < initialNumberOfWrongChickens && correctAnswer.Length == initialNumberOfCorrectChickens) //killed wrong chicken
        {
            Debug.Log("YOU KILLED THE WRONG CHICKEN, FKKKKKKKK");
            points--;
            killedAChicken = true;
            GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "WRONG!";
            GameObject.FindGameObjectsWithTag("CorrectAnswer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "WRONG!";
            GameObject.FindGameObjectsWithTag("Answer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "WRONG!";
            GameObject.FindGameObjectsWithTag("Answer")[1].GetComponent<TMPro.TextMeshProUGUI>().text = "WRONG!";
            GameObject.FindGameObjectsWithTag("Answer")[2].GetComponent<TMPro.TextMeshProUGUI>().text = "WRONG!";

            StartCoroutine(WaitResetAndPlayAgain());
        }
        if (answers.Length == initialNumberOfWrongChickens && correctAnswer.Length < initialNumberOfCorrectChickens) //killed correct chicken
        {
            Debug.Log("YOU KILLED THE RIGHT CHICKEN, YIPPPPPPI");
            points++;
            killedAChicken = true;
            GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "CORRECT!";
            GameObject.FindGameObjectsWithTag("Answer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = "CORRECT!";
            GameObject.FindGameObjectsWithTag("Answer")[1].GetComponent<TMPro.TextMeshProUGUI>().text = "CORRECT!";
            GameObject.FindGameObjectsWithTag("Answer")[2].GetComponent<TMPro.TextMeshProUGUI>().text = "CORRECT!";
            GameObject.FindGameObjectsWithTag("Answer")[3].GetComponent<TMPro.TextMeshProUGUI>().text = "CORRECT!";

            StartCoroutine(WaitResetAndPlayAgain());
        }
    }

    void PickRandomQuestion()
    {
        int randomNumber = Random.Range(1, 10);

        if (randomNumber == 1)
        {
            UpdateSignAndChickens("Q1", "R1", "W1.1", "W1.2", "W1.3", "W1.4");
        }
        else if (randomNumber == 2)
        {
            UpdateSignAndChickens("Q2", "R2", "W2.1", "W2.2", "W2.3", "W2.4");
        }
        else if (randomNumber == 3)
        {
            UpdateSignAndChickens("Q3", "R3", "W3.1", "W3.2", "W3.3", "W3.4");
        }
        else if (randomNumber == 4)
        {
            UpdateSignAndChickens("Q4", "R4", "W4.1", "W4.2", "W4.3", "W4.4");
        }
        else if (randomNumber == 5)
        {
            UpdateSignAndChickens("Q5", "R5", "W5.1", "W5.2", "W5.3", "W5.4");
        }
        else if (randomNumber == 6)
        {
            UpdateSignAndChickens("Q6", "R6", "W6.1", "W6.2", "W6.3", "W6.4");
        }
        else if (randomNumber == 7)
        {
            UpdateSignAndChickens("Q7", "R7", "W7.1", "W7.2", "W7.3", "W7.4");
        }
        else if (randomNumber == 8)
        {
            UpdateSignAndChickens("Q8", "R8", "W8.1", "W8.2", "W8.3", "W8.4");
        }
        else if (randomNumber == 9)
        {
            UpdateSignAndChickens("Q9", "R9", "W9.1", "W9.2", "W9.3", "W9.4");
        }
        else if (randomNumber == 10)
        {
            UpdateSignAndChickens("Q10", "R10", "W10.1", "W10.2", "W10.3", "W10.4");
        }
    }

    void UpdateSignAndChickens(string question, string correctAnswer, string wrongAnswer1, string wrongAnswer2, string wrongAnswer3, string wrongAnswer4)
    {
        GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = question;
        GameObject.FindGameObjectsWithTag("CorrectAnswer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = correctAnswer;
        GameObject.FindGameObjectsWithTag("Answer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswer1;
        GameObject.FindGameObjectsWithTag("Answer")[1].GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswer2;
        GameObject.FindGameObjectsWithTag("Answer")[2].GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswer3;
        GameObject.FindGameObjectsWithTag("Answer")[3].GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswer4;
    }

    IEnumerator WaitResetAndPlayAgain()
    {
        if (time > 0)
        {
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene("Game");
            Debug.Log("loaded game scene");
            SceneManager.LoadScene("PlayerHUD", LoadSceneMode.Additive);
            Debug.Log("loaded player HUD scene");
        }
    }

    void UpdateTimer()
    {
        time = time - Time.deltaTime;

        string timeString;

        if (time < 10)
        {
            timeString = "00:0" + ((int)time).ToString();
        }
        else
        {
            timeString = "00:" + ((int)time).ToString();
        }

        GameObject.FindGameObjectWithTag("Timer").GetComponent<TMPro.TextMeshProUGUI>().text = timeString;
    }
}
