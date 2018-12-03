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
            foreach (ShipPart shipPart in shipObject.FunctioningShipParts)
            {
                if (shipPart.IsOnBottom())
                {
                    uint damage = (uint)getRandNum(GameConfig.Instance.MinShallowWaterShipPartDamage, GameConfig.Instance.MaxShallowWaterShipPartDamage);
                    shipPart.TakeDamage(damage);
                    FullEventDetailsMessage += String.Format("Damned be the shallow seas! {0} took {1} damage \n", shipPart.name, damage);
                }
            }            
        }

        public override string eventDescription()
        {
            return "Avast ye! We be hittin' shallower waters! \n";
        }
    }
}