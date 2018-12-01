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
}
