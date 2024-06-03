using UnityEngine;

/// <summary>
/// This script handles shooting and animating the weapon.
/// </summary>
public class ShootWeapon : MonoBehaviour
{
    public float range = 100f;
    public Camera mainCamera;
    public ParticleSystem muzzleFlash;
    private Animator recoilAnimator;
    private GameManager gameManager;
    private const string correctAnswerTag = "CorrectAnswer";
    private const string wrongAnswerTag = "WrongAnswer";
    public AudioClip shootSound;
    public AudioClip successSound;
    public AudioClip errorSound;
    private AudioSource audioSource;

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
        if (Input.GetButtonDown("Fire1") && !gameManager.GetRoundStatus())
        {
            gameManager.AddShot();
            muzzleFlash.Play();
            recoilAnimator.SetTrigger("shoot");
            Ray ray = new Ray(this.transform.position, this.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, range))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.name == "Chicken(Clone)")
                {
                    handleKill(hit);
                    Destroy(hit.transform.gameObject);
                }
                PlayShootSound();
            }
        }
    }

    private void handleKill(RaycastHit hit)
    {
        if (hit.transform.tag == correctAnswerTag)
        {
            PlaySuccessSound();
            gameManager.UpdatePoints(1);
            gameManager.AddCorrectKill();
            gameManager.addCorrectAnswerToResult(this.getChickenText(hit));
            gameManager.FinishRound(ChickenshockProperties.correctFeedbackText);
        }
        else if (hit.transform.tag == wrongAnswerTag)
        {
            PlayErrorSound();
            gameManager.UpdatePoints(-1);
            gameManager.AddWrongKill();
            gameManager.addWrongAnswerToResult(this.getChickenText(hit));
            gameManager.FinishRound(ChickenshockProperties.wrongFeedbackText);
        }
    }

    private string getChickenText(RaycastHit chicken)
    {
        return chicken.transform.Find("Shield").transform.Find("Cube").transform.Find("Canvas").transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>().text;
    }

    /// <summary>
    /// This method initializes the global script and recoil animator.
    /// </summary>
    private void InitVariables()
    {
        this.gameManager = GameManager.instance;
        recoilAnimator = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Animator>();
    }

    /// <summary>
    /// This function plays the shoot sound.
    /// </summary>
    private void PlayShootSound()
    {
        //get AudioSource component
        audioSource=GetComponent<AudioSource>();
        //add AudioSource component if necessary
        if(audioSource == null)
        {
            audioSource=gameObject.AddComponent<AudioSource>();
        }
        //set audio clip
        audioSource.clip=shootSound;

        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }


    /// <summary>
    /// This function plays the success sound.
    /// </summary>
    private void PlaySuccessSound()
    {
        //get AudioSource component
        audioSource=GetComponent<AudioSource>();
        //add AudioSource component if necessary
        if(audioSource == null)
        {
            audioSource=gameObject.AddComponent<AudioSource>();
        }
        //set audio clip
        audioSource.clip=successSound;
        
        if (successSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(successSound);
        }
    }

    /// <summary>
    /// This function plays the error sound.
    /// </summary>
    private void PlayErrorSound()
    {
        //get AudioSource component
        audioSource=GetComponent<AudioSource>();
        //add AudioSource component if necessary
        if(audioSource == null)
        {
            audioSource=gameObject.AddComponent<AudioSource>();
        }
        //set audio clip
        audioSource.clip=errorSound;
        
        if (errorSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(errorSound);
        }
    }
}
