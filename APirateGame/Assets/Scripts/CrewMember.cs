using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember : MonoBehaviour {

    public ShipPart CurrentShipPart;
    public int Health;
    public bool isDead;
    public bool IsUnderPlauge;
    public int ResourceConsumption;

    public Ship ship;

    public CrewMember(ShipPart StartingShipPart)
    {
        Health = 10;
        isDead = false;
        CurrentShipPart = StartingShipPart;
    }

    public void ReduceHealth(int damage)
    {
        Health -= damage;
        if (Health <= 0)
            isDead = true;
    }
}
