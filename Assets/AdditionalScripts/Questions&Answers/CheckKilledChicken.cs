using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckKilledChicken : MonoBehaviour
{

    public GameObject[] answers;

    // Update is called once per frame
    void Update()
    {
        this.answers = GameObject.FindGameObjectsWithTag("Answer");
        Debug.Log("There are " + this.answers.Length + " Chickens on the field");

        if (answers.Length < 5)
        {
            bool rightAnswer = false;
            foreach(GameObject answer in answers){
                if (answer.GetComponent<TMPro.TextMeshProUGUI>().text.Equals("Answer"))
                {
                    rightAnswer = true;
                }
            }
            if (rightAnswer)
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
