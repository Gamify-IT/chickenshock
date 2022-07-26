using UnityEngine;

public class ShootWeapon : MonoBehaviour
{
    public float range = 100f;
    public RaycastHit hit;
    public Camera mainCamera;
    public ParticleSystem muzzleFlash;
    public Global globalScript;
    private Animator recoilAnimator;

    private void Start()
    {
        InitVariables();
    }

    void Update()
    {
        Shoot();
    }

    /// <summary>
    /// This method shoots the weapon if no chicken is killed yet by triggering the recoil animation and muzzle flash.
    /// After that it casts a ray from the mouse position to wherever you aim, if you hit a chicken while shooting it will get killed.
    /// </summary>
    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && !globalScript.pointsUpdated)
        {
            muzzleFlash.Play();
            recoilAnimator.SetTrigger("shoot");
            Ray ray = new Ray(this.transform.position, this.transform.forward);
            if (Physics.Raycast(ray, out hit, range))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.tag == "Chicken")
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// This method initializes the global script and recoil animator.
    /// </summary>
    private void InitVariables()
    {
        globalScript = FindObjectOfType<Global>();
        recoilAnimator = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Animator>();
    }

}
