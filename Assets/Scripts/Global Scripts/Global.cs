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
    public bool pointsUpdated = false;

    public static float time; //in seconds

    private string correctFeedback = "CORRECT!";
    private string wrongFeedback = "WRONG!";

    [DllImport("__Internal")]
    private static extern string GetConfiguration();

    [DllImport("__Internal")]
    private static extern string GetOriginUrl();

    void Start()
    {
        InitVariables();
    }

    void Update()
    {
        CheckGameTimeOver();
        UpdateRound();
        UpdateTimer();
        UnlockCursor();
    }

    /// <summary>
    /// This method initializes the variables needed. If the question catalogue is empty it skips to the end screen.
    /// </summary>
    private void InitVariables()
    {
        time = float.Parse(Properties.get("ingame.playtime"));
        this.initialNumberOfWrongChickens = 4;
        this.initialNumberOfCorrectChickens = 1;
        GameObject.FindGameObjectWithTag("Point Overlay").GetComponent<TMPro.TextMeshProUGUI>().text = points.ToString();
        if (allUnusedQuestions == null)
        {
            Debug.Log("try to fetch all questions!");
            this.FetchAllQuestions();
        }
        else if (allUnusedQuestions.Count > 0)
        {
            this.PickRandomQuestion();
        }
        else
        {
            LoadEndScreen();
        }
    }


    /// <summary>
    /// This method checks if the timer reached zero and sends you to the End Screen if it did.
    /// </summary>
    private void CheckGameTimeOver()
    {
        if (time <= 0)
        {
            LoadEndScreen();
        }
    }

    /// <summary>
    /// This method loads the end screen and updates the end screen's points
    /// </summary>
    private void LoadEndScreen()
    {
        EndScreen.points = points;
        SceneManager.LoadScene("EndScreen");
    }

    /// <summary>
    /// This method checks if a chicken was killed, if yes the points get updated and a new round starts.
    /// </summary>
    private void UpdateRound()
    {
        if (!this.pointsUpdated && this.CheckKilledChickens())
        { 
            this.UpdatePoints();
            StartCoroutine(WaitResetAndPlayAgain());
        }
    }

    /// <summary>
    /// This method Updates the points text in the top right corner.
    /// </summary>
    private void UpdatePoints()
    {
        GameObject.FindGameObjectWithTag("Point Overlay").GetComponent<TMPro.TextMeshProUGUI>().text = points.ToString();
        this.pointsUpdated = true;
    }

    /// <summary>
    /// This method checks wether the right or the wrond chicken was killed and returns true if any chicken was killed.
    /// </summary>
    /// <returns>bool - chicken was killed -> true; no chicken was killed -> false</returns>
    private bool CheckKilledChickens()
    {
        this.answers = GameObject.FindGameObjectsWithTag("Answer");
        this.correctAnswer = GameObject.FindGameObjectsWithTag("CorrectAnswer");

        if (answers.Length < initialNumberOfWrongChickens && correctAnswer.Length == initialNumberOfCorrectChickens) //killed wrong chicken
        {
            points--;
            GameObject.FindGameObjectsWithTag("CorrectAnswer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = wrongFeedback;
            GivePlayerFeedback(wrongFeedback);
            return true;
        }
        else if (answers.Length == initialNumberOfWrongChickens && correctAnswer.Length < initialNumberOfCorrectChickens) //killed correct chicken
        {
            points++;
            GivePlayerFeedback(correctFeedback);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// This method updates the shields with the corresponding player feedback.
    /// </summary>
    /// <param name="feedback">The feedback the player gets for killing the right/wrong chicken</param>
    private void GivePlayerFeedback(string feedback)
    {
        GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = feedback;
        foreach (GameObject chicken in GameObject.FindGameObjectsWithTag("Answer"))
        {
            chicken.GetComponent<TMPro.TextMeshProUGUI>().text = feedback;
        }
    }

    /// <summary>
    /// This method picks a random question. If a question is answered it gets removed from the catalogue.
    /// </summary>
    void PickRandomQuestion()
    {
        int randomNumber = UnityEngine.Random.Range(0, allUnusedQuestions.Count);
        Debug.Log("picked question number: " + randomNumber);
        Debug.Log("question count was: " + allUnusedQuestions.Count);
        UpdateSignAndChickens(allUnusedQuestions[randomNumber].getQuestionText(), allUnusedQuestions[randomNumber].getRightAnswer(), allUnusedQuestions[randomNumber].getWrongAnswerOne(), allUnusedQuestions[randomNumber].getWrongAnswerTwo(), allUnusedQuestions[randomNumber].getWrongAnswerThree(), allUnusedQuestions[randomNumber].getWrongAnswerFour());
        allUnusedQuestions.RemoveAt(randomNumber);
        Debug.Log("question count after removing the question was: " + allUnusedQuestions.Count);
    }

    /// <summary>
    /// This method updates the question and answer signs with the corresponding values.
    /// </summary>
    /// <param name="questionText"></param>
    /// <param name="rightAnswer"></param>
    /// <param name="wrongAnswerOne"></param>
    /// <param name="wrongAnswerTwo"></param>
    /// <param name="wrongAnswerThree"></param>
    /// <param name="wrongAnswerFour"></param>
    void UpdateSignAndChickens(string questionText, string rightAnswer, string wrongAnswerOne, string wrongAnswerTwo, string wrongAnswerThree, string wrongAnswerFour)
    {
        GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = questionText;
        GameObject.FindGameObjectsWithTag("CorrectAnswer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = rightAnswer;
        GameObject.FindGameObjectsWithTag("Answer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswerOne;
        GameObject.FindGameObjectsWithTag("Answer")[1].GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswerTwo;
        GameObject.FindGameObjectsWithTag("Answer")[2].GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswerThree;
        GameObject.FindGameObjectsWithTag("Answer")[3].GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswerFour;
    }

    /// <summary>
    /// This method resets the scene after 3 seconds if the timer is not zero.
    /// </summary>
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

    /// <summary>
    /// This method updates the timer.
    /// </summary>
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

    /// <summary>
    /// This method starts a coroutine that sends a Get request for all the questions to the moorhuhn api.
    /// </summary>
    public void FetchAllQuestions()
    {
        String configurationAsUUID = GetConfiguration();
        Debug.Log(configurationAsUUID);
        String url = GetOriginUrl();
        String path = Properties.get("REST.getQuestions");
        Debug.Log(path);
        StartCoroutine(GetRequest(url + path + configurationAsUUID));
    }

    /// <summary>
    /// This method sends a Get request and handles the response accordingly.
    /// </summary>
    /// <param name="uri"></param>
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

    /// <summary>
    /// This methods fixes the Json formatting.
    /// </summary>
    /// <param name="value"></param>
    private string fixJson(string value)
    {
        value = "{\"questions\":" + value + "}";
        return value;
    }

    /// <summary>
    /// This method unlocks your mouse cursor as long as you hold the left "Alt" key.
    /// </summary>
    private void UnlockCursor()
    {
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
        }
        else 
        {
            Cursor.visible = false;
        }
    }

}
