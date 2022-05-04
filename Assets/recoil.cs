using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public Vector3 upRecoil;
    Vector3 originalRotation;
    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            AddRecoil();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopRecoil();
        }
    }

    /*
     * add the desired amount of weapon movement to the rotation
     */
    private void AddRecoil()
    {
        transform.localEulerAngles += upRecoil;
    }

    /*
     * reset the rotation to the original state of the weapon
     */
    private void StopRecoil()
    {
        transform.localEulerAngles = originalRotation;
    }
}
