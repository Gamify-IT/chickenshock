using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<string> correctAnsweredQuestions;
    public List<string> wrongAnsweredQuestions;
    public string configurationAsUUID;

    public GameResult(int questionCount, float timeLimit, float finishedInSeconds, int correctKillsCount, int wrongKillsCount, int killsCount, int shotCount, int points, List<string> correctAnsweredQuestions, List<string> wrongAnsweredQuestions, string configurationAsUUID)
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
