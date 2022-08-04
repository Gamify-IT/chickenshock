using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoundResult
{
    public string questionUUId;
    public string answer;

    public RoundResult(string questionUUId, string answer)
    {
        this.questionUUId = questionUUId;
        this.answer = answer;
    }
}
