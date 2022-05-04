using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    public GameObject[] answers;

    // Update is called once per frame
    void Update()
    {
        this.answers = GameObject.FindGameObjectsWithTag("Answer");
        Debug.Log("There are " + this.answers.Length + " Chickens on the field");

        if (answers.Length < 5)
        {
            bool rightAnswerAvailable = false;
            foreach(GameObject answer in answers){
                if (answer.GetComponent<TMPro.TextMeshProUGUI>().text.Equals("Answer"))
                {
                    rightAnswerAvailable = true;
                }
            }
            if (rightAnswerAvailable)
            {
                Debug.Log("YOU KILLTED THE WRONG CHICKEN, FKKKKKKKK");
            }
            else
            {
                Debug.Log("YOU KILLTED THE RIGHT CHICKEN, YIPPPPPPI");
            }
        }
    }
}
