using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    public AudioSource footstep;
    public AudioSource thud;
    [SerializeField] private AudioClip[] footstepClips;
    public FPSController fpsController;
    private float defaultPitch;
    // Start is called before the first frame update
    void Start()
    {
        defaultPitch = footstep.pitch;   
    }

    // Update is called once per frame
    void Update()
    {
        footstep.pitch = fpsController.isRunning ? defaultPitch*1.3f : defaultPitch;
        // float footPitch = fpsController.isRunning ? 1.3f : 1f;
        if (fpsController.characterController.isGrounded)
        {
            if (fpsController.isJumping)
            {
                thud.PlayOneShot(thud.clip);
                fpsController.isJumping = false;
            }
        }
        if(fpsController.isMoving && fpsController.characterController.isGrounded)
        {
            if(!footstep.isPlaying) // Check if the sound is not already playing
            {
                // int rand = Random.Range(0, footstepClips.Length);

                // footstep.clip = footstepClips[rand];
                footstep.Play(); // Play the footstep sound
            }

            // SoundFXManager.instance.PlayRandomSoundFXClip(footstepClips, transform, 1f, footPitch);
            
        }
        else
        {
            footstep.Stop(); // Stop the footstep sound
        }
    }
}
