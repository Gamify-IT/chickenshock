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
    public Global globalScript;
    Animator recoilAnimator;

    private void Start()
    {
        globalScript = GameObject.FindObjectOfType<Global>();
        recoilAnimator = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !globalScript.killedAChicken)
        {
            Debug.Log("Shooting the Weapon!");
            Shoot();
        }
    }

    void Shoot()
    {
        //play the muzzleflash to give shooting feeling
        muzzleFlash.Play();
        recoilAnimator.SetTrigger("shoot");

        Ray ray = new Ray(this.transform.position, this.transform.forward);
        //cast a ray from the mouse position to wherever you aim, if you hit a chicken while shooting it will get killed
        if(Physics.Raycast(ray, out hit, range))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Chicken")
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
