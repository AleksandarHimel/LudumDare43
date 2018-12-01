using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember : MonoBehaviour {

    public ShipPart CurrentShipPart;
    public int Health;

    public CrewMember(ShipPart StartingShipPart)
    {
        Health = 10;
        CurrentShipPart = StartingShipPart;
    }

    public void ReduceHealth(int damage)
    {
        Health -= damage;
    }

    public bool IsDead()
    {
        if (Health <= 0)
            return true;
        return false;
    }
}
