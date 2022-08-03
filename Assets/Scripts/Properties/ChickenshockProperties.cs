using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChickenshockProperties
{
    public static float ingamePlaytime = 10;

    //REST PATHS
    public static String getQuestions = "/minigames/chickenshock/api/v1/configurations/{id}/questions";
    public static String saveRound = "/minigames/chickenshock/api/v1/results";
}
