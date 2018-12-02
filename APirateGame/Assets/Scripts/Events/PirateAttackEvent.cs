using Assets.Events;
using Assets.Scripts;
using System;
using UnityEngine;

namespace Assets.Events
{
    public class PirateAttackEvent : ShipEvent
    {
        public override void ExecuteEventInternal(Ship shipObject)
        {
            foreach (ShipPart shipPart in shipObject.ShipParts)
            {
                shipPart.TakeDamage((uint)getRandNum(GameConfig.Instance.MinPirateAttackShipPartDamage, GameConfig.Instance.MaxPirateAttackShipPartDamage));
            }

            foreach (CrewMember crewMember in shipObject.CrewMembers)
            {
                crewMember.ReduceHealth(getRandNum(GameConfig.Instance.MinPirateAttackCrewMemberDamage, GameConfig.Instance.MaxPirateAttackCrewMemberDamage));
            }

            int foodLooted = getRandNum(GameConfig.Instance.MinPirateAttackResourceDamage, GameConfig.Instance.MaxPirateAttackResourceDamage);
            shipObject.Inventory.ReduceResources((uint)foodLooted, 0);
        }

        public override string eventDescription()
        {
            return "Ye be attacked by some pirate ruffians!";
        }
    }
}