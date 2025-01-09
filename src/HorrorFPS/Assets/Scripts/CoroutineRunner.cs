using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple Monobehaviour class to handle running coroutines in non-monobehaviour classes
public class CoroutineRunner : MonoBehaviour
{
    public static CoroutineRunner instance { get; private set; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        else
        {
            instance = this;
        }
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
