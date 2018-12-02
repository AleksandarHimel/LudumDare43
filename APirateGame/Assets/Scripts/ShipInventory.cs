using System;
using UnityEngine;

[CreateAssetMenu]
public class ShipInventory : ScriptableObject
{
    public uint Food { get; set; }
    public uint WoodForFuel { get; set; }

    public ShipInventory()
    { }

    public ShipInventory(uint food, uint wood)
    {
        InitialiseResources(food, wood);
    }

    public void InitialiseResources (uint food, uint wood)
    {
        Food = food;
        WoodForFuel = wood;
    }

    public void ProcessMoveEnd()
    {
        TryRemoveAmountOfFood(1);
    }

    public void TryRemoveAmountOfFood (uint amount)
    {
        Food = Math.Max(Food - amount, 0);
    }

    public void TryRemoveAmountOfWood(uint amount)
    {
        WoodForFuel = Math.Max(WoodForFuel - amount, 0);
    }

    public void ReduceResources(uint foodToReduce, uint woodToReduce)
    {
        TryRemoveAmountOfFood(foodToReduce);
        TryRemoveAmountOfWood(woodToReduce);
    }
}