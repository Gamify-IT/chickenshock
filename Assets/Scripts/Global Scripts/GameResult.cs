using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the equivalent to the Result.java class in the backend
/// </summary>
[System.Serializable]
public class GameResult
{
    public int questionCount;
    public float timeLimit;
    public float finishedInSeconds;
    public int correctKillsCount;
    public int wrongKillsCount;
    public int killsCount;
    public int shotCount;
    public int points;
    public List<RoundResult> correctAnsweredQuestions;
    public List<RoundResult> wrongAnsweredQuestions;
    public string configurationAsUUID;

    public GameResult(int questionCount, float timeLimit, float finishedInSeconds, int correctKillsCount, int wrongKillsCount, int killsCount, int shotCount, int points, List<RoundResult> correctAnsweredQuestions, List<RoundResult> wrongAnsweredQuestions, string configurationAsUUID)
    {
        this.questionCount = questionCount;
        this.timeLimit = timeLimit;
        this.finishedInSeconds = finishedInSeconds;
        this.correctKillsCount = correctKillsCount;
        this.wrongKillsCount = wrongKillsCount;
        this.killsCount = killsCount;
        this.shotCount = shotCount;
        this.points = points;
        this.correctAnsweredQuestions = correctAnsweredQuestions;
        this.wrongAnsweredQuestions = wrongAnsweredQuestions;
        this.configurationAsUUID = configurationAsUUID;
    }
}
