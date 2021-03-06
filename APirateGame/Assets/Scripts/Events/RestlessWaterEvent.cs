﻿using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Events
{
    public class RestlessWaterEvent : ShipEvent
    {
        public override void ExecuteEventInternal(Ship ship)
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            foreach (CrewMember crewMember in ship.AliveCrewMembers)
            {
                if (!(crewMember.CurrentShipPart is Kitchen) && Random.Range(0f, 1f) <= GameConfig.Instance.PlagueSpreadingProbability)
                {
                    crewMember.PlagueThisGuy();
                    FullEventDetailsMessage += System.String.Format("Oh noo, {0} got sick! \n", crewMember.PirateName);
                }
            }
        }

        public override string eventDescription()
        {
            return "Batten down the hatches! Tis sea be filled with waves! \n";
        }
    }
}
