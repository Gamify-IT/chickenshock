using System;
using System.Collections.Generic;

[System.Serializable]
public class Question
{
    public long id;
    public String level;
    public String text;
    public String rightAnswer;
    public List<String> wrongAnswers;

    public Question(String level, String text, String rightAnswer, List<String> wrongAnswers)
    {
        this.level = level;
        this.text = text;
        this.rightAnswer = rightAnswer;
        this.wrongAnswers = wrongAnswers;
    }

    public long getId()
    {
        return id;
    }

    public void setId(long id)
    {
        this.id = id;
    }

    public String getLevel()
    {
        return level;
    }

    public void setLevel(String level)
    {
        this.level = level;
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
