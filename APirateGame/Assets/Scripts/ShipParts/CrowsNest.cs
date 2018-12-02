﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowsNest : ShipPart
{
    public CrowsNest(Ship parentShip) : base(parentShip) { }

    // Use this for initialization
    public override void InitShipPart(Ship ship)
    {
        ParentShip = ship;
        MaxNumberOfCrewMembers = 1;
        MaxHealth = 2;
        Health = MaxHealth;
        Weight = GameConfig.Instance.CrowsNestWeight;
    }

    public bool ProvidesScoutingBonus()
    {
        return true;
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