using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Assets.Events;

public class MapManager
{
    private static MapManager _instance;

    private static int RiskDepth = 3;

    private static int MapLength = 12;

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

        for (int i = 0; i < RiskDepth; i++)
        {
            Map.Add(new List<MapNode>());
            // Generate each path
            for (int j = 0; j < MapLength; j++)
            {
                Map[i].Add(new MapNode(RiskDepth));

                //(Devnote - Srki) Fix once we get eligible encounters in order.
                Map[i][j].Encounter = GetRandomEncounter(i);
            }
        }

        // Link allowed paths
        // Best code ever. Don't touch! I know you want to...
        for (int j = 0; j < MapLength; j++)
        {
            for (int i = 0; i < RiskDepth; i++)
            {
                for (int k = 0; k < RiskDepth; k++)
                {
                    if (j + 1 >= MapLength)
                    {
                        Map[i][j].Destinations[k] = null;
                    }
                    else if (i == k)
                    {
                        Map[i][j].Destinations[k] = Map[i][j + 1];
                    }
                    else
                    {
                        Map[i][j].Destinations[k] = (Random.Range(0.0f, 1.0f) < 1.0f / (RiskDepth-1.0f)) ? Map[k][j+1] : null;
                    }
                }
            }
        }

        StartingDestinations = new List<MapNode>(RiskDepth);

        for (int i = 0; i < RiskDepth; i++)
        {
            StartingDestinations.Add(new MapNode());

            StartingDestinations[i] = Map[0][i];
        }
    }

    public MapNode GetCurrentNode()
    { return Current;}

    private void SetCurrentNode(MapNode next)
    { Current = next;}

    public List<MapNode> GetPossibleDestinations()
    {
        return (Current == null) ? StartingDestinations : Current.Destinations;
    }

    EventEnum GetRandomEncounter(int riskTier)
    {
        switch (riskTier)
        {
            case 0 :
                return (EventEnum)Random.Range(0, (int)EventEnum.MAX_FIRST_TIER);
            case 1:
                return (EventEnum)Random.Range((int)EventEnum.MAX_FIRST_TIER + 1, (int)EventEnum.MAX_SECOND_TIER);
            default:
                return (EventEnum)Random.Range((int)EventEnum.MAX_SECOND_TIER + 1, (int)EventEnum.MAX_THIRD_TIER);
        }
    }
}