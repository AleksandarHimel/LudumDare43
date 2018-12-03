using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Assets.Events;
using System.Linq;

namespace Assets.Scripts
{
    public class MapManager
    {
        private static MapManager _instance;

        private static int RiskDepth = 3;

        private static int MapLength = 50;

        private List<List<MapNode>> Map;

        private MapNode Current;

        private List<MapNode> StartingDestinations;

        public static MapManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MapManager();
                }

                return _instance;
            }
        }

        MapManager()
        {
            Current = null;
            Map = new List<List<MapNode>>();

            for (int riskDepth = 0; riskDepth < (int)RiskPathEnum.MAX_RISK_PATH; riskDepth++)
            {
                Map.Add(new List<MapNode>());
                // Generate each path
                for (int mapPosition = 0; mapPosition < MapLength; mapPosition++)
                {
                    Map[riskDepth].Add(new MapNode(RiskDepth, riskDepth));

                    //(Devnote - Srki) Fix once we get eligible encounters in 
                    Map[riskDepth][mapPosition].NodeEvent = EventManager
                        .Instance.ComposeEvent()
                        .AddEvent(EventEnum.SPREAD_THE_PLAGUE)
                        .AddEvent(GetRandomEncounter(riskDepth));
                        //.AddEvent(EventEnum.CONSUME_RESOURCES_BETWEEEN_STAGES);
                }
            }

            // Link allowed paths
            // Best code ever. Don't touch! I know you want to...
            for (int mapPosition = 0; mapPosition < MapLength; mapPosition++)
            {
                for (int originRiskDepth = 0; originRiskDepth < (int)RiskPathEnum.MAX_RISK_PATH; originRiskDepth++)
                {
                    for (int destRiskDepth = 0; destRiskDepth < (int)RiskPathEnum.MAX_RISK_PATH; destRiskDepth++)
                    {
                        if (mapPosition + 1 >= MapLength)
                        {
                            Map[originRiskDepth][mapPosition].Destinations[destRiskDepth] = null;
                        }
                        else if (originRiskDepth == destRiskDepth)
                        {
                            Map[originRiskDepth][mapPosition].Destinations[destRiskDepth] = Map[originRiskDepth][mapPosition + 1];
                        }
                        else
                        {
                            Map[originRiskDepth][mapPosition].Destinations[destRiskDepth] = (Random.Range(0.0f, 1.0f) < 1.0f / ((int)RiskPathEnum.MAX_RISK_PATH - 1.0f)) ? Map[destRiskDepth][mapPosition + 1] : null;
                        }
                    }
                }
            }

            StartingDestinations = new List<MapNode>((int)RiskPathEnum.MAX_RISK_PATH);

            for (int riskDepth = 0; riskDepth < (int)RiskPathEnum.MAX_RISK_PATH; riskDepth++)
            {
                StartingDestinations.Add(new MapNode());

                StartingDestinations[riskDepth] = Map[riskDepth][0];
            }
        }

        public MapNode GetCurrentNode()
        { return Current; }

        public IEnumerable<MapNodeInformation> GetPossibleDestinations()
        {
            IEnumerable<MapNode> possibleNodeDestinations = (Current == null) ? StartingDestinations : Current.Destinations.Where(node => node != null);

            return possibleNodeDestinations.Select(node => new MapNodeInformation(GetRiskinessEvents(node.Riskiness), node.Riskiness));
        }

        public void GoToNextDestination(int riskiness)
        {
            List<MapNode> possibleNodeDestinations = (Current == null) ? StartingDestinations : Current.Destinations;
            if (riskiness >= 0)
            {
                Current = possibleNodeDestinations[riskiness];
            }
            else
            {
                int possibleDestinationsCount = possibleNodeDestinations.
                                                Where(node => node != null).Count();

                int fate = Random.Range(0, possibleDestinationsCount);
                Current = possibleNodeDestinations.Where(node => node != null).ToArray()[fate];
            }
        }

        public IEnumerable<EventEnum> GetRiskinessEvents(int riskiness)
        {
            List<int> possibleEvents = new List<int>();

            switch (riskiness)
            {
                case 0:
                    for (int i = 0; i <= (int)EventEnum.MAX_FIRST_TIER; i++)
                    {
                        possibleEvents.Add(i);
                    }

                    break;
                case 1:
                    for (int i = (int)EventEnum.MAX_FIRST_TIER + 1; i <= (int)EventEnum.MAX_SECOND_TIER; i++)
                    {
                        possibleEvents.Add(i);
                    }
                    break;
                default:
                    for (int i = (int)EventEnum.MAX_SECOND_TIER + 1; i <= (int)EventEnum.MAX_THIRD_TIER; i++)
                    {
                        possibleEvents.Add(i);
                    }
                    break;
            }

            return possibleEvents.Select(ev => (EventEnum)ev);
        }

        EventEnum GetRandomEncounter(int riskTier)
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            switch (riskTier)
            {
                case 0:
                    return (EventEnum)Random.Range(0, (int)EventEnum.MAX_FIRST_TIER);
                case 1:
                    return (EventEnum)Random.Range((int)EventEnum.MAX_FIRST_TIER + 1, (int)EventEnum.MAX_SECOND_TIER);
                default:
                    return (EventEnum)Random.Range((int)EventEnum.MAX_SECOND_TIER + 1, (int)EventEnum.MAX_THIRD_TIER);
            }
        }
    }
}
