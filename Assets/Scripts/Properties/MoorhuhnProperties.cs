using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoorhuhnProperties
{
    public static float ingamePlaytime = 10;

    //REST PATHS
    public static String getQuestions = "/api/v1/minigames/moorhuhn/configurations/{id}/questions";
    public static String saveRound = "/api/v1/minigames/moorhuhn/results";
}
