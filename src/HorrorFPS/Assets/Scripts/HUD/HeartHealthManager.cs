using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HeartHealthManager : MonoBehaviour, IHealthUpdater
{
    public GameObject heartPrefab;
    List<HeartHealth> hearts = new List<HeartHealth>();


    public void SetHealth(int maxHealth, int currentHealth)
    {
        createHearts(maxHealth, currentHealth);
    }

    public void createHearts(int maxHealth, int currentHealth)
    {
        maxHealth /= 10;
        currentHealth /= 10;
        
        clearAllHearts();

        //find how many hearts to draw in total based on maxHealth
        float healthRemainder = maxHealth % 2;
        int heartsToCreate = (int) ((maxHealth/2) + healthRemainder);

        for (int i=0; i<heartsToCreate; i++)
        {
            createEmptyHeart();
        }

        for (int i=0; i<hearts.Count; i++)
        {
            int heartStatusRemainder = (int) Mathf.Clamp(currentHealth - (i*2), 0, 2);
            hearts[i].setHeartImage((HeartStatus) heartStatusRemainder);
        }

    }

    public void createEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HeartHealth heartScript = newHeart.GetComponent<HeartHealth>();
        heartScript.setHeartImage(HeartStatus.Empty);
        hearts.Add(heartScript);
    }

    public void clearAllHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        hearts = new List<HeartHealth>();
    }
}
