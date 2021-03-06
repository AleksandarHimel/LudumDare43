﻿using Assets.Scripts;

public class Kitchen : ShipPart {

    public Kitchen(Ship parentShip) : base(parentShip) { }

    // Use this for initialization
    public override void InitShipPart(Ship ship)
    {
        ParentShip = ship;
        MaxNumberOfCrewMembers = 3;
        MaxHealth = 80;
        Health = MaxHealth;
        Weight = GameConfig.Instance.KitchenWeight;

        Description = "Placing crew members in the kitchen will keep your crew well fed and healthy";
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
