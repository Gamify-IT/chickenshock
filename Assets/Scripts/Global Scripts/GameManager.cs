using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine.UI;

/// <summary>
/// <include file="file.xml"></include>
/// This script handles updating all the game related data and general conditions for a game of chickenshock.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region initialInformations
    private int initialNumberOfWrongChickens;
    private float time; //in seconds
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
    public int score;
    public int rewards;
    private int volumeLevel;
    private List<RoundResult> correctAnsweredQuestions;
    private List<RoundResult> wrongAnsweredQuestions;
    #endregion

    #region global variables
    public static GameManager instance;
    private List<Question> allUnusedQuestions;
    private string currentActiveQuestion = "";
    private bool roundComplete = false;
    private bool questionLoaded = false;
    private bool gameFinished = false;
    #endregion

    #region gameobjects
    public GameObject chickenPrefab;
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

    public void Start()
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
        this.score = 0;
        this.rewards = 0;
        this.wrongAnsweredQuestions = new List<RoundResult>();
        this.correctAnsweredQuestions = new List<RoundResult>();
        this.pointOverlay = GameObject.FindGameObjectWithTag("Point Overlay");
        this.pointOverlay.GetComponent<TMPro.TextMeshProUGUI>().text = this.points.ToString();
        EndScreen.errorText = "";
    }

    /// <summary>
    /// This method updates the the game multiple times a second.
    /// </summary>
    void Update()
    {
        if (this.questionLoaded && !this.gameFinished)
        {
            this.CheckGameTimeOver();
            this.UpdateTimer();
            this.UnlockCursor();
        }
    }

    /// <summary>
    /// This method adds the answered question to the correctly answered questions
    /// <param name="answer">The answer on the chicken that got shot</param>
    /// </summary>
    public void AddCorrectAnswerToResult(string answer)
    {
        Debug.Log("Add correct answer to game result: " + answer);
        RoundResult correctResult = new RoundResult(currentActiveQuestion, answer);
        this.correctAnsweredQuestions.Add(correctResult);
    }

    /// <summary>
    /// This method adds the answered question to the incorrectly answered questions
    /// <param name="answer">The answer on the chicken that got shot</param>
    /// </summary>
    public void AddWrongAnswerToResult(string answer)
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
            Debug.Log("Time is over -> load end screen!");
            LoadEndScreen();
        }
    }

    /// <summary>
    /// This method loads the end screen and updates the end screen's points
    /// </summary>
    private void LoadEndScreen()
    {
        this.gameFinished = true;
        Cursor.lockState = CursorLockMode.None;
        this.finishedInSeconds = timeLimit - time;
        EndScreen.points = points;

        if (configurationAsUUID != "tutorial")
        {
            SaveRound();
        }
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
    /// This method removes all existing chickens
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
        foreach (GameObject chicken in GameObject.FindGameObjectsWithTag("WrongAnswer"))
        {
            chicken.transform.Find("Shield").transform.Find("Cube").transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text = feedback;
        }
        GameObject.FindGameObjectsWithTag("Question")[0].GetComponent<TMPro.TextMeshProUGUI>().text = feedback;
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
            int randomNumber = UnityEngine.Random.Range(0, this.allUnusedQuestions.Count);
            this.initialNumberOfWrongChickens = this.allUnusedQuestions[randomNumber].wrongAnswers.Count;
            LoadNewChickens(this.allUnusedQuestions[randomNumber].text, this.allUnusedQuestions[randomNumber].rightAnswer, this.allUnusedQuestions[randomNumber].wrongAnswers);
            this.currentActiveQuestion = this.allUnusedQuestions[randomNumber].id;
            this.allUnusedQuestions.RemoveAt(randomNumber);
        }
    }

    /// <summary>
    /// This method creates new chickens depending on the amount of answers
    /// </summary>
    /// <param name="questionText">Text of the question</param>
    /// <param name="rightAnswer">Text of the unique right answer</param>
    /// <param name="wrongAnswers">Text of the wrong answers</param>
    private void LoadNewChickens(string questionText, string rightAnswer, List<string> wrongAnswers)
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
    /// This method starts a coroutine that sends a Get request for all the questions to the chickenshock api.
    /// </summary>
    public void FetchAllQuestions()
    {
        Debug.Log("Try to fetch questions");
        String originURL;
        String restRequest;
        try
        {   
            configurationAsUUID = GetConfiguration();

            if (configurationAsUUID == "tutorial")
            {
                InitTutorialMinigame();
                return;
            }

            originURL = GetOriginUrl();
            restRequest = ChickenshockProperties.getQuestions.Replace("{id}", configurationAsUUID) + "/volume";
        } catch(EntryPointNotFoundException entryPointNotFoundException) {
            Debug.Log("EntryPointNotFoundException, probably becouse you started the game with the editor: " + entryPointNotFoundException);
            configurationAsUUID = ChickenshockProperties.editorConfiguration;
            originURL = "";
            restRequest = ChickenshockProperties.editorGetQuestions.Replace("{id}", configurationAsUUID) + "/volume";
        }
        string completeRequestString = originURL + restRequest;
        StartCoroutine(GetRequest(completeRequestString));
    }

    /// <summary>
    /// Loads the game configuration for the tutorial minigame
    /// </summary>
    private void InitTutorialMinigame()
    {
        string json = Resources.Load<TextAsset>("tutorialConfiguration.json").text;
        GameConfiguration gameConfiguration = JsonUtility.FromJson<GameConfiguration>(json);

        allUnusedQuestions = gameConfiguration.questions.ToList();
        time = gameConfiguration.time;
        timeLimit = gameConfiguration.time;
        UpdateVolumeLevel(1);
        questionCount = allUnusedQuestions.Count;
        PickRandomQuestion();
        questionLoaded = true;
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
                    GameConfiguration gameConfiguration = JsonUtility.FromJson<GameConfiguration>(webRequest.downloadHandler.text);
                    Question[] questions = gameConfiguration.questions;
                    allUnusedQuestions = questions.ToList();
                    ResultPanel.allQuestions = questions;
                    this.time = gameConfiguration.time;
                    this.timeLimit = gameConfiguration.time;
                    this.volumeLevel = gameConfiguration.volumeLevel;
                    UpdateVolumeLevel(volumeLevel);
                    questionCount = allUnusedQuestions.Count;
                    PickRandomQuestion();
                    this.questionLoaded = true;
                    break;
            }
        }
    }

    /// <summary>
    /// This method sends a Get request for the volume level to the chickenshock api and handles the response accordingly.
    /// </summary>
    public IEnumerator GetVolumeLevel()
    {
        String originURL;
        String restRequest;
        try
        {
            configurationAsUUID = GetConfiguration();
            originURL = GetOriginUrl();
            restRequest = ChickenshockProperties.getQuestions.Replace("{id}", configurationAsUUID) + "/volume";
        }
        catch (EntryPointNotFoundException entryPointNotFoundException)
        {
            Debug.Log("EntryPointNotFoundException, probably becouse you started the game with the editor: " + entryPointNotFoundException);
            configurationAsUUID = ChickenshockProperties.editorConfiguration;
            originURL = "";
            restRequest = ChickenshockProperties.editorGetQuestions.Replace("{id}", configurationAsUUID) + "/volume";
        }
        string completeRequestString = originURL + restRequest;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(completeRequestString))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                GameConfiguration gameConfiguration = JsonUtility.FromJson<GameConfiguration>(webRequest.downloadHandler.text);
                this.volumeLevel = gameConfiguration.volumeLevel;
                UpdateVolumeLevel(volumeLevel);
            }
            else
            {
                Debug.LogError(completeRequestString + ": Error: " + webRequest.error);
            }
        }
    }
    
    /// <summary>
    /// This function updates the level volume and applies the changes to all audio in the game
    /// </summary>
    private void UpdateVolumeLevel(int volumeLevel)
    {
        float volume = 0f;
        switch (volumeLevel)
        {
            case 0:
                volume = 0f;
                break;
            case 1:
                volume = 0.5f;
                break;
            case 2:
                volume = 1f;
                break;
            case 3:
                volume = 2f;
                break;
        }
        AudioListener.volume = volume;
    }

    /// <summary>
    /// This method starts a POST request coroutine that saves the result of the current round to the backend
    /// </summary>
    private void SaveRound()
    {
        Debug.Log("Save round details");
        string originURL;
        string restRequest;

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
        GameObject.Find("LoadingCircle").GetComponent<Image>().enabled = true;
        GameObject.Find("SpinningCircle").GetComponent<Image>().enabled = true;
    }

    /// <summary>
    /// This method sends a Post request and handles the response accordingly.
    /// </summary>
    /// <param name="uri"></param>
    private IEnumerator PostRequest(String uri)
    {
        GameResult round = new GameResult(questionCount,timeLimit,finishedInSeconds,correctKillsCount,wrongKillsCount,correctKillsCount + wrongKillsCount, shotCount,points,correctAnsweredQuestions,wrongAnsweredQuestions, configurationAsUUID, score, rewards);
        ResultPanel.correctAnsweredQuestions = correctAnsweredQuestions;
        ResultPanel.wrongAnsweredQuestions = wrongAnsweredQuestions;
        ResultPanel.finishedInSeconds = finishedInSeconds;
        ResultPanel.shotCount = shotCount;
        string jsonRound = JsonUtility.ToJson(round);
        byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonRound);
        GameResult receivedGameResult;

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
                    EndScreen.errorText = "Result Not Saved!: " + postRequest.error;
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(uri + ":\nReceived: " + postRequest.downloadHandler.text);          
                    string jsonResponse = postRequest.downloadHandler.text;
                    receivedGameResult = JsonUtility.FromJson<GameResult>(jsonResponse);
                    score = receivedGameResult.score;
                    rewards = receivedGameResult.rewards;
                    Debug.Log(score);
                    Debug.Log(rewards);        
                    
                    ResultButton.score = receivedGameResult.score;
                    ResultButton.rewards = receivedGameResult.rewards;


                    break;

            }
            postRequest.Dispose();
            SceneManager.LoadScene("EndScreen");
        }
    }

    /// <summary>
    /// This method unlocks your mouse cursor if you press the "p" key.
    /// </summary>
    private void UnlockCursor()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if(Input.GetKeyUp(KeyCode.P))
        {
            Cursor.lockState = CursorLockMode.Locked;
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
