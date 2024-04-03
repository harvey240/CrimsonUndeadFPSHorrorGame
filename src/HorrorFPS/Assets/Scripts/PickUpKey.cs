using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKey : Interactable
{
    // public Component doorCollider;
    public GameObject key;
    public PlayerTest playerTest;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
        playerTest.HasKey = true;
        gameObject.SetActive(false);
    }

    // void OnTriggerStay()
    // {
    //     if(Input.GetKey(KeyCode.E))
    //         {
    //             // doorCollider.GetComponent<BoxCollider>().enabled = true;
    //             playerTest.HasKey = true;
    //             key.SetActive(false);
    //         } 
    // }
}
