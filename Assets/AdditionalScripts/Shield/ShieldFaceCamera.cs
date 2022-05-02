using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldFaceCamera : MonoBehaviour
{

    public GameObject camera;
    public GameObject shield;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        shield.transform.LookAt(camera.transform);
    }
}
