using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitFlash : MonoBehaviour
{
    public Image flashImage;
    public float flashDuration = 0.5f;
    private Color originalColor;

    void Start()
    {
        flashImage.enabled = false;
    }

    public void DisplayHitFlash()
    {
        StartCoroutine(ShowFlashImage());
    }

    IEnumerator ShowFlashImage()
    {
        flashImage.enabled = true;
        yield return new WaitForSeconds(flashDuration);
        flashImage.enabled = false;
    }
}
