using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script handles playing the game again from the end screen.
/// </summary>
public class PlayAgainButton : MonoBehaviour
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
    }

    /// <summary>
    /// This Method is called when pressing the Play Again button in the end screen.
    /// It calls the click sound and calls the function for scenes loading after delay
    /// </summary>
    public void LoadGame()
    {
        PlayClickSound();
        Invoke("LoadScenes", delayBeforeScenesLoading);
    }

    /// <summary>
    /// This Method initializes the game again and resets the time, points and questions.
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
    /// This function is called by the <c>Play Again Button</c>.
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
