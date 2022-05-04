using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTarget : MonoBehaviour 
{   
    
    [SerializeField] private Camera mainCamera;
    // Update is called once per frame
    void Update()
    {
        //Cursor.visible = false;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            //Debug.Log(raycastHit.point);
            transform.position = Input.mousePosition;
        }
    }
}
