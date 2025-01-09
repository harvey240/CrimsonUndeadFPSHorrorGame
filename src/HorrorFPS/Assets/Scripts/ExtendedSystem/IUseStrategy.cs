using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// UseStrategy interface ensures all Use Strategy classes that implement it will have their own Use(Item item) method
// Makes use of the Strategy Design pattern so the way items are used can be more flexibly assigned/changed/extended
public interface IUseStrategy
{
    void Use(Item item);
}

// Strategy for healing player instantly
public class InstantHealStrategy : IUseStrategy
{
    private int HealAmount;

    public InstantHealStrategy(int healAmount)
    {
        HealAmount = healAmount;
    }

    public void Use(Item item)
    {

        if (PlayerInfo.instance.currentHealth != PlayerInfo.instance.maxHealth)
        {
            // heal player by relevant amount and remove 1 of the item from inventory
            PlayerInfo.instance.Heal(HealAmount);
            InventoryManager.instance.RemoveItem(item);
        }

        else
        {
            // Alert player that health is already full
            InventoryManager.instance.RequestAlert("Health is already full - cannot heal!");
        }
    }
}

#region Use Strategies
// Strategy for healing players gradually per second
public class GradualHealStrategy : IUseStrategy
{
    private int HealAmount;

    public GradualHealStrategy(int healAmount)
    {
        HealAmount = healAmount;
    }

    public void Use(Item item)
    {
        if (PlayerInfo.instance.currentHealth != PlayerInfo.instance.maxHealth)
        {
            // heal player by relevant amount gradually and remove 1 of the item from inventory
            InventoryManager.instance.RemoveItem(item);
            CoroutineRunner.instance.RunCoroutine(GradualHeal());
        }

        else
        {
            // Alert player that health is already full
            InventoryManager.instance.RequestAlert("Health is already full - cannot heal!");
        }
    }

    // use coroutine to heal per second
    public IEnumerator GradualHeal()
    {
        int healingRemaining = HealAmount;
        int healPerSecond = 10;

        while (healingRemaining > 0)
        {
            int gradualHealAmount = Mathf.Min(healPerSecond, healingRemaining);
            PlayerInfo.instance.Heal(gradualHealAmount);
            healingRemaining -= gradualHealAmount;

            yield return new WaitForSeconds(1f);
        }
    }
}

// Empty use strategy for items that cannot be used directly
public class EmptyStrategy : IUseStrategy
{
    public void Use(Item item)
    {
        // Do nothing - for items like CraftingMaterials
        if (item is CraftingMaterial)
        {
            InventoryManager.instance.RequestAlert("Cannot be used directly but is used in crafting!");
        }
    }
}

// Strategy for guns to equip them as current weapon (logic not implemented here as no fleshed out weapon system present in game currently)
public class EquipPrimaryWeaponStrategy : IUseStrategy
{
    public void Use(Item item)
    {
        // Logic can be implemented here to equip the item as primary weapon
        Debug.Log("Equipping as primary weapon");
    }
}

#endregion