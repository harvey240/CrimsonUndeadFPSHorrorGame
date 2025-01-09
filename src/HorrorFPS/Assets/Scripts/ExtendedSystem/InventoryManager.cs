using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Use Singleton design pattern to allow global reference to singular inventory
    public static InventoryManager instance { get; private set; }

    // A list of all items in the players inventory
    private List<Item> inventoryItemList = new List<Item>();

    [SerializeField]
    private List<ItemData> initialItems;

    // the amount of different items a player can hold
    [SerializeField]
    private int inventorySize = 8;

    // OBSERVER PATTERN to help UI update when changes happen in inventory
    // Events for inventory changes and alerts - observers can subscribe to these (InventoryUI)
    public delegate void inventoryChangedDelegate();
    public event inventoryChangedDelegate OnInventoryChanged;

    public delegate void inventorySizeChangedDelegate(int size);
    public event inventorySizeChangedDelegate OnInventorySizeChanged;

    public delegate void alertEvent(string alertMessage);
    public event alertEvent OnAlertRequested;


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

    void Start()
    {
        // Add any pre-existing items, useful if loading a pre-defined inventory at start of a level for example
        AddInitialItems();
    }

    #region Send Alert
    // Notifies observers of a relevant inventory alertMessage to be displayed
    public void RequestAlert(string alertMessage)
    {
        OnAlertRequested?.Invoke(alertMessage);
    }
    #endregion

    #region Getters, Setters etc.
    public List<Item> GetItems()
    {
        return inventoryItemList;
    }

    public int GetInventorySize()
    {
        return inventorySize;
    }

    public void SetInventorySize(int newSize)
    {
        inventorySize = newSize;
        OnInventorySizeChanged?.Invoke(newSize);
    }
    #endregion


    #region Inventory Operations


    private void AddInitialItems()
    {
        if (initialItems.Count > inventorySize)
        {
            throw new Exception("Cannot add more initial items than the size of the inventory");
        }

        foreach (ItemData itemData in initialItems)
        {
            Item itemToAdd = ItemFactoryManager.CreateItem(itemData);
            AddItem(itemToAdd);
        }
    }

    // Attempts to add an item, returns bool to indicate success/failure
    public bool AddItem(Item item)
    {
        Item itemInInventory = inventoryItemList.Find(itemToFind => itemToFind.Name == item.Name);

        // increase amount by 1 if item already present and stackable
        if (itemInInventory != null && item.Stackable)
        {
            itemInInventory.Amount += item.Amount;
        }

        // if no item slot is free return false as inventory is full
        else if (inventoryItemList.Count >= inventorySize)
        {
            return false;
        }

        // otherwise add the new item and return true;
        else
        {
            inventoryItemList.Add(item);
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    // Remove a given item, notify inventory changed
    public void RemoveItem(Item item)
    {
        Item itemInInventory = inventoryItemList.Find(itemToFind => itemToFind.Name == item.Name);
        if (itemInInventory != null)
        {
            itemInInventory.Amount -= 1;

            if (itemInInventory.Amount == 0)
            {
                inventoryItemList.Remove(itemInInventory);
            }

            OnInventoryChanged?.Invoke();
        }

    }

    // Remove an item by a given amount given it's ItemData representation, notify inventory changed
    // Used when crafting
    public void RemoveItemByData(ItemData itemData, int requiredAmount = 1)
    {
        Item itemInInventory = inventoryItemList.Find(itemToFind => itemToFind.Name == itemData.Name);

        if (itemInInventory != null && itemInInventory.Amount >= requiredAmount)
        {
            itemInInventory.Amount -= requiredAmount;
            if (itemInInventory.Amount == 0)
            {
                inventoryItemList.Remove(itemInInventory);
            }

            OnInventoryChanged?.Invoke();
        }
    }

    #endregion


    #region Inventory Checks
    // Use a simulated inventory to see if there will be space to add a crafted item after using its resources
    public bool CanCraftedItemFitPreCheck(ItemData craftedItemData, List<RecipeIngredient> ingredients)
    {
        List<Item> fakeInventory = new List<Item>(inventoryItemList);

        // Removed ingredients from simulated inventory
        foreach (RecipeIngredient ingredient in ingredients)
        {
            Item itemInInventory = fakeInventory.Find(itemToFind => itemToFind.Name == ingredient.ItemData.Name);

            if (itemInInventory.Amount - ingredient.RequiredAmount <= 0)
            {
                fakeInventory.Remove(itemInInventory);
            }
        }

        // if the item to be crafted exists in inventory and is stackable return true
        Item preExistingItem = fakeInventory.Find(itemToFind => itemToFind.Name == craftedItemData.Name);
        if (preExistingItem != null && preExistingItem.Stackable)
        {
            return true;
        }

        // otherwise return true if there is a free item slot
        else
        {
            return fakeInventory.Count < inventorySize;
        }
    }

    // Checks if inventory has a list of ingredients for crafting
    public bool HasIngredients(List<RecipeIngredient> ingredients)
    {
        foreach (RecipeIngredient ingredient in ingredients)
        {
            if (!HasIngredient(ingredient.ItemData, ingredient.RequiredAmount))
            {
                return false;
            }
        }
        return true;
    }

    // Checks if inventory has a given ingredient
    public bool HasIngredient(ItemData itemData, int requiredAmount)
    {
        // clear up logic here and all other places to eventually use itemData as the identifier (if it makes enough sense to)
        Item itemInInventory = inventoryItemList.Find(itemToFind => itemToFind.Name == itemData.Name);
        if (itemInInventory != null && itemInInventory.Amount >= requiredAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // Checks if the inventory contains an item
    public bool ContainsItem(Item item)
    {
        Item itemInInventory = inventoryItemList.Find(itemToFind => itemToFind.Name == item.Name);
        if (itemInInventory == null)
        {
            return false;
        }

        return true;
    }

    #endregion

    #region Debugging

    // Useful for debugging and viewing items
    public void PrintInventory()
    {
        foreach (Item item in inventoryItemList)
        {
            PrintItem(item);
        }
    }

    public void PrintItem(Item item)
    {
        string serializedText = "";
        serializedText = JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.Indented);
        Debug.Log(serializedText);
    }

    #endregion
}
