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
    #region initialInformations
    private int initialNumberOfWrongChickens;
    public static float time; //in seconds
    #endregion

    #region Chickens
    public GameObject chickenPrefab;
    public List<GameObject> wrongAnswerChickens;
    private GameObject correctAnswerChicken;
    #endregion 

    #region JavaScript Methods
    [DllImport("__Internal")]
    private static extern string GetConfiguration();

    [DllImport("__Internal")]
    private static extern string GetOriginUrl();
    #endregion

    #region persistant data
    private string configurationAsUUID;
    public static int questionCount;
    public static float timeLimit;
    public static float finishedInSeconds;
    public static int correctKillsCount;
    public static int wrongKillsCount;
    public static int shotCount;
    public static int points;
    public static List<string> correctAnsweredQuestions;
    public static List<string> wrongAnsweredQuestions;
    #endregion

    #region global variables
    private List<Question> allUnusedQuestions;
    private string currentActiveQuestion = "";
    public bool pointsUpdated = false;
    private bool questionLoaded = false;
    #endregion

    #region gameobjects
    private GameObject pointOverlay;
    #endregion

    void Start()
    {
        Debug.Log("started global script");
        InitVariables();
    }

    /// <summary>
    /// This method initializes the variables needed. If the question catalogue is empty it skips to the end screen.
    /// </summary>
    private void InitVariables()
    {
        points = 0;
        wrongAnswerChickens = new List<GameObject>();
        wrongAnsweredQuestions = new List<string>();
        correctAnsweredQuestions = new List<string>();
        time = MoorhuhnProperties.ingamePlaytime;
        timeLimit = time;
        pointOverlay = GameObject.FindGameObjectWithTag("Point Overlay");
        pointOverlay.GetComponent<TMPro.TextMeshProUGUI>().text = points.ToString();
        this.FetchAllQuestions();
    }

    void Update()
    {
        if (questionLoaded)
        {
            CheckGameTimeOver();
            UpdateRound();
            UpdateTimer();
            UnlockCursor();
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
        finishedInSeconds = timeLimit - time;
        EndScreen.points = points;
        Debug.Log("Load endscreen with round infos: configurationAsUUID: " + configurationAsUUID );
        Debug.Log("questionCount: " + questionCount);
        Debug.Log("timeLimit: " + timeLimit);
        Debug.Log("finished in seconds: " + finishedInSeconds);
        Debug.Log("correctKillsCount: " + correctKillsCount);
        Debug.Log("wrongKillsCount: " + wrongKillsCount);
        Debug.Log("shotCount: " + shotCount);
        Debug.Log("points: " + points);
        Debug.Log("correctAnsweredQuestions: " + correctAnsweredQuestions);
        Debug.Log("wrongAnsweredQuestions: " + wrongAnsweredQuestions);
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
            Invoke("killRestChickens", 1f);
        }
    }

    private void killRestChickens()
    {
        Destroy(GameObject.FindGameObjectWithTag("CorrectAnswer"));
        foreach(GameObject wrongChicken in GameObject.FindGameObjectsWithTag("WrongAnswer"))
        {
            Destroy(wrongChicken);
        }
        this.PickRandomQuestion();
    }

    /// <summary>
    /// This method Updates the points text in the top right corner.
    /// </summary>
    private void UpdatePoints()
    {
        pointOverlay.GetComponent<TMPro.TextMeshProUGUI>().text = points.ToString();
        this.pointsUpdated = true;
    }

    /// <summary>
    /// This method checks wether the right or the wrond chicken was killed and returns true if any chicken was killed.
    /// </summary>
    /// <returns>bool - chicken was killed -> true; no chicken was killed -> false</returns>
    private bool CheckKilledChickens()
    {
        string correctFeedback = "CORRECT!";
        string wrongFeedback = "WRONG!";
        this.wrongAnswerChickens = this.wrongAnswerChickens.Where(item => item != null).ToList();
        if (this.wrongAnswerChickens.Count < initialNumberOfWrongChickens && this.correctAnswerChicken != null) //killed wrong chicken
        {
            points--;
            wrongKillsCount++;
            wrongAnsweredQuestions.Add(currentActiveQuestion);
            GameObject.FindGameObjectWithTag("CorrectAnswer").transform.Find("Shield").transform.Find("Cube").transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = wrongFeedback;
            GivePlayerFeedback(wrongFeedback);
            return true;
        }
        else if (this.correctAnswerChicken == null) //killed correct chicken
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
        foreach (GameObject chicken in GameObject.FindGameObjectsWithTag("WrongAnswer"))
        {
            chicken.transform.Find("Shield").transform.Find("Cube").transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = feedback;
        }
    }

    /// <summary>
    /// This method picks a random question. If a question is answered it gets removed from the catalogue.
    /// </summary>
    void PickRandomQuestion()
    {
        if(allUnusedQuestions.Count <= 0)
        {
            LoadEndScreen();
        }
        int randomNumber = UnityEngine.Random.Range(0, allUnusedQuestions.Count);
        this.initialNumberOfWrongChickens = allUnusedQuestions[randomNumber].getWrongAnswers().Count;
        LoadNewChickens(allUnusedQuestions[randomNumber].getQuestionText(), allUnusedQuestions[randomNumber].getRightAnswer(), allUnusedQuestions[randomNumber].getWrongAnswers());
        currentActiveQuestion = allUnusedQuestions[randomNumber].getId();
        allUnusedQuestions.RemoveAt(randomNumber);
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
    void LoadNewChickens(string questionText, string rightAnswer, List<string> wrongAnswers)
    {
        GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = questionText;
        GameObject chickenHorde = GameObject.Find("ChickenHorde");

        this.correctAnswerChicken = Instantiate(chickenPrefab, chickenHorde.transform);
        Debug.Log("init correct Chicken");
        this.correctAnswerChicken.tag = "CorrectAnswer";
        this.correctAnswerChicken.transform.parent = chickenHorde.transform;
        this.correctAnswerChicken.transform.Find("Shield").transform.Find("Cube").transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = rightAnswer;

        foreach (string wrongAnswer in wrongAnswers)
        {
            GameObject wrongChicken = Instantiate(chickenPrefab, chickenHorde.transform);
            wrongChicken.tag = "WrongAnswer";
            wrongChicken.transform.parent = chickenHorde.transform;
            wrongChicken.transform.Find("Shield").transform.Find("Cube").transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswer;
            this.wrongAnswerChickens.Add(wrongChicken);
        }
        this.pointsUpdated = false;
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
        //configurationAsUUID = GetConfiguration();
        Debug.Log("configuration as uuid:"+configurationAsUUID);
        String url = "http://localhost/minigames/moorhuhn/api/v1/configurations/f7ea293d-9976-4645-9be7-3896e440bf39/questions";//GetOriginUrl();
        String path = "";//MoorhuhnProperties.getQuestions.Replace("{id}",configurationAsUUID);
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
                    this.questionLoaded = true;
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
