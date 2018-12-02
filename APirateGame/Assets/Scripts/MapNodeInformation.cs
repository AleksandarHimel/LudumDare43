using Assets.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class MapNodeInformation
    {
        public List<EventEnum> PossibleEncounter;

        public int Riskiness;

        public MapNodeInformation(IEnumerable<EventEnum> possibleEncounters, int riskiness)
        {
            foreach (EventEnum ev in possibleEncounters)
            {
                PossibleEncounter.Add(ev);
            }

            Riskiness = riskiness;
        }
    }
}
