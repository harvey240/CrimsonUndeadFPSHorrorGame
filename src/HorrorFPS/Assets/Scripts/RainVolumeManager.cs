using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainVolumeManager : MonoBehaviour
{
    public AudioSource rainSound;
    public float outsideVolume = 1f;
    public float insideVolume = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rainSound.volume = outsideVolume;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rainSound.volume = insideVolume;
        }
    }
}
