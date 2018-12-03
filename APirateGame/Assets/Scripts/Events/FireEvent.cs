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

            foreach (ShipPart shipPart in shipObject.FunctioningShipParts)
            {
                uint damage = (uint)getRandNum(GameConfig.Instance.MinFireShipPartDamage, GameConfig.Instance.MaxFireShipPartDamage);
                shipPart.TakeDamage(damage);
                FullEventDetailsMessage += String.Format("{0} was burning and it took {1} damage \n", shipPart.name, damage);
            }
            foreach (CrewMember crewMember in shipObject.AliveCrewMembers)
            {
                int damage = getRandNum(GameConfig.Instance.MinFireCrewMemberDamage, GameConfig.Instance.MaxFireCrewMemberDamage);
                crewMember.ReduceHealth(damage);
                FullEventDetailsMessage += String.Format("{0} got burned for {1} damage \n", crewMember.PirateName, damage);
            }
        }

        public override string eventDescription()
        {
            return "Fire! Avast ye, or we be scramblin' to Davy Jones locker! \n";
        }
    }
}