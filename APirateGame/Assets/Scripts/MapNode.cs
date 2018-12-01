using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Events;

public class MapNode
{
    public EventEnum Encounter{ get; set; }

    public MapNode() {}

    public MapNode(int numOfPaths)
    {
        Destinations = new List<MapNode>(numOfPaths);
        for (int i = 0; i < numOfPaths; i++)
        {
            Destinations.Add(new MapNode());
        }


    }

    public List <MapNode> Destinations;
    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
