using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScript : MonoBehaviour
{

    public static int points;

    // Update is called once per frame
    void Update()
    {
        GameObject.FindGameObjectWithTag("Point Overlay").GetComponent<TMPro.TextMeshProUGUI>().text = points.ToString();
    }
}
