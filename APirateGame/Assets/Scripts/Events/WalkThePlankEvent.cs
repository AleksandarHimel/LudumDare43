using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Events
{
    public class WalkThePlankEvent : ShipEvent
    {
        public override void ExecuteEventInternal(Ship behaviouralObject)
        {
            // TODO: Fix if you wish to use

            //throw new NotImplementedException();

            //Ship shipObject = behaviouralObject as Ship;

            //int shortStraw = getRandNum(0, shipObject.AliveCrewMembers.Count + 1);

            //CrewMember crewMemberToWalkThePlank = shipObject.AliveCrewMembers[shortStraw];
            //CrewMember Timmy = crewMemberToWalkThePlank;

            ////Timmy has to go
            //Timmy.ReduceHealth(Timmy.Health);
        }

        public override string eventDescription()
        {
            return "Timmy you scoundrel! Off with you! \n";
        }
    }
}