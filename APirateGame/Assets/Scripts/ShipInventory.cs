using UnityEngine;

[CreateAssetMenu]
public class ShipInventory : ScriptableObject
{
    public uint Food { get; set; }
    public uint WoodForFuel { get; set; }

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

    public void TryRemoveAmountOfFood (uint amount)
    {
        Food = Food >= amount ? Food - amount : 0;
    }

    public void TryRemoveAmountOfWood(uint amount)
    {
        WoodForFuel = WoodForFuel >= amount ? WoodForFuel - amount : 0;
    }
}
