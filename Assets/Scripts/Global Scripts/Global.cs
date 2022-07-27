using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

public class Global : MonoBehaviour
{
    public static bool isInitialized;
    public GameObject[] wrongAnswers;
    public GameObject[] correctAnswer;

    public int initialNumberOfWrongChickens;

    public static List<Question> allUnusedQuestions;
    //persistant data
    public static int questionCount;//done
    public static float timeLimit;//done
    public static float finishedInSeconds;//done
    public static int correctKillsCount;//done
    public static int wrongKillsCount;//done
    public static int shotCount;//done
    public static int points;//done
    public static List<string> correctAnsweredQuestions;//done
    public static List<string> wrongAnsweredQuestions;//done
    public static string configurationAsUUID;//done

    private string currentActiveQuestion = "";
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
        Debug.Log("started global script");
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
        if (!isInitialized)
        {
            wrongAnsweredQuestions = new List<string>();
            correctAnsweredQuestions = new List<string>();
            Debug.Log("load propertie playtime");
            time = MoorhuhnProperties.ingamePlaytime;
            Debug.Log("loaded propertie playtime: " + time);
            timeLimit = time;
            isInitialized = true;
        }
        this.initialNumberOfWrongChickens = 4;
        GameObject.FindGameObjectWithTag("Point Overlay").GetComponent<TMPro.TextMeshProUGUI>().text = points.ToString();
        if (allUnusedQuestions == null)
        {
            Debug.Log("try to fetch all questions!");
            this.FetchAllQuestions();
        }
        else if (allUnusedQuestions.Count > 0)
        {
            Debug.Log("try to pick new question!");
            this.PickRandomQuestion();
        }
        else
        {
            if(time > 0)
            {
                Debug.Log("time over and no more questions");
                finishedInSeconds = timeLimit - time;
            }
            Debug.Log("load end screen before time was over");
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
        saveRound();
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
        this.wrongAnswers = GameObject.FindGameObjectsWithTag("Answer");
        this.correctAnswer = GameObject.FindGameObjectsWithTag("CorrectAnswer");

        if (wrongAnswers.Length < initialNumberOfWrongChickens) //killed wrong chicken
        {
            points--;
            wrongKillsCount++;
            wrongAnsweredQuestions.Add(currentActiveQuestion);
            GameObject.FindGameObjectsWithTag("CorrectAnswer")[0].GetComponent<TMPro.TextMeshProUGUI>().text = wrongFeedback;
            GivePlayerFeedback(wrongFeedback);
            return true;
        }
        else if (correctAnswer.Length == 0) //killed correct chicken
        {
            points++;
            correctKillsCount++;
            correctAnsweredQuestions.Add(currentActiveQuestion);
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
        UpdateSignAndChickens(allUnusedQuestions[randomNumber].getQuestionText(), allUnusedQuestions[randomNumber].getRightAnswer(), allUnusedQuestions[randomNumber].getWrongAnswers()[0], allUnusedQuestions[randomNumber].getWrongAnswers()[1], allUnusedQuestions[randomNumber].getWrongAnswers()[2], allUnusedQuestions[randomNumber].getWrongAnswers()[3]);
        currentActiveQuestion = allUnusedQuestions[randomNumber].getId();
        Debug.Log("question UUID is: " + currentActiveQuestion);
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
        configurationAsUUID = GetConfiguration();
        Debug.Log("configuration as uuid:"+configurationAsUUID);
        String url = GetOriginUrl();
        String path = MoorhuhnProperties.getQuestions.Replace("{id}",configurationAsUUID);
        Debug.Log("get questions with uuid path:" + path);
        StartCoroutine(GetRequest(url + path));
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
                    questionCount = allUnusedQuestions.Count;
                    PickRandomQuestion();
                    break;
            }
        }
    }

    private void saveRound()
    {
        String url = GetOriginUrl();
        String path = MoorhuhnProperties.saveRound;
        Debug.Log("save round infos with path:" + path);
        StartCoroutine(PostRequest(url + path));
    }

    private IEnumerator PostRequest(String uri)
    {
        GameResult round = new GameResult(questionCount,timeLimit,finishedInSeconds,correctKillsCount,wrongKillsCount,correctKillsCount + wrongKillsCount, shotCount,points,correctAnsweredQuestions,wrongAnsweredQuestions, configurationAsUUID);
        string jsonRound = JsonUtility.ToJson(round);
        Debug.Log(jsonRound);
        byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonRound);

        using (UnityWebRequest postRequest = new UnityWebRequest(uri, "POST"))
        {
            postRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            postRequest.downloadHandler = new DownloadHandlerBuffer();
            postRequest.SetRequestHeader("Content-Type", "application/json");

            yield return postRequest.SendWebRequest();

            switch (postRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(uri + ": Error: " + postRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(uri + ": HTTP Error: " + postRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(uri + ":\nReceived: " + postRequest.downloadHandler.text);
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
