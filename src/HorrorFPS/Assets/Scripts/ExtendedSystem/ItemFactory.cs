using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Make use of factory method pattern to better adhere to open/closed principle
// Allows for adding new item types while only having to extend the itemFactories Dictionary in existing code
// And of course helps with creation of different items without needing to specify exact class
// NOTE: these ItemFactory classes also decide the default UseStrategy based on the type of item being created
public interface ItemFactory
{
    Item CreateItem(ItemData itemData);
}


#region Item Factory Method(s)
public class HealthItemFactory : ItemFactory
{
    public Item CreateItem(ItemData itemData)
    {
        IUseStrategy useStrategy = new InstantHealStrategy(itemData.healAmount);
        return new HealthItem(itemData.Name, itemData.Description, useStrategy, itemData.Image, itemData.healAmount);
    }
}

public class CraftingMaterialFactory : ItemFactory
{
    public Item CreateItem(ItemData itemData)
    {
        IUseStrategy useStrategy = new EmptyStrategy();
        return new CraftingMaterial(itemData.Name, itemData.Description, useStrategy, itemData.Image);
    }
}

public class GunFactory : ItemFactory
{
    public Item CreateItem(ItemData itemData)
    {
        IUseStrategy useStrategy = new EquipPrimaryWeaponStrategy();
        return new Gun(itemData.Name, itemData.Description, useStrategy, itemData.Image);
    }
}

#endregion
