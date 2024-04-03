using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class OpenDoor : Interactable
{
    public PlayerTest playerTest;
    public Animation doorOpen;

    [SerializeField]
    private AudioSource doorOpenSound;
    [SerializeField]
    private AudioSource doorCloseSound;
    [SerializeField]
    private AudioSource tryOpenDoorSound;

    public bool locked;
    public bool openOnStart;
    public bool isOpen = false;
    private bool isChangingState = false;
    private bool initialOpen = false;


    float _closedRotation;
    float _openRotation;

    void Awake()
    {
        _closedRotation = transform.rotation.eulerAngles.y;
        _openRotation = _closedRotation - 90;

    }


    void Update()
    {
        if (openOnStart && !initialOpen)
        {
            initialOpen = true;
            Interact();
        }
    }
    
    protected override void Interact()
    {
        if (!isChangingState)
        {
            isChangingState = true;
            if ((!locked) && !isOpen)
            {
                doorOpen.Play("DoorOpenIn");
                isOpen = true;
                StartCoroutine(ChangeStateAfterDelay());
                print("trying to Open");
                doorOpenSound.PlayOneShot(doorOpenSound.clip);
                promptMessage = "Close Door";
            }
            else if (isOpen){
                doorOpen.Play("DoorCloseIn");
                isOpen = false;
                StartCoroutine(ChangeStateAfterDelay());
                print("trying to close");
                doorCloseSound.PlayOneShot(doorCloseSound.clip);
                promptMessage = "Open Door";
            }

            else
            {
                isChangingState = false;
            }

        }

        if (locked)
        {
            doorOpen.Play("doorShake");
            tryOpenDoorSound.PlayOneShot(tryOpenDoorSound.clip);
        }       
    }

    // void OnTriggerStay()
    // {
    //     if (Input.GetKey(KeyCode.E) && !isChangingState)
    //     {
    //         isChangingState = true;
    //         if ((!locked) && !isOpen)
    //         {
    //             doorOpen.Play("DoorOpenIn");
    //             isOpen = true;
    //             StartCoroutine(ChangeStateAfterDelay());
    //             print("trying to Open");
                
    //         }
    //         else if (isOpen){
    //             doorOpen.Play("DoorCloseIn");
    //             isOpen = false;
    //             StartCoroutine(ChangeStateAfterDelay());
    //             print("trying to close");
    //         }
    //         else
    //         {
    //             isChangingState = false;
    //         }

    //     }
    // }

    IEnumerator ChangeStateAfterDelay()
    {
        yield return new WaitForSeconds(doorOpen["DoorOpenIn"].length); // Change to the appropriate animation length
        isChangingState = false;
    }
}
