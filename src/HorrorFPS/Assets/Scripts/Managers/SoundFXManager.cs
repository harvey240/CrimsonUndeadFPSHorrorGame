using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    [SerializeField] private AudioSource soundFXObject;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Assign audio clip
        audioSource.clip = audioClip;

        // Assign Volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // Get length of sound FX clip
        float clipLength = audioSource.clip.length;

        // Destroy clip after playing
        Destroy(audioSource.gameObject, clipLength);
    }

     public void PlayRandomSoundFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume, float pitchMultiplier=1f)
    {
        // Assign random index
        int rand = Random.Range(0, audioClips.Length);

        // spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Assign audio clip
        audioSource.clip = audioClips[rand];

        // Modify pitch (defaults to the original pitch)
        audioSource.pitch = audioSource.pitch * pitchMultiplier;

        // Assign Volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // Get length of sound FX clip
        float clipLength = audioSource.clip.length;

        // Destroy clip after playing
        Destroy(audioSource.gameObject, clipLength);
    }
}
