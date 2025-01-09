using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Helps centralise logic between the overall InventoryUI and CraftingUI classes
// Namely, adding events so item descriptions are displayed on the UI when the mouse hovers over them
public static class UIHelper
{
    private static GameObject descriptionUI;
    private static TextMeshProUGUI descriptionText;

    public static void SetDescriptionUI(GameObject descriptionUIPanel)
    {
        descriptionUI = descriptionUIPanel;
        descriptionText = descriptionUI.GetComponentInChildren<TextMeshProUGUI>();
    }

    // add hover event to a given eventTrigger of an item in the UI to display given descriptions
    public static void AddHoverEvent(EventTrigger eventTrigger, string description)
    {
        eventTrigger.triggers.Clear();

        EventTrigger.Entry mouseEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        mouseEnter.callback.AddListener(eventData => ShowDescription(description));
        eventTrigger.triggers.Add(mouseEnter);

        EventTrigger.Entry mouseExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        mouseExit.callback.AddListener(eventData => HideItemDescription());
        eventTrigger.triggers.Add(mouseExit);
    }


    private static void ShowDescription(string description)
    {
        descriptionText.text = description;
        descriptionUI.SetActive(true);
    }

    private static void HideItemDescription()
    {
        descriptionText.text = "";
        descriptionUI.SetActive(false);
    }
}
