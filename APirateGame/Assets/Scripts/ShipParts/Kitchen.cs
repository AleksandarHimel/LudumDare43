using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : ShipPart {

    public Kitchen(Ship parentShip) : base(parentShip) { }

	// Use this for initialization
	void Start ()
    {
        MaxNumberOfCrewMembers = 10;
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
