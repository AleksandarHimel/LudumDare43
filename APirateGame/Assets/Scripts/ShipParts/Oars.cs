using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oars : ShipPart {

    public Oars(Ship parentShip) : base (parentShip) { }

    // Use this for initialization
    public override void InitShipPart(Ship ship)
    {
        ParentShip = ship;
        MaxNumberOfCrewMembers = 2;
        MaxHealth = 80;
        Health = MaxHealth;
        Weight = GameConfig.Instance.OarsWeight;

        Description = "The ship moves faster when there are people manning the oars";
    }
    
    // Update is called once per frame
    void Update ()
    {
        
    }

    public override bool IsOnBottom()
    {
        return true;
    }
}
