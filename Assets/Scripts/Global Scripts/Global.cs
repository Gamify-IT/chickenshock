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
    private List<RoundResult> correctAnsweredQuestions;
    private List<RoundResult> wrongAnsweredQuestions;
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
    /// This method creates a new instance, if an instance exists already then the new GameObject will be deleted.
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
        this.InitVariables();
        this.FetchAllQuestions();
    }

    /// <summary>
    /// This method initializes the variables needed. If the question catalogue is empty it skips to the end screen.
    /// </summary>
    private void InitVariables()
    {
        Debug.Log("init game variables");
        this.points = 0;
        this.wrongAnsweredQuestions = new List<RoundResult>();
        this.correctAnsweredQuestions = new List<RoundResult>();
        this.time = ChickenshockProperties.ingamePlaytime;
        this.timeLimit = this.time;
        this.pointOverlay = GameObject.FindGameObjectWithTag("Point Overlay");
        this.pointOverlay.GetComponent<TMPro.TextMeshProUGUI>().text = this.points.ToString();
    }

    /// <summary>
    /// This method (Todo: add comment) job that checks the game status
    /// </summary>
    void Update()
    {
        if (this.questionLoaded)
        {
            this.CheckGameTimeOver();
            this.UpdateTimer();
            this.UnlockCursor();
        }
    }

    /// <summary>
    /// This method adds the answered question to the correctly answered questions
    /// </summary>
    public void addCorrectAnswerToResult(String answer)
    {
        Debug.Log("Add correct answer to game result: " + answer);
        RoundResult correctResult = new RoundResult(currentActiveQuestion, answer);
        this.correctAnsweredQuestions.Add(correctResult);
    }

    /// <summary>
    /// This method adds the answered question to the incorrectly answered questions
    /// </summary>
    public void addWrongAnswerToResult(String answer)
    {
        Debug.Log("Add wrong answer to game result: " + answer);
        RoundResult wrongResult = new RoundResult(currentActiveQuestion, answer);
        this.wrongAnsweredQuestions.Add(wrongResult);
    }

    /// <summary>
    /// This method checks if the timer reached zero and sends you to the End Screen if it did.
    /// </summary>
    private void CheckGameTimeOver()
    {
        if (time <= 0)
        {
            Debug.Log("Time is over load ens screen!");
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
        Debug.Log("------------------------");
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
        Debug.Log("------------------------");
        SceneManager.LoadScene("EndScreen");
        SaveRound();
    }

    /// <summary>
    /// This method updates the visuals and removes the remaining chickens
    /// </summary>
    public void FinishRound(String feedback)
    {
        GivePlayerFeedback(feedback);
        pointOverlay.GetComponent<TMPro.TextMeshProUGUI>().text = points.ToString();
        Invoke("KillRestChickens", 1f);
        this.roundComplete = true;
    }

    /// <summary>
    /// This method removes all existing chickens and calls PickRandomQuestion()
    /// </summary>
    private void KillRestChickens()
    {
        Debug.Log("Find and remove left over chickens");
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
        Debug.Log("Show player feedback: " + feedback);
        if(GameObject.FindGameObjectWithTag("CorrectAnswer") != null)
        {
            GameObject.FindGameObjectWithTag("CorrectAnswer").transform.Find("Shield").transform.Find("Cube").transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = ChickenshockProperties.wrongFeedbackText;
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
    /// This method creates new chickens depending on the amount of answers
    /// </summary>
    /// <param name="questionText">Text of the question</param>
    /// <param name="rightAnswer">Text of the unique right answer</param>
    /// <param name="wrongAnswers">Text of the wrong answers</param>
    void LoadNewChickens(string questionText, string rightAnswer, List<string> wrongAnswers)
    {
        Debug.Log("Create chickens");
        GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = questionText;
        GameObject chickenHorde = GameObject.Find("ChickenHorde");

        CreateCorrectChicken(rightAnswer, chickenHorde);
        CreateWrongChickens(wrongAnswers, chickenHorde);

        this.roundComplete = false;
    }

    /// <summary>
    /// This method creates the chickens carrying the wrong answers
    /// </summary>
    /// <param name="wrongAnswers"></param>
    /// <param name="chickenHorde"></param>
    private void CreateWrongChickens(List<string> wrongAnswers, GameObject chickenHorde)
    {
        Debug.Log("Create wrong chickens");
        foreach (string wrongAnswer in wrongAnswers)
        {
            GameObject wrongChicken = Instantiate(chickenPrefab, chickenHorde.transform);
            wrongChicken.tag = "WrongAnswer";
            wrongChicken.transform.parent = chickenHorde.transform;
            wrongChicken.transform.Find("Shield").transform.Find("Cube").transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = wrongAnswer;
        }
    }

    /// <summary>
    /// This method creates the chicken carrying the right answer
    /// </summary>
    /// <param name="rightAnswer"></param>
    /// <param name="chickenHorde"></param>
    private void CreateCorrectChicken(string rightAnswer, GameObject chickenHorde)
    {
        Debug.Log("Create correct chicken");
        GameObject correctAnswerChicken = Instantiate(chickenPrefab, chickenHorde.transform);
        Debug.Log("init correct Chicken");
        correctAnswerChicken.tag = "CorrectAnswer";
        correctAnswerChicken.transform.parent = chickenHorde.transform;
        correctAnswerChicken.transform.Find("Shield").transform.Find("Cube").transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = rightAnswer;
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
        Debug.Log("Try to fetch questions");
        Debug.Log("configuration as uuid:"+configurationAsUUID);
        String originURL;
        String restRequest;
        try
        {   
            configurationAsUUID = GetConfiguration();
            originURL = GetOriginUrl();
            restRequest = ChickenshockProperties.getQuestions.Replace("{id}", configurationAsUUID);
        } catch(EntryPointNotFoundException entryPointNotFoundException) {
            Debug.Log("EntryPointNotFoundException, probably becouse you started the game with the editor: " + entryPointNotFoundException);
            configurationAsUUID = ChickenshockProperties.editorConfiguration;
            originURL = "";
            restRequest = ChickenshockProperties.editorGetQuestions.Replace("{id}", configurationAsUUID);
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

    /// <summary>
    /// This method starts a POST request coroutine that saves the result of the current round to the backend
    /// </summary>
    private void SaveRound()
    {
        Debug.Log("Save round details");
        String originURL;
        String restRequest;
        try
        {
            originURL = GetOriginUrl();
            restRequest = ChickenshockProperties.saveRound;
        }
        catch (EntryPointNotFoundException entryPointNotFoundException)
        {
            Debug.Log("EntryPointNotFoundException, probably becouse you started the game with the editor: " + entryPointNotFoundException);
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

    public void AddWrongKill()
    {
        this.wrongKillsCount++;
    }
    public void AddCorrectKill()
    {
        this.correctKillsCount++;
    }

    public void AddShot()
    {
        this.shotCount++;
    }

    public void UpdatePoints(int amountToUpdate)
    {
        this.points += amountToUpdate;
    }

    public bool GetRoundStatus()
    {
        return roundComplete;
    }

}
