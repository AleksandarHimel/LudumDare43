using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Assets.Events;

public class MapManager : MonoBehaviour
{

    private static int RiskDepth = 3;
    private static int MapLength = 12;

    private List<List<MapNode>> Map;

    private MapNode Current;

    private Random RandomGenerator;

    private List<MapNode> StartingDestinations;

    // Use this for initialization
    void Start()
    {
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateMap()
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
                Map[i][j].Encounter = GetRandomEncounter(10 * (i + 1));
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
                    if (j + 1 == MapLength)
                    {
                        Map[i][j].Destinations[i] = null;
                    }
                    else if (i == k)
                    {
                        Map[i][j].Destinations[i] = Map[i][j + 1];
                    }
                    else
                    {
                        Map[i][j].Destinations[i] = (Random.Range(0, 1) < 1.0f / (RiskDepth)) ? Map[k][j] : null;
                    }
                }
            }
        }

        StartingDestinations = new List<MapNode>(RiskDepth);

        for (int i = 0; i < RiskDepth; i++)
        {
            StartingDestinations[i] = Map[0][i];
        }
    }

    public MapNode GetCurrentNode()
    { return Current;}

    private void SetCurrentNode(MapNode next)
    { Current = next;}

    List<MapNode> GetPossibleDestinations()
    {
        return (Current == null) ? StartingDestinations : Current.Destinations;
    }

    EventEnum GetRandomEncounter(int MaxEvent)
    {
        return (EventEnum) Random.Range(0, (int) EventEnum.EVENT_MAX-1 );
    }
}