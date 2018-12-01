using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CrewMember : MonoBehaviour {

    public ShipPart CurrentShipPart;
    public int Health;
    public bool IsUnderPlauge;
    public int ResourceConsumption;

    public Ship ship;

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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
}
