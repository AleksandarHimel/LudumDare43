using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : ShipPart {

    public Kitchen(Ship parentShip) : base(parentShip) { }

    // Use this for initialization
    public override void InitShipPart(Ship ship)
    {
        ParentShip = ship;
        MaxNumberOfCrewMembers = 10;
        MaxHealth = 2;
        Health = MaxHealth;
        Weight = 150;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override bool IsOnBottom()
    {
        return false;
    }
}
