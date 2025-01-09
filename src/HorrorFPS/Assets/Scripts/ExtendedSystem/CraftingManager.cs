using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// Manages all currently present crafting recipes and addition of any new ones at runtime
// Also (sort of) acts as a facade for crafting a given recipe with its AttemptToCraft method
public class CraftingManager : MonoBehaviour
{
    // Use Singleton Design pattern to allow global reference to a single list of crafting recipes
    public static CraftingManager instance { get; private set; }

    // List stores all current crafting recipes
    [SerializeField]
    private List<CraftingRecipeData> recipeDataList;

    // event for when the recipeDataList is changed
    public delegate void recipesChangedDelegate();
    public event recipesChangedDelegate OnRecipesChanged;

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


    // only instantiate CraftingRecipe object when attempting to craft - then call its craft method
    public void AttemptToCraft(CraftingRecipeData recipeData)
    {
        CraftingRecipe recipe = new CraftingRecipe(recipeData);
        recipe.Craft();
    }

    // Can add a new crafting recipe during runtime
    // Notifies observers of change to the crafting recipes 
    public void AddNewRecipe(CraftingRecipeData craftingRecipeData)
    {
        recipeDataList.Add(craftingRecipeData);
        OnRecipesChanged?.Invoke();
    }

    public List<CraftingRecipeData> GetRecipes()
    {
        return recipeDataList;
    }

}

