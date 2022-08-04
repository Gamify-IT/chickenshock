using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoorhuhnProperties
{
    public static float ingamePlaytime = 10;

    //REST PATHS
    public static String getQuestions = "/minigames/chickenshock/api/v1/configurations/{id}/questions";
    public static String saveRound = "/minigames/chickenshock/api/v1/results";

    //PLAYER FEEDBACK
    public static String correctFeedbackText = "CORRECT!";
    public static String wrongFeedbackText = "WRONG!";
}
