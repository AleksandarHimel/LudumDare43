using Assets.Events;
using System;
using UnityEngine;

namespace Assets.Events
{
    public class SpreadThePlague : ShipEvent
    {
        public override void ExecuteEventInternal(Ship ship)
        {
            ship.SpreadPlague();
        }

        public override string eventDescription()
        {
            return "The black spot is upon us!";
        }
    }
}