using Assets.Events;
using System;
using UnityEngine;

namespace Assets.Events
{
    public class SpreadThePlagueEvent : ShipEvent
    {
        public override void ExecuteEventInternal(Ship ship)
        {
            ship.SpreadPlague();
        }
    }
}