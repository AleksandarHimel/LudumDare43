using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : ShipPart
{
    public Cannon(Ship parentShip) : base(parentShip) { }

    // Use this for initialization
    public override void InitShipPart(Ship ship)
    {
        ParentShip = ship;
        // Assign texture

        this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Cannon");

        MaxNumberOfCrewMembers = 1;
        MaxHealth = 2;
        Health = MaxHealth;
        Weight = GameConfig.Instance.CannonWeight;
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
