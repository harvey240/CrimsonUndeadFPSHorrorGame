using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float restartDelay = 3f;
    public GameObject player;
    public PlayerTest playerTest;
    public float timeLimit = 240f;
    public GameObject levelCompleteScreen;
    public GameObject RespawnTextObject;
    public GameObject PickupsGO;
    public static GameManager instance;

    public bool timerStarted = false;
    private String baseRepsawnText = "You Have Died... Respawning in";
    private String baseEnemyRespawnText = "You have defeated all enemies... More will arrive in";

    void Awake()
    {
        instance = this;
        // playerTest = player.GetComponent<PlayerTest>();

    }

    void Start()
    {
        // ignore collision between players and enemies...
        // Physics.IgnoreLayerCollision(7,10);
    }

    void Update()
    {
        if (timerStarted)
        {
            timeLimit -= Time.deltaTime;

            
            if (timeLimit <=0f)
            {
                timerStarted = false;
                LevelComplete();
            }
        }

    }

    public void EndGame()
    {
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            Debug.Log("GAME OVER");
            StartCoroutine(RespawnMessage());
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        Debug.Log("in restart");

        playerTest.Respawn();
        gameHasEnded = false;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator EnemyRespawnMessage()
    {
        RespawnTextObject.SetActive(true);
        TextMeshProUGUI respawnText = RespawnTextObject.GetComponent<TextMeshProUGUI>();
        respawnText.text = baseEnemyRespawnText + " 5";

        yield return new WaitForSeconds(1f);
        respawnText.text = baseEnemyRespawnText + " 4";
        yield return new WaitForSeconds(1f);
        respawnText.text = baseEnemyRespawnText + " 3";
        yield return new WaitForSeconds(1f);
        respawnText.text = baseEnemyRespawnText + " 2";
        yield return new WaitForSeconds(1f);
        respawnText.text = baseEnemyRespawnText + " 1";                
        yield return new WaitForSeconds(1f);

        RespawnTextObject.SetActive(false);
    }

    IEnumerator RespawnMessage()
    {
        RespawnTextObject.SetActive(true);
        TextMeshProUGUI respawnText = RespawnTextObject.GetComponent<TextMeshProUGUI>();
        respawnText.text = baseRepsawnText + " 3";
        
        yield return new WaitForSeconds(1f);
        respawnText.text = baseRepsawnText + " 2";
        yield return new WaitForSeconds(1f);
        respawnText.text = baseRepsawnText + " 1";
        yield return new WaitForSeconds(1f);
        RespawnTextObject.SetActive(false);

    }

    void LevelComplete()
    {
        StudyMetricManager.instance.CreateText();
        MusicManager.instance.PlayEndMusic();
        player.GetComponent<PlayerTest>().isDead = true;
        levelCompleteScreen.SetActive(true);
        // Time.timeScale = 0f;
    }

    public void RespawnPickUps()
    {
        foreach(Transform child in PickupsGO.transform)
        {
            child.gameObject.SetActive(true);
        }
        //TODO implement this method
    }
}
