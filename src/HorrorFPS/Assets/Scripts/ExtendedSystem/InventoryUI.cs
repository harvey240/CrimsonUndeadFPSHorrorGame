using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


// Manages the Majority of the user interface, primarily the Inventory grid
public class InventoryUI : MonoBehaviour
{
    // Overall UI parent gamobject
    [SerializeField]
    private GameObject UIParent;

    // Inventory item slots UI elements
    [SerializeField]
    private GameObject itemSlotPrefab;
    [SerializeField]
    private GameObject itemSlotsParent;
    private List<GameObject> itemSlots = new List<GameObject>();

    // UI elements for displaying hover-over item descriptions
    [SerializeField]
    private GameObject descriptionUI;

    // UI elements for displaying alerts
    [SerializeField]
    private GameObject alertUI;
    private TextMeshProUGUI alertText;

    // track if the UI is open or not
    private bool inventoryIsOpen = false;

    // current active alert coroutine running
    private Coroutine activeAlertCoroutine;

    // Default image if item has none
    public Sprite defaultSprite;

    void Awake()
    {
        InventoryManager.instance.OnInventoryChanged += UpdateInventoryUI;
        InventoryManager.instance.OnInventorySizeChanged += UpdateInventorySize;
        InventoryManager.instance.OnAlertRequested += ShowUIAlert;
    }

    // Start is called before the first frame update
    void Start()
    {
        // initialise the description UI elements for the UI helper 
        UIHelper.SetDescriptionUI(descriptionUI);

        UpdateInventorySize(InventoryManager.instance.GetInventorySize());

        alertText = alertUI.GetComponentInChildren<TextMeshProUGUI>();

        UpdateInventoryUI();
    }

    void OnDestroy()
    {
        InventoryManager.instance.OnInventoryChanged -= UpdateInventoryUI;
        InventoryManager.instance.OnInventorySizeChanged -= UpdateInventorySize;
        InventoryManager.instance.OnAlertRequested -= ShowUIAlert;
    }
    
    #region Update Inventory
    // Updates the inventory UI to display all currently held items in the inventory, as well as their amounts, images, etc.
    // Allows user to click to try use an item, and hover mouse over for a description
    public void UpdateInventoryUI()
    {
        List<Item> currentInventoryList = InventoryManager.instance.GetItems();

        // set up all the present item slots
        for (int i=0; i < itemSlots.Count; i++)
        {
            if (i < currentInventoryList.Count)
            {
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = currentInventoryList[i].Image != null ? currentInventoryList[i].Image : defaultSprite;

                // Amount Number text should appear when item amount is > 1
                if (currentInventoryList[i].Amount > 1)
                {
                    TextMeshProUGUI amountText = itemSlots[i].GetComponentInChildren<TextMeshProUGUI>();
                    amountText.enabled = true;
                    amountText.text = currentInventoryList[i].Amount.ToString();
                }
                else
                {
                    itemSlots[i].GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                }

                // Add Appropriate OnClick Use() function for the current item
                itemSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
                itemSlots[i].GetComponent<Button>().onClick.AddListener(currentInventoryList[i].Use);

                // Set up hover events to display item description(s)
                int curIndex = i;
                EventTrigger eventTrigger = itemSlots[curIndex].GetComponent<EventTrigger>();
                UIHelper.AddHoverEvent(eventTrigger, InventoryManager.instance.GetItems()[curIndex].Description);
                
            }

            else
            {
                // Clear all buttons, images, and triggers as the slot is empty
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                itemSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                itemSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
                itemSlots[i].GetComponent<EventTrigger>().triggers.Clear();
                itemSlots[i].GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }
        }
        
    }

    // used to dynamically update the inventory size by instantiating all required itemslots for the current inventory size
    public void UpdateInventorySize(int size)
    {
        foreach (GameObject itemSlot in itemSlots)
        {
            Destroy(itemSlot);
        }

        for (int i=0; i<size; i++)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, itemSlotsParent.transform);
            itemSlots.Add(itemSlot);
        }
    }
    
    #endregion

    #region UI Alert

    //Observes OnAlertRequested to display relevant message on the UI
    public void ShowUIAlert(string alertMessage)
    {
        if (activeAlertCoroutine != null)
        {
            StopCoroutine(activeAlertCoroutine);
        }

        activeAlertCoroutine = StartCoroutine(UIAlert(alertMessage));
    }

    // Coroutine to display UI alert message for 2 seconds
    private IEnumerator UIAlert(string alertMessage)
    {
        alertText.text = alertMessage;
        alertUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        alertUI.SetActive(false);

        activeAlertCoroutine = null;
    }
    #endregion
    
    #region Toggle UI
    // Opens/Closes the inventory and crafting menu
    private void ToggleInventoryUI()
    {
        inventoryIsOpen = !inventoryIsOpen;
        UIParent.SetActive(inventoryIsOpen);

        // Bool determines if player can move etc.
        PlayerManager.instance.InventoryOpen = inventoryIsOpen;

        if (inventoryIsOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
    #endregion
}
