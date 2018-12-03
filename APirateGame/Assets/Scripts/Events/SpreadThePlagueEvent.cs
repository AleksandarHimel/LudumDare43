using Assets.Events;
using Assets.Scripts;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Events
{
    public class SpreadThePlagueEvent : ShipEvent
    {
        public override void ExecuteEventInternal(Ship ship)
        {
            foreach (ShipPart shipPart in ship.FunctioningShipParts)
            {
                bool isRoomSafe = !shipPart.PresentCrewMembers.Any(crew => crew.IsUnderPlague);

                if (!isRoomSafe)
                {
                    foreach (CrewMember crewMember in shipPart.PresentCrewMembers)
                    {
                        if (!crewMember.IsUnderPlague)
                        {
                            if (UnityEngine.Random.Range(0f, 1f) > GameConfig.Instance.PlagueSpreadingProbability)
                            {
                                crewMember.PlagueThisGuy();
                                FullEventDetailsMessage += String.Format("Oh no, it's contagious. Now even {0} is sick! \n", crewMember.PirateName);
                            }
                        }
                    }
                }
            }
        }

        public override string eventDescription()
        {
            return "The black spot is upon us! \n";
        }
    }
}