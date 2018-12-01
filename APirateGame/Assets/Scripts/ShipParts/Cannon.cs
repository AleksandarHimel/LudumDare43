using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : ShipPart
{
    public Cannon(Ship parentShip) : base(parentShip) { }

    // Use this for initialization
    public override void InitShipPart()
    {
        // Assign texture
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Cannon");

        MaxNumberOfCrewMembers = 1;
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
