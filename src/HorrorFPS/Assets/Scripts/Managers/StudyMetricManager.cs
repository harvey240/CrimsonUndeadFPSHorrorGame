using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StudyMetricManager : MonoBehaviour
{
    public int deathCounter;
    public int killCounter;
    public int reloadCounter;
    public int healCounter;
    public List<float> remainingAmmoPercentages = new();
    public List<float> remainingHealthPercentages = new();
    public List<float> wastedHealthValues = new();

    private float averageAmmoWastage;
    private float averageHealthWastage;
    private float averageHealthWastageValue;

    public static StudyMetricManager instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        deathCounter = 0;
        killCounter = 0;
        reloadCounter = 0;
        healCounter = 0;
        // CreateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalulateAverageMetrics()
    {
        float ammoSum = 0;
        float healthSum = 0;
        float healthValueSum = 0;

        foreach(float x in remainingAmmoPercentages)
        {
            ammoSum += x;
        }

        foreach(float x in remainingHealthPercentages)
        {
            healthSum += x;
        }

        foreach(float x in wastedHealthValues)
        {
            healthValueSum += x;
        }

        averageAmmoWastage = ammoSum / remainingAmmoPercentages.Count;
        averageHealthWastage = healthSum / remainingHealthPercentages.Count;
        averageHealthWastageValue = healthValueSum / wastedHealthValues.Count;

    }

    public void CreateText()
    {
        CalulateAverageMetrics();

        string metricsText = $"Study Log\n\nDeath Count: {deathCounter} \n\nKill Count: {killCounter} \n\nReload Count: {reloadCounter} \n\nHeal Count: {healCounter} \n\n\nAverage Remaining Ammunition Percentage When Reloading: {averageAmmoWastage} \n\nAverage Remaining Health Percentage When Healing: {averageHealthWastage} \n\nAverage Amount of Extra Health Wasted when Healing: {averageHealthWastageValue}";

        string path = Application.dataPath + "/Log.txt";

        if(!File.Exists(path))
        {
            File.WriteAllText(path,"");
        }

        File.WriteAllText(path, metricsText);
    }

}
