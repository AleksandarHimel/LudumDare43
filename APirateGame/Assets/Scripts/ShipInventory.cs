using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInventory : MonoBehaviour
{

    public uint Food { get; private set; }
    public uint WoodForFuel { get; private set; }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitialiseResources (uint food, uint wood)
    {
        Food = food;
        WoodForFuel = wood;
    }
}
