using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gunHeat : MonoBehaviour, IAmmoUpdater
{
    public Renderer slideRenderer;
    private Material originalMaterial;

    public Renderer reserveLED;
    private Material LEDMaterial;
    public Gradient LEDgradient;
    private int maxAmmo;
    // Start is called before the first frame update
    void Awake()
    {
        originalMaterial = slideRenderer.material;
        LEDMaterial = reserveLED.material;
    }

    public void SetAmmo(int ammo, int MaxAmmo)
    {
        maxAmmo = MaxAmmo;
        if (ammo == MaxAmmo)
        {
            slideRenderer.material = originalMaterial;
        }

        else
        {
            float normalizedAmmo = (float)ammo / MaxAmmo;
            Color newColor = new Color(1f, 0.5f*normalizedAmmo, 0f);

            Material updatedMaterial = new Material(originalMaterial);
            updatedMaterial.color = newColor;

            slideRenderer.material = updatedMaterial;
        }

    }

    public void SetReserve(int reserveAmmo)
    {
        //TODO remove hardcoding of max reserve
        float normalizedReserve = (float) reserveAmmo / 60;
        Material updatedMaterial = new Material(LEDMaterial);
        updatedMaterial.SetColor("_EmissionColor", LEDgradient.Evaluate(normalizedReserve));
        reserveLED.material = updatedMaterial;
    }
}
