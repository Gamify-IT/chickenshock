using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script handles starting the game from the main screen.
/// </summary>
public class StartButton : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;
    public float delayBeforeScenesLoading=0.1f;

    public void Start()
    {
        audioSource=GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource=gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip=clickSound;
        AudioListener.volume = 0f;
    }

    /// <summary>
    /// This method calls the click sound and calls the function for scenes loading after delay
    /// </summary>
    public void LoadGame()
    {
        PlayClickSound();
        Invoke("LoadScenes", delayBeforeScenesLoading);
    }

    /// <summary>
    /// This method loads the Game scene and loads the HUD over it. 
    /// </summary>
    public void LoadScenes()
    {
        SceneManager.LoadScene("Game");   
        Debug.Log("loaded game scene");
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("PlayerHUD", LoadSceneMode.Additive);
        Debug.Log("loaded player HUD scene");
    }


    /// <summary>
    /// This function is called by the <c>Start Button</c>.
    /// This function plays the click sound.
    /// </summary>
    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
