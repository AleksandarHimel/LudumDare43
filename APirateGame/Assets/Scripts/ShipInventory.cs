using UnityEngine;

[CreateAssetMenu]
public class ShipInventory : ScriptableObject
{
    public uint Food { get; set; }
    public uint WoodForFuel { get; set; }

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

    public void ReduceResources(uint foodToReduce, uint woodToReduce)
    {

    }
}