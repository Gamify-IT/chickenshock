using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultPanel : MonoBehaviour
{
    public static List<RoundResult> correctAnsweredQuestions;
    public static List<RoundResult> wrongAnsweredQuestions;
    public static Question[] allQuestions;

    public TMP_Text resultStatusText;
    public TMP_Text questionsText;
    public TMP_Text answersText;
    public GameObject resultPanel;

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
            answersText.text += $"{question.rightAnswer}\n";

            bool isCorrect = correctAnsweredQuestions.Exists(r => r.questionUUId == question.id);
            bool isWrong = wrongAnsweredQuestions.Exists(r => r.questionUUId == question.id);

            if (isCorrect)
            {
                resultStatusText.text += ":)\n";
                Debug.Log($"{question.text} answered correctly");
            }
            else if (isWrong)
            {
                resultStatusText.text += ":(\n";
                Debug.Log($"{question.text} answered incorrectly");
            }
            else
            {
                resultStatusText.text += "-\n";
                Debug.Log($"{question.text} not answered");
            }
        }
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
