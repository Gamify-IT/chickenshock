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

    public GameObject resultPanel;
    public TMP_Text extraInformation;
    public GameObject correctEntryPrefab; 
    public GameObject wrongEntryPrefab; 
    public Transform resultContent; 

    void Start()
    {
        Debug.Log("ResultPanel Start");
        UpdateResults();
    }

    public void UpdateResults()
    {
        Debug.Log("UpdateResults called");

        foreach (Transform child in resultContent)
        {
            Destroy(child.gameObject);
        }

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

            GameObject entryPrefab = null;

            var correctAnswer = correctAnsweredQuestions.Find(r => r.questionUUId == question.id);
            var wrongAnswer = wrongAnsweredQuestions.Find(r => r.questionUUId == question.id);

            if (correctAnswer != null)
            {
                entryPrefab = Instantiate(correctEntryPrefab, resultContent);
                Debug.Log($"{question.text} answered correctly with {correctAnswer.answer}");
            }
            else if (wrongAnswer != null)
            {
                entryPrefab = Instantiate(wrongEntryPrefab, resultContent);
                Debug.Log($"{question.text} answered incorrectly with {wrongAnswer.answer}");
            }
            else
            {
                continue;
            }

            if (entryPrefab != null)
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
        }

      extraInformation.text = $"time: {finishedInSeconds.ToString("F2")} seconds             amount of shots: {shotCount}";
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
