using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public PlayerTest playerTest;
    public AudioClip trapAttackSound;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("entering Trap");
            SoundFXManager.instance.PlaySoundFXClip(trapAttackSound, transform, 0.5f);
            playerTest.TakeDamage(10);
        }
    }
}
