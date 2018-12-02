using Assets.Events;
using System;
using UnityEngine;

namespace Assets.Events
{
    public class ShallowWaterEvent : ShipEvent
    {
        public readonly uint ShallowWaterDamageOnShipParts = 1;

        public override void ExecuteEventInternal(Ship shipObject)
        {
            foreach (ShipPart shipPart in shipObject.ShipParts)
            {
                if (shipPart.IsOnBottom())
                {
                    shipPart.TakeDamage(ShallowWaterDamageOnShipParts);
                }
            }            
        }
    }
}