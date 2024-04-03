using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour, IAmmoUpdater
{
    public Image fill;
    public GameObject reserveGrid;
    public GameObject MagazineAndFill;
    private int maxAmmo;

    void Start()
    {
        // fill.fillAmount = 1;
    }

    public void SetAmmo(int ammoCount, int MaxAmmo)
    {
        maxAmmo = MaxAmmo;
        fill.fillAmount = (float) ammoCount / MaxAmmo;
    }

    public void SetReserve(int reserveAmmo)
    {
        clearReserve();

        int magCount = reserveAmmo / maxAmmo;
        int partialMagFill = reserveAmmo % maxAmmo;

        for (int i=0; i < magCount; i++)
        {
            GameObject newMag = Instantiate(MagazineAndFill);
            newMag.transform.SetParent(reserveGrid.transform);
        }

        if (partialMagFill > 0)
        {
            GameObject partialMag = Instantiate(MagazineAndFill);
            partialMag.transform.SetParent(reserveGrid.transform);
            Image partialFill = partialMag.transform.Find("Fill").GetComponent<Image>();
            partialFill.fillAmount = (float) partialMagFill / maxAmmo;
            
        }

    }

    private void clearReserve()
    {
        foreach(Transform t in reserveGrid.transform)
        {
            Destroy(t.gameObject);
        }
    }
}
