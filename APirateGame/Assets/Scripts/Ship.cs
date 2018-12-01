using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public List<ShipPart> ShipParts { get; private set; }
    public List<CrewMember> CrewMembers { get; private set; }

	// Use this for initialization
	void Start ()
    {
        ShipParts = new List<ShipPart>();
        //ShipParts.Add(...)

        CrewMembers = new List<CrewMember>();
        // CrewMembers.Add(...)
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public bool IsDestroyed()
    {
        return false;
    }
}
