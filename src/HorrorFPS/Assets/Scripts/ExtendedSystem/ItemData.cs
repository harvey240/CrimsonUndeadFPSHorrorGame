using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// Scriptable object to store data for different items
// Does not have to be a scriptable object but it allows for much easier usage and creation when in the Unity inspector in my case
[CreateAssetMenu(fileName = "New Item", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string Name;
    public string Description;
    public Sprite Image;

    [Header("Optionally Specifcy the use strategy")]
    // Is unused by default but can be specified
    // e.g. for a HealthItem that wants to use a use strategy other than an instant heal
    public UseType UseStrategy = UseType.DEFAULT;

    [Header("Gun Specific Atributes")]
    // Attributes for guns
    public int damage;

    [Header("Health Specific Atributes")]
    // Attributes for HealthItems
    public int healAmount;
}

#region Enums for Item and Use Types
// Extend this if adding new item type
public enum ItemType
{
    HEALTHITEM,
    CRAFTINGMATERIAL,
    GUN
}

// Extend this when adding a new strategy
public enum UseType
{
    DEFAULT,
    INSTANTHEAL,
    GRADUALHEAL,
    EMPTY,
    EQUIPPRIMARY
}

#endregion