using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : ShipPart {

    public Kitchen(Ship parentShip) : base(parentShip) { }

    // Use this for initialization
    public override void InitShipPart(Ship ship)
    {
        ParentShip = ship;
        MaxNumberOfCrewMembers = 3;
        MaxHealth = 2;
        Health = MaxHealth;
        Weight = GameConfig.Instance.KitchenWeight;
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
