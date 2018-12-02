using Assets.Events;
using Assets.Scripts;
using System;
using UnityEngine;

namespace Assets.Events
{
    public class ShallowWaterEvent : ShipEvent
    {
        public override void ExecuteEventInternal(Ship shipObject)
        {
            foreach (ShipPart shipPart in shipObject.ShipParts)
            {
                if (shipPart.IsOnBottom())
                {
                    shipPart.TakeDamage((uint)getRandNum(GameConfig.Instance.MinShallowWaterShipPartDamage, GameConfig.Instance.MaxShallowWaterShipPartDamage));
                }
            }            
        }

        public override string eventDescription()
        {
            return "Avast ye! We be hittin' shallower waters!";
        }
    }
}