using System;

[System.Serializable]
public class Question
{
    public long id;
    public String level;
    public String questionText;
    public String rightAnswer;
    public String wrongAnswerOne;
    public String wrongAnswerTwo;
    public String wrongAnswerThree;
    public String wrongAnswerFour;

    public Question(String level, String questionText, String rightAnswer, String wrongAnswerOne, String wrongAnswerTwo, String wrongAnswerThree, String wrongAnswerFour)
    {
        this.level = level;
        this.questionText = questionText;
        this.rightAnswer = rightAnswer;
        this.wrongAnswerOne = wrongAnswerOne;
        this.wrongAnswerTwo = wrongAnswerTwo;
        this.wrongAnswerThree = wrongAnswerThree;
        this.wrongAnswerFour = wrongAnswerFour;
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
        return questionText;
    }

    public void setQuestion(String questionText)
    {
        this.questionText = questionText;
    }

    public String getRightAnswer()
    {
        return rightAnswer;
    }

    public void setRightAnswer(String rightAnswer)
    {
        this.rightAnswer = rightAnswer;
    }

    public String getWrongAnswerOne()
    {
        return wrongAnswerOne;
    }

    public void setWrongAnswerOne(String wrongAnswerOne)
    {
        this.wrongAnswerOne = wrongAnswerOne;
    }

    public String getWrongAnswerTwo()
    {
        return wrongAnswerTwo;
    }

    public void setWrongAnswerTwo(String wrongAnswerTwo)
    {
        this.wrongAnswerTwo = wrongAnswerTwo;
    }

    public String getWrongAnswerThree()
    {
        return wrongAnswerThree;
    }

    public void setWrongAnswerThree(String wrongAnswerThree)
    {
        this.wrongAnswerThree = wrongAnswerThree;
    }

    public String getWrongAnswerFour()
    {
        return wrongAnswerFour;
    }

    public void setWrongAnswerFour(String wrongAnswerFour)
    {
        this.wrongAnswerFour = wrongAnswerFour;
    }


}
