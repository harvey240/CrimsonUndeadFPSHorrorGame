using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepSoundClips;
    [SerializeField] private GameObject rightHand;
    [HideInInspector] public bool attackHandActive = false;

    void Awake()
    {

    }

    public void playFootstep()
    {
        SoundFXManager.instance.PlayRandomSoundFXClip(footstepSoundClips, transform, 1f);

    }

    public void ActivateHandCollider()
    {
        attackHandActive = true;
        rightHand.GetComponent<Collider>().enabled = true;
    }

    public void DeactivateHandCollider()
    {
        attackHandActive = false;
        rightHand.GetComponent<Collider>().enabled = false;
    }
}
