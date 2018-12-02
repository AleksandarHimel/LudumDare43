using Assets.Scripts;
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
            foreach (CrewMember crewMember in ship.CrewMembers)
            {
                if (Random.Range(0, 1) <= GameConfig.Instance.PlagueSpreadingProbability)
                {
                    crewMember.PlagueThisGuy();
                }
            }
        }
    }
}
