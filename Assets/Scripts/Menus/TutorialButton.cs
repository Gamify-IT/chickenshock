using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject.Find("TutorialButton").transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject.Find("TutorialButton").transform.GetChild(1).gameObject.SetActive(false);
    }
}