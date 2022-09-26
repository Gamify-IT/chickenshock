using System;
using System.Collections.Generic;

/// <summary>
/// This is the equivalent to the Question.java class in the backend
/// </summary>
[System.Serializable]
public class Question
{
    public string id;
    public String text;
    public String rightAnswer;
    public List<String> wrongAnswers;

    public Question(String level, String text, String rightAnswer, List<String> wrongAnswers)
    {
        this.id = level;
        this.text = text;
        this.rightAnswer = rightAnswer;
        this.wrongAnswers = wrongAnswers;
    }
}
