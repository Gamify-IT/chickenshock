using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult
{
    private int questionCount;
    private float timeLimit;
    private float finishedInSeconds;
    private int correctKillsCount;
    private int wrongKillsCount;
    private int killsCount;
    private int shotCount;
    private int points;
    private List<string> correctAnsweredQuestions;
    private List<string> wrongAnsweredQuestions;
    private string configurationAsUUID;

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
