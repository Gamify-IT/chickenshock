using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    public static List<RoundResult> correctAnsweredQuestions;
    public static List<RoundResult> wrongAnsweredQuestions;
    public static float finishedInSeconds;
    public static int shotCount;
    public static Question[] allQuestions;

    public TMP_Text resultStatusText;
    public TMP_Text questionsText;
    public TMP_Text answersText;
    public GameObject resultPanel;
    public TMP_Text extraInformation;

    void Start()
    {
        Debug.Log("ResultPanel Start");
        UpdateResults();
    }

    public void UpdateResults()
    {
        Debug.Log("UpdateResults called");

        questionsText.text = "";
        answersText.text = "";
        resultStatusText.text = "";
        extraInformation.text = "";

        if (allQuestions == null)
        {
            Debug.Log("allQuestions is null");
            return;
        }

        if (correctAnsweredQuestions == null)
        {
            Debug.Log("correctAnsweredQuestions is null");
            return;
        }

        if (wrongAnsweredQuestions == null)
        {
            Debug.Log("wrongAnsweredQuestions is null");
            return;
        }

        foreach (var question in allQuestions)
        {
            Debug.Log($"Processing question: {question.text}");

            questionsText.text += $"{question.text}\n";

            var correctAnswer = correctAnsweredQuestions.Find(r => r.questionUUId == question.id);
            var wrongAnswer = wrongAnsweredQuestions.Find(r => r.questionUUId == question.id);

            if (correctAnswer != null)
            {
                answersText.text += $"{correctAnswer.answer}\n";
                resultStatusText.text += ":)\n";
                Debug.Log($"{question.text} answered correctly with {correctAnswer.answer}");
            }
            else if (wrongAnswer != null)
            {
                answersText.text += $"{wrongAnswer.answer}\n";
                resultStatusText.text += ":(\n";
                Debug.Log($"{question.text} answered incorrectly with {wrongAnswer.answer}");
            }
            else
            {
                answersText.text += "-\n";
                resultStatusText.text += "-\n";
                Debug.Log($"{question.text} not answered");
            }
        }

        extraInformation.text = $"Your time: {finishedInSeconds} Your amount of shots: {shotCount}";
    }

    public void OpenResultPanel()
    {
        Debug.Log("Opening Result Panel");
        resultPanel.SetActive(true);
        UpdateResults();
    }

    public void CloseResultPanel()
    {
        Debug.Log("Closing Result Panel");
        resultPanel.SetActive(false);
    }
}
