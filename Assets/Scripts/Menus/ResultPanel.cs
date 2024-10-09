using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the result panel, which displays the player's performance after completing a set of questions.
/// It updates the UI to show the correct and incorrect answers, along with additional statistics like time and shot count.
/// </summary>
public class ResultPanel : MonoBehaviour
{
    public static List<RoundResult> correctAnsweredQuestions;
    public static List<RoundResult> wrongAnsweredQuestions;
    public static float finishedInSeconds;
    public static int shotCount;
    public static Question[] allQuestions;

    public GameObject resultPanel;
    public TMP_Text extraInformation;
    public GameObject correctEntryPrefab;
    public GameObject wrongEntryPrefab;
    public Transform resultContent;


    /// <summary>
    /// This method is called when the result panel starts, initializing the results.
    /// </summary>
    void Start()
    {
        Debug.Log("ResultPanel Start");
        UpdateResults();
    }

    /// <summary>
    /// Updates the results displayed on the result panel. It checks for correct and wrong answers 
    /// and instantiates the appropriate UI elements to display them.
    /// </summary>
    public void UpdateResults()
    {
        Debug.Log("UpdateResults called");

        ClearPreviousResults();

        if (!ValidateData()) return;

        foreach (var question in allQuestions)
        {
            Debug.Log($"Processing question: {question.text}");

            var correctAnswer = correctAnsweredQuestions.Find(r => r.questionUUId == question.id);
            var wrongAnswer = wrongAnsweredQuestions.Find(r => r.questionUUId == question.id);

            GameObject entryPrefab = GetEntryPrefab(correctAnswer, wrongAnswer);
            if (entryPrefab == null) continue;

            SetEntryText(entryPrefab, question, correctAnswer, wrongAnswer);
        }

        extraInformation.text = $"time: {finishedInSeconds:F2} seconds             amount of shots: {shotCount}";
    }

    /// <summary>
    /// Clears all previous results displayed on the result panel.
    /// </summary>
    private void ClearPreviousResults()
    {
        foreach (Transform child in resultContent)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Validates the required data to ensure it is not null.
    /// </summary>
    /// <returns>Returns true if data is valid, false otherwise.</returns>
    private bool ValidateData()
    {
        if (allQuestions == null)
        {
            Debug.Log("allQuestions is null");
            return false;
        }

        if (correctAnsweredQuestions == null)
        {
            Debug.Log("correctAnsweredQuestions is null");
            return false;
        }

        if (wrongAnsweredQuestions == null)
        {
            Debug.Log("wrongAnsweredQuestions is null");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Instantiates the correct or wrong entry prefab based on the answers.
    /// </summary>
    /// <param name="correctAnswer">The correct answer for the question.</param>
    /// <param name="wrongAnswer">The wrong answer for the question.</param>
    /// <returns>The instantiated prefab, or null if neither answer is found.</returns>
    private GameObject GetEntryPrefab(RoundResult correctAnswer, RoundResult wrongAnswer)
    {
        if (correctAnswer != null)
        {
            Debug.Log($"{correctAnswer.questionUUId} answered correctly");
            return Instantiate(correctEntryPrefab, resultContent);
        }
        else if (wrongAnswer != null)
        {
            Debug.Log($"{wrongAnswer.questionUUId} answered incorrectly");
            return Instantiate(wrongEntryPrefab, resultContent);
        }

        return null;
    }

    /// <summary>
    /// Sets the text for the question and its corresponding answer on the prefab.
    /// </summary>
    /// <param name="entryPrefab">The prefab to set the text on.</param>
    /// <param name="question">The question object.</param>
    /// <param name="correctAnswer">The correct answer object.</param>
    /// <param name="wrongAnswer">The wrong answer object.</param>
    private void SetEntryText(GameObject entryPrefab, Question question, RoundResult correctAnswer, RoundResult wrongAnswer)
    {
        TMP_Text questionText = entryPrefab.transform.Find("QuestionText").GetComponent<TMP_Text>();
        TMP_Text answerText = entryPrefab.transform.Find("AnswerText").GetComponent<TMP_Text>();

        questionText.text = question.text;

        if (correctAnswer != null)
        {
            answerText.text = correctAnswer.answer;
        }
        else if (wrongAnswer != null)
        {
            answerText.text = wrongAnswer.answer;
        }
        else
        {
            answerText.text = "-";
        }
    }


    /// <summary>
    /// This method opens the result panel and updates the results displayed on it.
    /// </summary>
    public void OpenResultPanel()
    {
        Debug.Log("Opening Result Panel");
        resultPanel.SetActive(true);
        UpdateResults();
    }

    /// <summary>
    /// This method closes the result panel and hides it from view.
    /// </summary>
    public void CloseResultPanel()
    {
        Debug.Log("Closing Result Panel");
        resultPanel.SetActive(false);
    }
}
