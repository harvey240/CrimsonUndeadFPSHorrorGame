using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour, IHealthUpdater
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    // public bool useGradient;

    public void SetHealth(int maxHealth, int currentHealth)
    {
        SetHealth(currentHealth);
    }

    public void SetHealth(int Health){
        slider.value = Health;
        // fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxHealth(int Health){
        slider.maxValue = Health;
        slider.value = Health;

        // fill.color = gradient.Evaluate(1f);
    }
}
