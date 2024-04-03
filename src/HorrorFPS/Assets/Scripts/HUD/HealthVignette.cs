using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthVignette : MonoBehaviour, IHealthUpdater
{
    public Image vignette;

    public void SetHealth(int maxHealth, int health)
    {
        //normalise health value appropriately and subtract from 1 so alpha increases as health decreases
        float alpha = 1f - ((float)health / maxHealth);

        // Clamp alpha value to keep it between [0,1]
        alpha = Mathf.Clamp01(alpha);

        Color vignetteColor = vignette.color;
        vignetteColor.a = alpha;
        vignette.color = vignetteColor;
    }
}
