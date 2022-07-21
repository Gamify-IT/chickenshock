using UnityEngine;

public class EndScreen : MonoBehaviour
{

    public static int points;

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
        GameObject.FindGameObjectWithTag("Point Overlay").GetComponent<TMPro.TextMeshProUGUI>().text = points.ToString();
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
    }
}
