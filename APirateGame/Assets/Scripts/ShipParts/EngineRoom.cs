using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineRoom : ShipPart {

    public EngineRoom(Ship parentShip) : base (parentShip) { }

    // Use this for initialization
    public override void InitShipPart()
    {
        MaxNumberOfCrewMembers = 2;
        MaxHealth = 2;
        Health = MaxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override bool isOnBottom()
    {
        return false;
    }
}
