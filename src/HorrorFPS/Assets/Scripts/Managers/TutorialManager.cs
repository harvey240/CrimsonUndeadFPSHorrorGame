using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [System.Serializable]
    public class TutorialTask
    {
        public string description;
        public bool completed;
    }

    public List<TutorialTask> tutorialTasks = new List<TutorialTask>();
    private int currentTaskIndex = 0;
    private bool playerDamaged = false;

    public bool isActive = true;
    public TextMeshProUGUI promptText;
    public PlayerTest playerTest;
    public gun gun;
    public OpenDoor openDoor;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (isActive)    
        {
            gun.ammoCount = 0;
            gun.ammoReserve = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isActive)
        {
            openDoor.locked = true;
            UpdateTutorialText(GetCurrentTaskDescription());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Keypad1))
        // {
        //     TaskCompleted(0);
        // }

        // if (Input.GetKeyDown(KeyCode.Keypad2))
        // {
        //     TaskCompleted(1);
        // }

        if (isActive)
        {
            if (currentTaskIndex == 3 && !playerDamaged)
            {
                playerDamaged = true;
                playerTest.TakeDamage(80);
            }
        }
        
    }

    string GetCurrentTaskDescription()
    {
        if (currentTaskIndex < tutorialTasks.Count)
        {
            return tutorialTasks[currentTaskIndex].description;
        }
        else
        {
            EnemyManager.instance.ActivateEnemies();
            openDoor.locked = false;
            openDoor.BaseInteract();
            //ACTIVATE TIMER
            GameManager.instance.timerStarted = true;
            return null;
        }
    }

    public void TaskCompleted(int taskIndex)
    {
        if (taskIndex == currentTaskIndex)
        {
            tutorialTasks[currentTaskIndex].completed = true;

            currentTaskIndex++;

            UpdateTutorialText(GetCurrentTaskDescription());
        }
    }

    void UpdateTutorialText(string newText)
    {
        if (newText != null)
        {
            promptText.text = newText;
        }

        else
        {
            StartCoroutine(removeTutorialText());
            // promptText.text = "Now explore the building and try to eliminate as many zombies as you can find, but watch out for traps and use medkits and ammo boxes if you need.";
            // Invoke("removeTutorialText", 4f);
            // promptText.text = "Remember you can hold SHIFT to sprint";
            // Invoke("removeTutorialText", 2f);
        }
    }

    IEnumerator removeTutorialText()
    {
        promptText.text = "Now explore the building and try to eliminate as many zombies as you can find, but watch out for traps and use medkits and ammo boxes if you need.";
        yield return new WaitForSeconds(4f);
        promptText.text = "Remember you can hold SHIFT to sprint";
        yield return new WaitForSeconds(2f);
        promptText.text = "";
    }
}
