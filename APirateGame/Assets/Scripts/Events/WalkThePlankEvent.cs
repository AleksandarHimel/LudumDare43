using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Events
{
    public class WalkThePlankEvent : IEvent
    {
        public readonly int PlagueResourceConsumptionIncrement = 30;

        public void Execute(MonoBehaviour behaviouralObject)
        {
            CrewMember crewMember = behaviouralObject as CrewMember;

            if (crewMember != null)
            {
                //Timmy has to go
                crewMember.ReduceHealth(crewMember.Health);
            }
        }
    }
}