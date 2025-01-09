using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

#region Base Class

// Base abstract class for all items - provides all core attributes of an item and a Use() method
// additionally allows the UseStrategy to be changed via the SetUseStrategy method
public abstract class Item
{
    public string Name;
    public int Amount;
    public string Description;
    public IUseStrategy UseStrategy;
    [JsonIgnore]
    public Sprite Image;

    // true by default but can be set false within subclass constructors
    public bool Stackable = true;

    protected Item(string name, string description, IUseStrategy useStrategy, Sprite image)
    {
        Name = name;
        Amount = 1;
        Description = description;
        UseStrategy = useStrategy;
        Image = image;
    }

    // Different item types will be used differently
    public void Use()
    {
        UseStrategy.Use(this);
    }

    public void SetUseStrategy(IUseStrategy useStrategy)
    {
        UseStrategy = useStrategy;
    }
}

#endregion

#region Subclasses
// The subclasses below each represent a different type of item - some storing additional logic and parameters as needed
// Add new subclasses here to add a new item type

public class HealthItem : Item
{
    public int HealAmount;

    public HealthItem(string name, string description, IUseStrategy useStrategy, Sprite image, int healAmount) : base(name, description, useStrategy, image)
    {
        HealAmount = healAmount;
    }


}

public class CraftingMaterial : Item
{
    public CraftingMaterial(string name, string description, IUseStrategy useStrategy, Sprite image = null) : base(name, description, useStrategy, image)
    {
    }
}

public class Gun : Item
{
    public Gun(string name, string description, IUseStrategy useStrategy, Sprite image = null) : base(name, description, useStrategy, image)
    {
        Stackable = false;
    }
}


#endregion


