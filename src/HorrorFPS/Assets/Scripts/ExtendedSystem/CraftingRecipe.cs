using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Crafting Recipe

// Crafting Recipe includes a list of ingredients (with amounts) and the resultant craftable item
// Also provides a small use of the facade design pattern by taking responsibility of crafting (and checking)
[System.Serializable]
public class CraftingRecipe
{
    public CraftingRecipeData RecipeData;

    public CraftingRecipe(CraftingRecipeData recipeData)
    {
        RecipeData = recipeData;
    }

    public bool CheckForIngredients()
    {
        return InventoryManager.instance.HasIngredients(RecipeData.Ingredients);
    }

    // run InventoryManager pre-check to check if their will be space for an item after crafting
    public bool CheckForInventorySpace()
    {
        return InventoryManager.instance.CanCraftedItemFitPreCheck(RecipeData.CraftedItemData, RecipeData.Ingredients);
    }

    // Remove ingredients and add crafted item provided their is space and enough materials
    public void Craft()
    {
        if (!CheckForIngredients())
        {
            InventoryManager.instance.RequestAlert("Insufficient materials to craft!");
            return;
        }

        if (!CheckForInventorySpace())
        {
            Debug.Log("NOT ENOUGH SPACE");
            InventoryManager.instance.RequestAlert("Not enough space in inventory!");
            return;
        }

        foreach (RecipeIngredient ingredient in RecipeData.Ingredients)
        {
            InventoryManager.instance.RemoveItemByData(ingredient.ItemData, ingredient.RequiredAmount);
        }

        Item craftedItem = ItemFactoryManager.CreateItem(RecipeData.CraftedItemData);
        InventoryManager.instance.AddItem(craftedItem);
    }
}
#endregion

#region Crafting Recipe Ingredient

// A CraftingRecipe RecipeIngredient holds the itemData for the item, and the amount needed
[System.Serializable]
public class RecipeIngredient
{
    public ItemData ItemData;
    public int RequiredAmount;

    public RecipeIngredient(ItemData itemData, int requiredAmount)
    {
        ItemData = itemData;
        RequiredAmount = requiredAmount;
    }
}


#endregion
