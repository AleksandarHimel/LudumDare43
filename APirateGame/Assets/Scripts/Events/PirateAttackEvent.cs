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
                uint damage = (uint)getRandNum(GameConfig.Instance.MinPirateAttackShipPartDamage, GameConfig.Instance.MaxPirateAttackShipPartDamage);
                shipPart.TakeDamage(damage);
                FullEventDetailsMessage += String.Format("Pirates fired their cannons and {0} took {1} damage /n", shipPart.name, damage);
            }

            foreach (CrewMember crewMember in shipObject.CrewMembers)
            {
                int damage = getRandNum(GameConfig.Instance.MinPirateAttackCrewMemberDamage, GameConfig.Instance.MaxPirateAttackCrewMemberDamage);
                crewMember.ReduceHealth(damage);
                FullEventDetailsMessage += String.Format("{0} fought like a true pirate but took {1} damage /n", crewMember.PirateName, damage);
            }

            int foodLooted = getRandNum(GameConfig.Instance.MinPirateAttackResourceDamage, GameConfig.Instance.MaxPirateAttackResourceDamage);
            shipObject.Inventory.ReduceResources((uint)foodLooted, 0);
            if (foodLooted > 0)
                FullEventDetailsMessage += String.Format("Damn pirates! {0} food was looted from your ship /n", foodLooted);
        }

        public override string eventDescription()
        {
            return "Ye be attacked by some pirate ruffians! \n";
        }
    }
}