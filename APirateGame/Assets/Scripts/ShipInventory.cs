using UnityEngine;

public class ShipInventory : MonoBehaviour
{
    public uint Food { get; private set; }
    public uint WoodForFuel { get; private set; }

    public ShipInventory(uint food, uint wood)
    {
        InitialiseResources(food, wood);
    }

    // Use this for initialization
    void Start()
    {
        InitialiseResources(100, 100);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitialiseResources (uint food, uint wood)
    {
        Food = food;
        WoodForFuel = wood;
    }

    public uint tryRemoveAmountOfFood (uint amount)
    {
        uint retVal = Food >= amount ? amount : Food;
        Food = Food >= amount ? Food - amount : 0;
        return retVal;
    }

    public uint tryRemoveAmountOfWood(uint amount)
    {
        uint retVal = WoodForFuel >= amount ? amount : WoodForFuel;
        WoodForFuel = WoodForFuel >= amount ? WoodForFuel - amount : 0;
        return retVal;
    }
}
