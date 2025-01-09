using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the factory method implementation and creation of items
// Uses the appropriate ItemFactory Class depending on the provided ItemData type
public class ItemFactoryManager
{
    // Dictionary maps ItemType to it's relevant ItemFactory class
    // EXTEND THIS when adding new item types
    private static readonly Dictionary<ItemType, ItemFactory> itemFactories = new Dictionary<ItemType, ItemFactory>
    {
        {ItemType.HEALTHITEM, new HealthItemFactory()},
        {ItemType.CRAFTINGMATERIAL, new CraftingMaterialFactory()},
        {ItemType.GUN, new GunFactory()}
    };

    // Used when item has a specified use strategy
    // EXTEND THIS when adding new use strategies that you want to items to be able to specify
    public static IUseStrategy GetUseStrategy(ItemData itemData)
    {
        switch (itemData.UseStrategy)
        {
            case UseType.INSTANTHEAL:
                return new InstantHealStrategy(itemData.healAmount);
            case UseType.GRADUALHEAL:
                return new GradualHealStrategy(itemData.healAmount);
            case UseType.EMPTY:
                return new EmptyStrategy();
            case UseType.EQUIPPRIMARY:
                return new EquipPrimaryWeaponStrategy();
        }
        return null;
    }

    // Use appropriate ItemFactory method to instantiate and return an item of the correct type
    public static Item CreateItem(ItemData itemData)
    {
        if (itemFactories.TryGetValue(itemData.itemType, out ItemFactory itemFactory))
        {
            Item createdItem = itemFactory.CreateItem(itemData);

            // if itemData specifies a UseStrategy type other than DEFAULT then manually set the UseStrategy of the createdItem at runtime
            if (itemData.UseStrategy != UseType.DEFAULT)
            {
                IUseStrategy specifiedUseStrategy = GetUseStrategy(itemData);
                createdItem.SetUseStrategy(specifiedUseStrategy);
            }

            return createdItem;
        }
        else
        {
            throw new System.NotImplementedException("Item type not recognised");
        }
    }
}
