using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] tracks;
    public AudioClip endMusic;
    public AudioMixer audioMixer;
    public static MusicManager instance;

    private AudioSource audioSource;
    private int trackIndex = 0;
    private bool levelComplete = false;
    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
        ShuffleTracks(tracks);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((!audioSource.isPlaying || Input.GetKeyDown(KeyCode.P)) && !levelComplete)
        {            
            audioSource.clip = GetNextTrack();
            audioSource.Play();
            
        }
    }

    private AudioClip GetNextTrack()
    {
        AudioClip nextTrack = tracks[trackIndex];
        if ((trackIndex + 1) > (tracks.Length-1))
        {
            trackIndex = 0;
        }
        else
        {
            trackIndex ++;
        }

        return nextTrack;

    }

    public void PlayEndMusic()
    {
        audioMixer.SetFloat("soundFXVolume", -80f);
        audioMixer.SetFloat("ambienceVolume", -80f);
        audioSource.clip = endMusic;
        audioSource.Play();
    }

    void ShuffleTracks(AudioClip[] tracks)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < tracks.Length; t++ )
        {
            AudioClip tmp = tracks[t];
            int r = Random.Range(t, tracks.Length);
            tracks[t] = tracks[r];
            tracks[r] = tmp;
        }
    }
}
