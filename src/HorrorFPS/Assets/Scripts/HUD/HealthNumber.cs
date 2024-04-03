using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthNumber : MonoBehaviour, IHealthUpdater
{

    public GameObject maxHealthNumber;
    public GameObject currentHealthNumber;

    private TextMeshProUGUI maxHealthText;
    private TextMeshProUGUI currentHealthText;

    void Awake()
    {
        maxHealthText = maxHealthNumber.GetComponent<TextMeshProUGUI>();
        currentHealthText = currentHealthNumber.GetComponent<TextMeshProUGUI>();
    }

    public void SetHealth(int maxHealth, int currentHealth)
    {
        maxHealthText.text = "/" + maxHealth.ToString();
        currentHealthText.text = currentHealth.ToString();
    }

}
