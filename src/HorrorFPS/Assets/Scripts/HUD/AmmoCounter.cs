using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour, IAmmoUpdater
{
    public GameObject ammoCount;
    public GameObject reserveCount;

    private TextMeshProUGUI ammoCountText;
    private TextMeshProUGUI reserveCountText;

    void Awake()
    {
        ammoCountText = ammoCount.GetComponent<TextMeshProUGUI>();
        reserveCountText = reserveCount.GetComponent<TextMeshProUGUI>();
    }

    public void SetAmmo(int ammo, int maxAmmo)
    {
        ammoCountText.text = ammo.ToString() + "/" + maxAmmo.ToString();
    }

    public void SetReserve(int reserveAmmo)
    {
        reserveCountText.text = reserveAmmo.ToString();
    }

}
