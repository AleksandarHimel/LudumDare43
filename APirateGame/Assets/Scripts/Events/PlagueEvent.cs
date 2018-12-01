using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Events
{
    public class PlagueEvent : IEvent
    {
        public readonly int PlagueResourceConsumptionIncrement = 30;

        public void Execute(MonoBehaviour behaviouralObject)
        {
            CrewMember crewMember = behaviouralObject as CrewMember;

           if (crewMember != null)
           {
                if (!crewMember.IsUnderPlague)
                {
                    crewMember.ResourceConsumption += PlagueResourceConsumptionIncrement;
                    crewMember.IsUnderPlague = true;
                }
            }
        }
    }
}
