using Assets.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Events
{
    public class HarmShipPartEvent : IEvent
    {
        public readonly uint damageToInflict = 30;

        public void Execute(MonoBehaviour behaviouralObject)
        {
            ShipPart shipPart = behaviouralObject as ShipPart;
            shipPart.TakeDamage(damageToInflict);

            if (shipPart != null)
            {
                foreach (CrewMember crewMember in shipPart.PresentCrewMembers)
                {
                    EventMgr.Instance.ExecuteEvent(EventEnum.HARM_CREW_MEMBER, crewMember);
                }
            }
        }
    }
}
