using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script sets various chickenshock properties.
/// </summary>
public static class ChickenshockProperties
{
    //REST PATHS
    public static String getQuestions = "/minigames/chickenshock/api/v1/configurations/{id}";
    public static String saveRound = "/minigames/chickenshock/api/v1/results";

    //EDITOR VALUES
    public static String editorGetQuestions = "http://localhost/minigames/chickenshock/api/v1/configurations/{id}";
    public static String editorConfiguration = "70fcd00c-b67c-46f2-be73-961dc0bc8de1";

    //PLAYER FEEDBACK
    public static String correctFeedbackText = "CORRECT!";
    public static String wrongFeedbackText = "WRONG!";
}
