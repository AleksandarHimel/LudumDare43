using Assets.Events;
using Assets.Scripts;
using System;
using UnityEngine;

namespace Assets.Events
{
    public class FireEvent : ShipEvent
    {
        public override void ExecuteEventInternal(Ship behaviouralObject)
        {
            Ship shipObject = behaviouralObject as Ship;

            foreach (ShipPart shipPart in shipObject.ShipParts)
            {
                shipPart.TakeDamage((uint)getRandNum(GameConfig.Instance.MinFireShipPartDamage, GameConfig.Instance.MaxFireShipPartDamage));
            }
            foreach (CrewMember crewMember in shipObject.CrewMembers)
            {
                crewMember.ReduceHealth(getRandNum(GameConfig.Instance.MinFireCrewMemberDamage, GameConfig.Instance.MaxFireCrewMemberDamage));
            }
        }

        public override string eventDescription()
        {
            return "Fire! Avast ye, or we be scramblin' to Davy Jones locker!";
        }
    }
}