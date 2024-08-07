using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the equivalent to the RoundResult.java class in the backend
/// </summary>
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
