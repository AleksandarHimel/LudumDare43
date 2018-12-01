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

    private MapNode current;

    private Random RandomGenerator;

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
                for (int k=0; k < RiskDepth; k++)
                { 
                    if (j + 1 == MapLength)
                    {
                        Map[i][j].Destinations[i] = null;
                    }
                    else if (i==k)
                    {
                        Map[i][j].Destinations[i] = Map[i][j+1];
                    }
                    else
                    {
                        Map[i][j].Destinations[i] = (Random.Range(0, 1) < 1.0f / (RiskDepth))? Map[k][j] : null ;
                    }
                }
            }
        }
    }

    public MapNode GetCurrentNode()
    { return current;}

    private void SetCurrentNode(MapNode next)
    { current = next;}

    EventEnum GetRandomEncounter(int MaxEvent)
    {
        return (EventEnum) Random.Range(0, (int) EventEnum.EVENT_MAX-1 );
    }
}