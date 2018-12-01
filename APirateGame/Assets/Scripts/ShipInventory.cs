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
        uint retVal = Food >= amount ? amount : Food;
        Food = Food >= amount ? Food - amount : 0;
        return retVal;
    }

    public uint TryRemoveAmountOfWood(uint amount)
    {
        uint retVal = WoodForFuel >= amount ? amount : WoodForFuel;
        WoodForFuel = WoodForFuel >= amount ? WoodForFuel - amount : 0;
        return retVal;
    }
}
