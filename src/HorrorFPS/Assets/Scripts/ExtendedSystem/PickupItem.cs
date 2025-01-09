using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// This script can be attached to gameObjects that represent items for the player to pick up in-game

// NOTE: Gamobjects with this attached need to be placed on the "Interactable" layer within the Unity Editor
// AND must have a collider (can be trigger)
public class PickupItem : Interactable
{
    // Set to the relevant itemData representation of the item so it can be instantiated fully when added to the inventory
    [SerializeField]
    private ItemData itemData;

    void Start()
    {
        promptMessage = "Pick Up " + itemData.Name;
    }

    // Here instantiate the itemData appropriately - makes use of the Factory Method Pattern
    protected override void Interact()
    {
        // add item to inventory if possible and destroy gameobject in world
        Item itemToAdd = ItemFactoryManager.CreateItem(itemData);
        
        if (InventoryManager.instance.AddItem(itemToAdd))
        {
            Destroy(gameObject);
        }

        else
        {
            StartCoroutine(InventoryFullMessage());
        }
    }

    IEnumerator InventoryFullMessage()
    {
        string defaultPromptMessage = promptMessage;
        promptMessage = "Inventory Full";
        yield return new WaitForSeconds(0.8f);
        promptMessage = defaultPromptMessage;
    }
}
