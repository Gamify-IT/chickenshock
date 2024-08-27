using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// This script handles closing chickenshock.
/// </summary>
public class ExitButton : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;
    public float delayBeforeClose=0.5f;

    [DllImport("__Internal")]
    private static extern void CloseMinigame();

    /// <summary>
    /// This method is triggered by clicking the exit button on the start and end screen. 
    /// It calls a JavaScript method that closes the Minigame IFrame.
    /// </summary>
    public void callCloseMinigame()
    {
        PlayClickSound();
        Invoke("CloseMinigame", delayBeforeClose);
    }

    /// <summary>
    /// This function is called by the <c>Exit Button</c>.
    /// This function plays the click sound.
    /// </summary>
    private void PlayClickSound()
    {
        audioSource=GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource=gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip=clickSound;

        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
