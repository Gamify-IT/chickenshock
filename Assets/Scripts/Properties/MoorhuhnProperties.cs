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

    public static String localOrigin = "http://localhost/minigames/chickenshock/api/v1/configurations/4683e231-ff5c-4b42-be34-2b4bcf8a3f39/questions";

    //PLAYER FEEDBACK
    public static String correctFeedbackText = "CORRECT!";
    public static String wrongFeedbackText = "WRONG!";
}
