using System.Collections;
using System.Collections.Generic;
// using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoBulletsManager : MonoBehaviour, IAmmoUpdater
{
    public GameObject bulletPrefab;
    public GameObject fullMagPrefab;
    public GameObject reserveBulletPrefab;
    public GameObject reserveBulletsGrid;
    private int maxAmmo;
    
    public void SetAmmo(int ammoCount, int MaxAmmo)
    {
        maxAmmo = MaxAmmo;
        createBullets(ammoCount);
    }

    public void createBullets(int currentAmmo)
    {
        clearAllBullets();

        for (int i=0; i<currentAmmo; i++)
        {
            createBullet();
        }
    }

    public void createBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.SetParent(transform);
    }

    public void clearAllBullets()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }
    
    public void SetReserve(int reserveAmmo)
    {
        clearReserve();
        int magCount = reserveAmmo / maxAmmo;
        int bulletCount = reserveAmmo % maxAmmo;

        for (int i=0; i < magCount; i++)
        {
            GameObject newMag = Instantiate(fullMagPrefab);
            newMag.transform.SetParent(reserveBulletsGrid.transform);
        }

        for (int i=0; i<bulletCount; i++)
        {
            GameObject newBullet = Instantiate(reserveBulletPrefab);
            newBullet.transform.SetParent(reserveBulletsGrid.transform);
        }
    }

    private void clearReserve()
    {
        foreach(Transform t in reserveBulletsGrid.transform)
        {
            Destroy(t.gameObject);
        }
    }

}
