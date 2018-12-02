using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineRoom : ShipPart {

    public EngineRoom(Ship parentShip) : base (parentShip) { }

    // Use this for initialization
    public override void InitShipPart(Ship ship)
    {
        ParentShip = ship;
        MaxNumberOfCrewMembers = 2;
        MaxHealth = 2;
        Health = MaxHealth;
        Weight = GameConfig.Instance.EngineRoomWeight;
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
