using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Manages the crafting section of the user interface
public class CraftingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject recipeSlotPrefab;
    [SerializeField]
    private Transform recipeSlotsParent;
    [SerializeField]
    private GameObject ingredientPrefab;

    private List<GameObject> recipeSlots = new List<GameObject>();

    void Awake()
    {
        // means UI will be updated whenever craftingRecipes are changed
        CraftingManager.instance.OnRecipesChanged += UpdateCraftingUI;
    }

    void Start()
    {
        UpdateCraftingUI();
    }

    void OnDestroy()
    {
        CraftingManager.instance.OnRecipesChanged -= UpdateCraftingUI;
    }

    // Update UI to display all current crafting recipes, including their required items (and amounts), and the item to be crafted
    // will scale well with any number of crafting recipes
    public void UpdateCraftingUI()
    {
        // Clear all recipe slots first
        foreach (GameObject recipeSlot in recipeSlots)
        {
            Destroy(recipeSlot);
        }
        recipeSlots.Clear();

        List<CraftingRecipeData> currentRecipes = CraftingManager.instance.GetRecipes();

        // instantiate ingredients
        foreach (CraftingRecipeData recipeData in currentRecipes)
        {
            GameObject recipeSlot = Instantiate(recipeSlotPrefab, recipeSlotsParent);
            recipeSlots.Add(recipeSlot);

            Transform ingredientsParent = recipeSlot.transform.Find("Ingredients");
            Transform craftedItemSlot = recipeSlot.transform.Find("CraftedItem");

            // fill out ingredients
            List<RecipeIngredient> ingredientsList = recipeData.Ingredients;
            foreach (RecipeIngredient ingredient in ingredientsList)
            {
                GameObject ingredientSlot = Instantiate(ingredientPrefab, ingredientsParent);
                ingredientSlot.transform.GetChild(0).GetComponent<Image>().sprite = ingredient.ItemData.Image;

                EventTrigger ingredientEventTrigger = ingredientSlot.GetComponent<EventTrigger>();
                UIHelper.AddHoverEvent(ingredientEventTrigger, ingredient.ItemData.Description);

                // Manage item amount indicator
                TextMeshProUGUI amountText = ingredientSlot.GetComponentInChildren<TextMeshProUGUI>();
                if (ingredient.RequiredAmount > 1)
                {
                    amountText.enabled = true;
                    amountText.text = ingredient.RequiredAmount.ToString();
                }
                else
                {
                    amountText.enabled = false;
                }
            }

            // set up the craftable item
            ItemData craftableItemData = recipeData.CraftedItemData;
            Image craftableItemImage = craftedItemSlot.GetComponent<Image>();
            craftableItemImage.sprite = craftableItemData.Image;

            // Mouse Hover events   
            EventTrigger craftableItemEventTrigger = craftedItemSlot.GetComponent<EventTrigger>();
            UIHelper.AddHoverEvent(craftableItemEventTrigger, craftableItemData.Description);

            // Try craft the item when it is clicked
            recipeSlot.GetComponentInChildren<Button>().onClick.AddListener(() => CraftingManager.instance.AttemptToCraft(recipeData));
        }
    }
}
