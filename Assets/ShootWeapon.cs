using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWeapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public RaycastHit hit;
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
        //play the muzzleflash to give shooting feeling
        muzzleFlash.Play();

        Ray ray = new Ray(this.transform.position, this.transform.forward);
        //Ray rayToCast = mainCamera.ScreenPointToRay(new Vector2(Screen.height / 2, Screen.width / 2));
        //cast a ray from the mouse position to wherever you aim, if you hit a chicken while shooting it will get killed
        if(Physics.Raycast(ray, out hit, range))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.name == "Toon Chicken")
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hit.point, 1f);
    }
}
