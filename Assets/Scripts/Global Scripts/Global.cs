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
    private float time; //in seconds
    #endregion

    #region Chickens
    public GameObject chickenPrefab;
    private List<GameObject> wrongAnswerChickens;
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
    private int questionCount;
    private float timeLimit;
    private float finishedInSeconds;
    private int correctKillsCount;
    private int wrongKillsCount;
    private int shotCount;
    private int points;
    private List<string> correctAnsweredQuestions;
    private List<string> wrongAnsweredQuestions;
    #endregion

    #region global variables
    public static Global instance;
    private List<Question> allUnusedQuestions;
    private string currentActiveQuestion = "";
    private bool roundComplete = false;
    private bool questionLoaded = false;
    #endregion

    #region gameobjects
    private GameObject pointOverlay;
    #endregion

    /// <summary>
    /// This method 
    /// </summary>
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitVariables();
    }

    /// <summary>
    /// This method initializes the variables needed. If the question catalogue is empty it skips to the end screen.
    /// </summary>
    private void InitVariables()
    {
        Debug.Log("init game variables");
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
            UpdateTimer();
            UnlockCursor();
        }
    }

    public void addCorrectAnswerToResult()
    {
        this.correctAnsweredQuestions.Add(currentActiveQuestion);
    }

    public void addWrongAnswerToResult()
    {
        this.wrongAnsweredQuestions.Add(currentActiveQuestion);
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
        SaveRound();
    }

    public void FinishRound(String feedback)
    {
        GivePlayerFeedback(feedback);
        pointOverlay.GetComponent<TMPro.TextMeshProUGUI>().text = points.ToString();
        Invoke("KillRestChickens", 1f);
        this.roundComplete = true;
    }

    private bool CheckIfWrongChickenIsDead()
    {
        return this.wrongAnswerChickens.Count < initialNumberOfWrongChickens;
    }

    private void KillRestChickens()
    {
        Destroy(GameObject.FindGameObjectWithTag("CorrectAnswer"));
        foreach(GameObject wrongChicken in GameObject.FindGameObjectsWithTag("WrongAnswer"))
        {
            Destroy(wrongChicken);
        }
        this.PickRandomQuestion();
    }

    /// <summary>
    /// This method updates the shields with the corresponding player feedback.
    /// </summary>
    /// <param name="feedback">The feedback the player gets for killing the right/wrong chicken</param>
    private void GivePlayerFeedback(string feedback)
    {
        if(GameObject.FindGameObjectWithTag("CorrectAnswer") != null)
        {
            GameObject.FindGameObjectWithTag("CorrectAnswer").transform.Find("Shield").transform.Find("Cube").transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = MoorhuhnProperties.wrongFeedbackText;
        }

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
        Debug.Log("pick new question with: " + allUnusedQuestions.Count + " questions remaining");
        if (allUnusedQuestions.Count <= 0)
        {
            LoadEndScreen();
        }
        else
        {
            Debug.Log(allUnusedQuestions.Count + " questions remaining");
            int randomNumber = UnityEngine.Random.Range(0, allUnusedQuestions.Count);
            this.initialNumberOfWrongChickens = allUnusedQuestions[randomNumber].getWrongAnswers().Count;
            LoadNewChickens(allUnusedQuestions[randomNumber].getQuestionText(), allUnusedQuestions[randomNumber].getRightAnswer(), allUnusedQuestions[randomNumber].getWrongAnswers());
            currentActiveQuestion = allUnusedQuestions[randomNumber].getId();
            allUnusedQuestions.RemoveAt(randomNumber);
        }
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

        CreateCorrectChicken(rightAnswer, chickenHorde);
        CreateWrongChickens(wrongAnswers, chickenHorde);

        this.roundComplete = false;
    }

    private void CreateWrongChickens(List<string> wrongAnswers, GameObject chickenHorde)
    {
        foreach (string wrongAnswer in wrongAnswers)
        {
            GameObject wrongChicken = Instantiate(chickenPrefab, chickenHorde.transform);
            wrongChicken.tag = "WrongAnswer";
            wrongChicken.transform.parent = chickenHorde.transform;
            wrongChicken.transform.Find("Shield").transform.Find("Cube").transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswer;
            this.wrongAnswerChickens.Add(wrongChicken);
        }
    }

    private void CreateCorrectChicken(string rightAnswer, GameObject chickenHorde)
    {
        this.correctAnswerChicken = Instantiate(chickenPrefab, chickenHorde.transform);
        Debug.Log("init correct Chicken");
        this.correctAnswerChicken.tag = "CorrectAnswer";
        this.correctAnswerChicken.transform.parent = chickenHorde.transform;
        this.correctAnswerChicken.transform.Find("Shield").transform.Find("Cube").transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = rightAnswer;
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
        Debug.Log("Try to fetch questions");
        Debug.Log("configuration as uuid:"+configurationAsUUID);
        String originURL;
        String restRequest;
        try
        {
            originURL = GetOriginUrl();
            restRequest = MoorhuhnProperties.getQuestions.Replace("{id}", configurationAsUUID);
        } catch(EntryPointNotFoundException e) {
            Debug.Log("EntryPointNotFoundException, probably becouse you started the game with the editor: " + e);
            originURL = MoorhuhnProperties.localOrigin;
            restRequest = "";
        }
        string completeRequestString = originURL + restRequest;
        StartCoroutine(GetRequest(completeRequestString));
    }

    /// <summary>
    /// This method sends a Get request and handles the response accordingly.
    /// </summary>
    /// <param name="uri"></param>
    private IEnumerator GetRequest(String uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            Debug.Log("Question request: " + uri);
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
                    string fixedText = FixJson(webRequest.downloadHandler.text);
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

    private void SaveRound()
    {
        String originURL;
        String restRequest;
        try
        {
            originURL = GetOriginUrl();
            restRequest = MoorhuhnProperties.saveRound;
        }
        catch (EntryPointNotFoundException e)
        {
            Debug.Log("EntryPointNotFoundException, probably becouse you started the game with the editor");
            originURL = "http://localhost/minigames/chickenshock/api/v1/results";
            restRequest = "";
        }
        string completeRequestString = originURL + restRequest;
        StartCoroutine(PostRequest(completeRequestString));
    }

    private IEnumerator PostRequest(String uri)
    {
        GameResult round = new GameResult(questionCount,timeLimit,finishedInSeconds,correctKillsCount,wrongKillsCount,correctKillsCount + wrongKillsCount, shotCount,points,correctAnsweredQuestions,wrongAnsweredQuestions, configurationAsUUID);
        string jsonRound = JsonUtility.ToJson(round);
        Debug.Log(jsonRound);
        byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonRound);

        using (UnityWebRequest postRequest = new UnityWebRequest(uri, "POST"))
        {
            Debug.Log("Save game result: " + uri);
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
            postRequest.Dispose();
        }
    }

    /// <summary>
    /// This methods fixes the Json formatting.
    /// </summary>
    /// <param name="value"></param>
    private string FixJson(string value)
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

    public void addWrongKill()
    {
        this.wrongKillsCount++;
    }
    public void addCorrectKill()
    {
        this.correctKillsCount++;
    }

    public void addShot()
    {
        this.shotCount++;
    }

    public void updatePoints(int amountToUpdate)
    {
        this.points += amountToUpdate;
    }

    public bool getRoundStatus()
    {
        return roundComplete;
    }

}
