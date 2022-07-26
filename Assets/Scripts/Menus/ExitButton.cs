using System.Runtime.InteropServices;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CloseMinigame();

    /// <summary>
    /// This method is triggered by clicking the exit button on the start and end screen. 
    /// It calls a JavaScript method that closes the Minigame IFrame.
    /// </summary>
    public void callCloseMinigame()
    {
        CloseMinigame();
    }
}
