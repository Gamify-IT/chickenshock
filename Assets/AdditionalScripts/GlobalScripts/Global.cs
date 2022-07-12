using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using System.Linq;
using System.Runtime.InteropServices;

public class Global : MonoBehaviour
{

    public GameObject[] answers;
    public GameObject[] correctAnswer;

    public int initialNumberOfWrongChickens;
    public int initialNumberOfCorrectChickens;

    public static List<Question> allUnusedQuestions;
    public static int points;
    public bool killedAChicken = false;

    public static float time = 30; //in seconds

    [DllImport("__Internal")]
    private static extern string GetConfiguration();

    [DllImport("__Internal")]
    private static extern string GetOriginUrl();

    void Start()
    {
        this.initialNumberOfWrongChickens = 4;
        this.initialNumberOfCorrectChickens = 1;

        if(allUnusedQuestions == null)
        {
            Debug.Log("try to fetch all questions!");
            this.FetchAllQuestions();
        }
        else
        {
            this.PickRandomQuestion();
        }
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
        int randomNumber = UnityEngine.Random.Range(0, allUnusedQuestions.Count);
        Debug.Log("picked question number: " + randomNumber);
        Debug.Log("question count was: " + allUnusedQuestions.Count);
        UpdateSignAndChickens(allUnusedQuestions[randomNumber].getQuestion(), allUnusedQuestions[randomNumber].getRightAnswer(), allUnusedQuestions[randomNumber].getWrongAnswerOne(), allUnusedQuestions[randomNumber].getWrongAnswerTwo(), allUnusedQuestions[randomNumber].getWrongAnswerThree(), allUnusedQuestions[randomNumber].getWrongAnswerFour());
       
        if(allUnusedQuestions.Count > 1)
        {
            allUnusedQuestions.RemoveAt(randomNumber);
        }

    }

    void UpdateSignAndChickens(string question, string rightAnswer, string wrongAnswerOne, string wrongAnswerTwo, string wrongAnswerThree, string wrongAnswerFour)
    {
        GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = question;
        GameObject.FindGameObjectsWithTag("CorrectAnswer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = rightAnswer;
        GameObject.FindGameObjectsWithTag("Answer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswerOne;
        GameObject.FindGameObjectsWithTag("Answer")[1].GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswerTwo;
        GameObject.FindGameObjectsWithTag("Answer")[2].GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswerThree;
        GameObject.FindGameObjectsWithTag("Answer")[3].GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswerFour;
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

    public void FetchAllQuestions()
    {
        String configuration = GetConfiguration();
        Debug.Log(configuration);
        String url = GetOriginUrl();
        StartCoroutine(GetRequest(url + "/api/moorhuhn/get-all-questions/" + configuration));
    }

    private IEnumerator GetRequest(String uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(uri + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(uri + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(uri + ":\nReceived: " + webRequest.downloadHandler.text);
                    string fixedText = fixJson(webRequest.downloadHandler.text);
                    Debug.Log("fixedText: " + fixedText);
                    QuestionWrapper questionWrapper = JsonUtility.FromJson<QuestionWrapper>(fixedText);
                    Question[] questions = questionWrapper.questions;
                    allUnusedQuestions = questions.ToList();
                    PickRandomQuestion();
                    break;
            }
        }
    }

    private string fixJson(string value)
    {
        value = "{\"questions\":" + value + "}";
        return value;
    }
}
