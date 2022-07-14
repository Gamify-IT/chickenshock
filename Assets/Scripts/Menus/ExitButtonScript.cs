using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ExitButtonScript : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CloseMinigame();

    public void callCloseMinigame()
    {
        CloseMinigame();
    }
}
