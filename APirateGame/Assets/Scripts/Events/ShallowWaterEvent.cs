using Assets.Events;
using System;
using UnityEngine;

namespace Assets.Events
{
    public class ShallowWaterEvent : ShipEvent
    {
        public readonly uint ShallowWaterDamageOnShipParts = 1;

        public override void ExecuteEventInternal(MonoBehaviour behaviouralObject)
        {
            Ship shipObject = behaviouralObject as Ship;

            foreach (ShipPart shipPart in shipObject.ShipParts)
            {
                shipPart.TakeDamage(ShallowWaterDamageOnShipParts);
            }            
        }
    }
}