using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [System.Serializable]
    public class SpawnPointSet
    {
        public List<Transform> spawnPoints;
    }

    [SerializeField] public List<SpawnPointSet> spawnPointSets = new List<SpawnPointSet>();
    [SerializeField] private GameObject[] enemies;
    private GameObject[] currentEnemies;
    private UnityEngine.Object spawnFXRef;
    [SerializeField] private bool notInitalWave = false;
    public int enemyCounter;

    void Awake()
    {
        // enemyCounter = enemies.Length;
        enemyCounter = 10;

        instance = this;
        SpawnEnemies();
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnFXRef = Resources.Load("SpawnParticle");
        // SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyCounter == 0)
        {
            enemyCounter = 10;

            Debug.Log("All enemies cleared");
            GameManager.instance.RespawnPickUps();
            StartCoroutine(GameManager.instance.EnemyRespawnMessage());
            Invoke("SpawnEnemies", 5f);
            Invoke("ActivateEnemies", 5.5f);


            
        }
        
    }

    void SpawnEnemies()
    {

        if (spawnPointSets.Count == 0)
        {
            Debug.LogWarning("No spawn point sets for enemies available!");
            return;
        }

        int randomIndex = Random.Range(0, spawnPointSets.Count);
        SpawnPointSet selectedSpawnPoints = spawnPointSets[randomIndex];

        for (int i=0; i < selectedSpawnPoints.spawnPoints.Count; i++)
        {
            Debug.Log("Trying to spawn enemy " + i + "at " + selectedSpawnPoints.spawnPoints[i].position);
            
            // if (!enemies[i].activeInHierarchy)
            // {   
            //     enemies[i].SetActive(true);
            // }

            // enemies[i].transform.position = selectedSpawnPoints.spawnPoints[i].position;
            // enemies[i].transform.rotation = selectedSpawnPoints.spawnPoints[i].rotation;
            int randomEnemyIndex = Random.Range(0, enemies.Length);
            if (notInitalWave)
            {
                Instantiate(spawnFXRef, selectedSpawnPoints.spawnPoints[i].position, selectedSpawnPoints.spawnPoints[i].rotation);
            }
            GameObject currentEnemy = Instantiate(enemies[randomEnemyIndex], selectedSpawnPoints.spawnPoints[i].position, selectedSpawnPoints.spawnPoints[i].rotation);
    
            // if (notInitalWave)
            // {
            //     enemies[i].GetComponent<Enemy>().Respawn();
            // }

            currentEnemy.GetComponent<NavMeshAgent>().enabled = false;
            currentEnemy.GetComponent<EnemyController>().enabled = false;
 
        }

        notInitalWave = true;
        

    }

    public void ActivateEnemies()
    {
        currentEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        // enemyCounter = currentEnemies.Length;
        // enemyCounter = 10;

        for (int i=0; i<currentEnemies.Length; i++)
        {
            currentEnemies[i].GetComponent<NavMeshAgent>().enabled = true;
            currentEnemies[i].GetComponent<EnemyController>().enabled = true;
        }
    }


    public void enemyKilled()
    {
        enemyCounter -= 1;
    }
}
