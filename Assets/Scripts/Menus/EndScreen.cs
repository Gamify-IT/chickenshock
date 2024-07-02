using UnityEngine;

/// <summary>
/// This script handles updating the end screen items.
/// </summary>
public class EndScreen : MonoBehaviour
{

    public static int points;
    public static int rewards;
    public static string errorText;

    private void Start()
    {
        ShowCursor();
        UpdateEndPoints();
        
    }

    /// <summary>
    /// This method updates the text that shows the points on the End Screen.
    /// </summary>
    private void UpdateEndPoints()
    {
        TMPro.TextMeshProUGUI textComponent = GameObject.FindGameObjectWithTag("Point Overlay").GetComponent<TMPro.TextMeshProUGUI>();

        if (errorText != null){
            textComponent.text = errorText;
        }
        else
        {
            string scoreText = $"Your Score: {points}\nYou've gained {rewards} coins!";
            textComponent.text = scoreText;
        }
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
    }
}
