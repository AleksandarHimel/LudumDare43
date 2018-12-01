using Assets.Events;
using System;
using UnityEngine;

namespace Assets.Events
{
    public class FireEvent : ShipEvent
    {
        public readonly uint FireDamageOnShipParts = 2;
        public readonly int FireDamageOnCrewMembers = 2;

        public override void ExecuteEventInternal(Ship behaviouralObject)
        {
            Ship shipObject = behaviouralObject as Ship;

            foreach (ShipPart shipPart in shipObject.ShipParts)
            {
                shipPart.TakeDamage(FireDamageOnShipParts);
            }
            foreach (CrewMember crewMember in shipObject.CrewMembers)
            {
                crewMember.ReduceHealth(FireDamageOnCrewMembers);
            }
        }
    }
}