using System;
using System.Collections.Generic;

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

    public string getId()
    {
        return id;
    }

    public void setId(string id)
    {
        this.id = id;
    }

    public String getQuestionText()
    {
        return text;
    }

    public void setQuestion(String questionText)
    {
        this.text = questionText;
    }

    public String getRightAnswer()
    {
        return rightAnswer;
    }

    public void setRightAnswer(String rightAnswer)
    {
        this.rightAnswer = rightAnswer;
    }

    public void setWrongAnswers(List<String> wrongAnswers)
    {
        this.wrongAnswers = wrongAnswers;
    }

    public List<String> getWrongAnswers()
    {
        return this.wrongAnswers;
    }
}
