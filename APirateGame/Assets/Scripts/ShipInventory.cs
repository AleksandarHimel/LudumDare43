using UnityEngine;

public class ShipInventory : ScriptableObject
{
    public uint Food { get; private set; }
    public uint WoodForFuel { get; private set; }

    public ShipInventory()
    {
        InitialiseResources(100, 100);
    }

    public ShipInventory(uint food, uint wood)
    {
        InitialiseResources(food, wood);
    }

    private void InitialiseResources (uint food, uint wood)
    {
        Food = food;
        WoodForFuel = wood;
    }

    public uint TryRemoveAmountOfFood (uint amount)
    {
        Food = Food >= foodToReduce ? Food - foodToReduce : 0;
    }

    public void reduceWood (uint woodToReduce)
    {
        WoodForFuel = WoodForFuel >= woodToReduce ? WoodForFuel - woodToReduce : 0;
    }

    public uint TryRemoveAmountOfWood(uint amount)
    {
        reduceFood(foodToReduce);
        reduceWood(woodToReduce);      
    }
}
