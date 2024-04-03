using UnityEngine;
using System.Collections;
using System.Linq;

public class gun : MonoBehaviour
{
    public int MaxAmmo = 10;
    public int ammoReserve = 20;
    public int maxAmmoReserve = 120;
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 3.5f;
    public int ammoCount = 10;
    [HideInInspector] public bool isReloading = false;
    private bool wasRunning = false;

    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject bloodImpactEffect;
    public Animation gunMovement;
    [SerializeField]
    private AudioSource gunshot;
    [SerializeField]
    private AudioSource reload;
    [SerializeField]
    private AudioSource emptyFire;

    private float nextTimeToFire = 0f;
    public LayerMask layerMask;
    // private int layerMask = ~0;

    public PlayerTest playerTest;
    public FPSController fpsController;

    // Manage the different possible Ammo UI elements
    public HUDManager hudManager;
    public AmmoCounter ammoCounter;
    public AmmoBulletsManager ammoBulletsManager;
    public AmmoBar ammoBar;
    public gunHeat gunHeat;

    void Start()
    {
        Debug.Log("ammoCount: " + ammoCount + " MaxAmmo: " + MaxAmmo);
        hudManager.currentAmmoUpdater.SetAmmo(ammoCount, MaxAmmo);
        hudManager.currentAmmoUpdater.SetReserve(ammoReserve);
        // ammoCounter.SetAmmo(ammoCount, MaxAmmo);
        // gunHeat.SetAmmo(ammoCount, MaxAmmo);
        // ammoBulletsManager.createBullets(ammoCount);
    }

    void Awake()
    {
        // ammoCount = MaxAmmo;
        // ammoBulletsManager.createBullets(ammoCount);
        // ammoBar.SetAmmo(ammoCount,MaxAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReloading && !playerTest.isDead && !fpsController.isRunning)
        {
            if (wasRunning)
            {
                gunMovement.Stop();
                wasRunning = false;
            }

            if ((Input.GetButtonDown("Fire1")) && (Time.time >= nextTimeToFire) && (ammoCount > 0))
            {
                nextTimeToFire = Time.time + 1f/fireRate;
                Shoot();
                ammoCount -= 1;
                hudManager.currentAmmoUpdater.SetAmmo(ammoCount, MaxAmmo);
                // ammoCounter.SetAmmo(ammoCount, MaxAmmo);
                // ammoBulletsManager.createBullets(ammoCount);
                // ammoBar.SetAmmo(ammoCount, MaxAmmo);
                // gunHeat.SetAmmo(ammoCount, MaxAmmo);
            }

            else if ((Input.GetButtonDown("Fire1")) && ammoCount == 0)
            {
                emptyFire.PlayOneShot(emptyFire.clip);
            }

            if (!gunMovement.isPlaying)
            {
                gunMovement.Play("idlePistol");
            }

            if (Input.GetKeyDown(KeyCode.R) && ammoCount < MaxAmmo && ammoReserve > 0)
            {
                StartCoroutine(ReloadGun());
            }
            
            
        }
        
        else if (fpsController.isRunning && fpsController.isMoving && !isReloading)
        {
            wasRunning = true;
            gunMovement.Play("sprint");
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        gunshot.PlayOneShot(gunshot.clip);
        gunMovement.Play("Gun Fire");

        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range, layerMask,QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.transform.name);

            Enemy enemy = hit.transform.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                GameObject bloodImpactGO = Instantiate(bloodImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                bloodImpactGO.transform.SetParent(enemy.transform);
                Destroy(bloodImpactGO, 2f);
            }

            else
            {
                if (hit.transform.name == "Target")
                {
                    TutorialManager.instance.TaskCompleted(2);
                }
                            
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
                Debug.Log("HIT!");
            }


        }
    }

    IEnumerator ReloadGun()
    {
        isReloading = true;
        // Play Animation and Sound
        gunMovement.Play("Gun Reload");
        reload.PlayOneShot(reload.clip);
        // Wait A certain Time
        yield return new WaitForSeconds(gunMovement["Gun Reload"].length);

        // Gather Study metrics
        StudyMetricManager.instance.reloadCounter++;
        Debug.Log("ammoCount: " + ammoCount + " MaxAmmo: " + MaxAmmo);
        float remainingAmmoPercentage = ((float)ammoCount / MaxAmmo) * 100f;
        Debug.Log(remainingAmmoPercentage);
        StudyMetricManager.instance.remainingAmmoPercentages.Add(remainingAmmoPercentage);

        // Reset Ammunition Counter and reserve
        int ammoUsed = MaxAmmo - ammoCount;
        if (ammoUsed > ammoReserve) ammoUsed = ammoReserve;
        ammoCount += ammoUsed;
        ammoReserve -= ammoUsed;
        

        hudManager.currentAmmoUpdater.SetAmmo(ammoCount, MaxAmmo);
        hudManager.currentAmmoUpdater.SetReserve(ammoReserve);

        // ammoCounter.SetAmmo(ammoCount, MaxAmmo);
        // ammoBulletsManager.createBullets(ammoCount);
        // ammoBar.SetAmmo(ammoCount, MaxAmmo);
        // gunHeat.SetAmmo(ammoCount, MaxAmmo);

        if(TutorialManager.instance.isActive)
        {
            TutorialManager.instance.TaskCompleted(1);
        }
        
        isReloading = false;
    }

    public void addAmmo(int ammoAmount)
    {
        if ((ammoAmount + ammoReserve) <= maxAmmoReserve)
        {
            ammoReserve += ammoAmount;
        }
        else
        {
            ammoReserve = maxAmmoReserve;
        }

        hudManager.currentAmmoUpdater.SetReserve(ammoReserve);

    }
}
