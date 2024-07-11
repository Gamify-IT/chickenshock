using UnityEngine;

/// <summary>
/// This script handles updating the end screen items.
/// </summary>
public class EndScreen : MonoBehaviour
{

    public static int points;
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
        if(errorText != null){
            GameObject.FindGameObjectWithTag("Point Overlay").GetComponent<TMPro.TextMeshProUGUI>().text = errorText;
        }else{
            GameObject.FindGameObjectWithTag("Point Overlay").GetComponent<TMPro.TextMeshProUGUI>().text = "";
        }
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
    }
}
