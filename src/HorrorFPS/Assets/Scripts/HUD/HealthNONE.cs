using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthNONE : MonoBehaviour, IHealthUpdater
{
    public void SetHealth(int maxHealth, int currentHealth)
    {
        // NO HUD so do nothing
    }

    public void SetMaxHealth(int Health)
    {
        
    }
}
