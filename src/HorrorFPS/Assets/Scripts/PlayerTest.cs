using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTest : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HUDManager hudManager;

    public HealthBar healthBar;
    public HeartHealthManager heartHealthManager;
    public HealthNumber healthNumber;
    public HealthVignette healthVignette;
    public HitFlash hitFlash;
    public bool isDead = false;


    public AudioSource damageSound;
    public Animation cameraMovement;

    public GameObject playerCamera;
    public FPSController fpsController;
    public GameObject spawnPoint;
    private Transform spawnTransform;

    public bool HasKey = false;

    private Vector3 defaultCameraPosition;
    private Quaternion defaultCameraRotation;

    // Start is called before the first frame update
    void Start()
    {
        spawnTransform = spawnPoint.transform;
        defaultCameraPosition = playerCamera.transform.localPosition;
        defaultCameraRotation = playerCamera.transform.localRotation;

        fpsController = GetComponent<FPSController>();

        hudManager.currentHealthUpdater.SetHealth(maxHealth, currentHealth);
        // healthNumber.SetHealth(maxHealth, currentHealth);
        // healthVignette.SetHealth(maxHealth,currentHealth);
        
        // healthBar.SetHealth(maxHealth);
        // heartHealthManager.createHearts(maxHealth, currentHealth);
    }

    void Awake(){
        // playerCamera = GameObject.FindWithTag("MainCamera");
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.K)){
        //     print("K key pressed");
        //     TakeDamage(10);
        // }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyHand"))
        {
            Debug.Log("HAND HIT ONCE");
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damage){
        if(currentHealth - damage >= 0)
        {
            currentHealth -= damage;
        }
        else
        {
            currentHealth = 0;
        }

        hitFlash.DisplayHitFlash();
        damageSound.PlayOneShot(damageSound.clip);

        hudManager.currentHealthUpdater.SetHealth(maxHealth, currentHealth);

        if (currentHealth == 0)
        {
            StudyMetricManager.instance.deathCounter++;

            FindObjectOfType<GameManager>().EndGame();
            isDead = true;
            cameraMovement.Play("dieCam");
        }

        // healthBar.SetHealth(currentHealth);
        // heartHealthManager.createHearts(maxHealth, currentHealth);

        // healthNumber.SetHealth(maxHealth, currentHealth);
        // healthVignette.SetHealth(maxHealth,currentHealth);

    }

    public void Heal(int health)
    {
        if (currentHealth < maxHealth)
        {
            // Gather Study Metrics
            float remainingHealthPercentage;
            float wastedHealthValue;            
            StudyMetricManager.instance.healCounter++;

            if (currentHealth + health >maxHealth)
            {
                wastedHealthValue = ((float)currentHealth + health) - 100f;
                int healthAboveThreshold = currentHealth - 20;
                remainingHealthPercentage = ((float)healthAboveThreshold / 80) * 100f;


                currentHealth=maxHealth;
            }
            else
            {
                wastedHealthValue = 0f;
                remainingHealthPercentage = 0f;
                currentHealth += health;
            }

            StudyMetricManager.instance.remainingHealthPercentages.Add(remainingHealthPercentage);
            StudyMetricManager.instance.wastedHealthValues.Add(wastedHealthValue);
        }

        hudManager.currentHealthUpdater.SetHealth(maxHealth, currentHealth);

        // healthNumber.SetHealth(maxHealth, currentHealth);
        // healthVignette.SetHealth(maxHealth,currentHealth);

        // healthBar.SetHealth(currentHealth);
        // heartHealthManager.createHearts(maxHealth, currentHealth);
    }

    public void Respawn()
    {
        // Heal(100);
        currentHealth = 100;
        hudManager.currentHealthUpdater.SetHealth(maxHealth, currentHealth);
        Debug.Log(defaultCameraPosition);
        Debug.Log(defaultCameraRotation);

        Debug.Log("Spawn Position: " + spawnTransform.position);
        Debug.Log("Spawn Rotation: " + spawnTransform.rotation.eulerAngles);
        cameraMovement.Stop("dieCam");

        playerCamera.transform.localPosition = defaultCameraPosition;
        playerCamera.transform.localRotation = defaultCameraRotation;
        
        fpsController.characterController.enabled = false;
        transform.position = spawnTransform.position;
        transform.rotation = spawnTransform.rotation;
        fpsController.characterController.enabled = true;

        

        isDead = false;
    }

}
