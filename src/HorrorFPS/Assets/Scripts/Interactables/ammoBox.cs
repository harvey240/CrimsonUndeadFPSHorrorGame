using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoBox : Interactable
{
    public gun gun;
    public bool permanent;
    public AudioClip SFX;
    [SerializeField]
    private int ammoAmount = 20;

    protected override void Interact()
    {
        if (gun.ammoReserve != gun.maxAmmoReserve)
        {
            TutorialManager.instance.TaskCompleted(0);
            gun.addAmmo(ammoAmount);
            SoundFXManager.instance.PlaySoundFXClip(SFX, transform, 1);

            if (!permanent)
            {
                gameObject.SetActive(false);
            }
        }

        else
        {
            StartCoroutine(AmmoFullDebugMessage());
        }
    }

    IEnumerator AmmoFullDebugMessage()
    {
        String defaultPromptMessage = promptMessage;
        promptMessage = "Ammo Already Full";
        yield return new WaitForSeconds(0.8f);
        promptMessage = defaultPromptMessage;
    }
}
