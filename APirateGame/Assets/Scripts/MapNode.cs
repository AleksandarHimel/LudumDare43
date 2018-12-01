using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Events;

public class MapNode : MonoBehaviour
{
    public EventEnum Encounter{ get; set; }

    public MapNode(int numOfPaths)
    {
        Destinations = new List<MapNode>(numOfPaths);
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
