using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera mainCamera;
    public ParticleSystem muzzleFlash;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hit;
        if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, range))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.name == "Toon Chicken")
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }
}
