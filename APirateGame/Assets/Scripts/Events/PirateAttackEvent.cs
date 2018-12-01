using Assets.Events;
using System;
using UnityEngine;

namespace Assets.Events
{
    public class PirateAttackEvent : ShipEvent
    {
        public readonly uint PirateAttackDamageOnShipParts = 2;
        public readonly int PirateAttackDamageOnCrewMembers = 2;
        public readonly uint PirateAttackFoodLooted = 1;
        public readonly uint PirateAttackWoodLooted = 1;

        public override void ExecuteEventInternal(MonoBehaviour behaviouralObject)
        {
            Ship shipObject = behaviouralObject as Ship;

            foreach (ShipPart shipPart in shipObject.ShipParts)
            {
                shipPart.TakeDamage(PirateAttackDamageOnShipParts);
            }
            foreach (CrewMember crewMember in shipObject.CrewMembers)
            {
                crewMember.ReduceHealth(PirateAttackDamageOnCrewMembers);
            }
            shipObject.Inventory.TryRemoveAmountOfFood(PirateAttackFoodLooted);
            shipObject.Inventory.TryRemoveAmountOfWood(PirateAttackWoodLooted);
        }
    }
}