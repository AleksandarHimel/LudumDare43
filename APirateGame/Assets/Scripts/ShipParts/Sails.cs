using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sails : ShipPart
{

    public Sails(Ship parentShip) : base(parentShip) { }

    // Use this for initialization
    public override void InitShipPart(Ship ship)
    {
        ParentShip = ship;
        MaxNumberOfCrewMembers = 1;
        MaxHealth = 40;
        Health = MaxHealth;
        Weight = GameConfig.Instance.SailsWeight;

        Description = "Having expirienced sailer operating sails will speed you up";

        this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Sails");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool IsOnBottom()
    {
        return false;
    }
}