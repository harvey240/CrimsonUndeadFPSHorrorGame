using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoNONE : MonoBehaviour, IAmmoUpdater
{
    public void SetAmmo(int ammoCount, int MaxAmmo)
    {
        // No HUD so do nothing
    }

    public void SetReserve(int reserveAmmo)
    {
        
    }
}
