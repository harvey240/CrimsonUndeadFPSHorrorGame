using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : Interactable
{
    public PlayerTest playerTest;
    public bool permanent;
    public AudioClip SFX;
    [SerializeField]
    private int healAmount = 80;

    protected override void Interact()
    {
        if (playerTest.currentHealth != playerTest.maxHealth)
        {
             TutorialManager.instance.TaskCompleted(3);

            playerTest.Heal(healAmount);
            SoundFXManager.instance.PlaySoundFXClip(SFX, transform, 1);
            Debug.Log("Interacted with " + gameObject.name);

            if (!permanent)
            {
                gameObject.SetActive(false);
            }
        }

        else
        {
            StartCoroutine(HealthFullDebugMessage());
        }

    }

    IEnumerator HealthFullDebugMessage()
    {
        String defaultPromptMessage = promptMessage;
        promptMessage = "Health Already Full";
        yield return new WaitForSeconds(0.8f);
        promptMessage = defaultPromptMessage;
    }
}
