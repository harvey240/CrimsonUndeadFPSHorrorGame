using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable object to store relevant data for different crafting recipes
// acts as a blueprint for crafting recipes
[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/CraftingRecipeData")]
public class CraftingRecipeData : ScriptableObject
{
    public string RecipeName;
    public List<RecipeIngredient> Ingredients;
    public ItemData CraftedItemData;
}
