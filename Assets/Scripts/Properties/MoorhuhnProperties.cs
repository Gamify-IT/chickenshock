using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoorhuhnProperties
{
    public static float ingamePlaytime = 50;

    //REST PATHS
    public static String getQuestions = "minigames/moorhuhn/api/v1/configurations/{id}/questions";
    public static String saveRound = "minigames/moorhuhn/api/v1/results";
}
