using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hull : ShipPart
{
    public Hull(Ship parentShip) : base(parentShip) { }

    // Use this for initialization
    public override void InitShipPart()
    {
        MaxNumberOfCrewMembers = 0;
        MaxHealth = 2;
        Health = MaxHealth;
        Weight = 100;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool IsOnBottom()
    {
        return true;
    }
}
