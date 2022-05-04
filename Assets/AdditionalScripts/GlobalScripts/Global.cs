using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    public GameObject[] answers;
    public int points = 0;
    public int chickenCount = 5;
    // Update is called once per frame
    void Update()
    {
        this.checkChickens();
        this.updatePoints();
    }

    void checkChickens()
    {
        this.answers = GameObject.FindGameObjectsWithTag("Answer");
        Debug.Log("There are " + this.answers.Length + " Chickens on the field");

        if (answers.Length < chickenCount)
        {
            bool rightAnswerAvailable = false;
            foreach (GameObject answer in answers)
            {
                if (answer.GetComponent<TMPro.TextMeshProUGUI>().text.Equals("Answer"))
                {
                    rightAnswerAvailable = true;
                }
            }
            if (rightAnswerAvailable)
            {
                this.points--;
                Debug.Log("YOU KILLTED THE WRONG CHICKEN, FKKKKKKKK");
            }
            else
            {
                this.points++;
                Debug.Log("YOU KILLTED THE RIGHT CHICKEN, YIPPPPPPI");
            }
            this.chickenCount--;
        }
    }

    /**
     * This Method updates the Point counter 
     * in the top left of the Screen.
     */
    void updatePoints()
    {
        GameObject pointOverlay = GameObject.FindGameObjectWithTag("Point Overlay");
        pointOverlay.GetComponent<TMPro.TextMeshProUGUI>().text = ""+this.points;
    }
}
