using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public List<ShipPart> ShipParts { get; private set; }
    public List<CrewMember> CrewMembers { get; private set; }

    public Dictionary<CrewMember, ShipPart> Assignment { get; private set; }

    // Assign crew member to the ship part
    public void AssignCrewMember(CrewMember crew, ShipPart part)
    {
        if (Assignment.ContainsKey(crew))
        {
            Assignment.Remove(crew);
        }

        Assignment.Add(crew, part);
    }

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
		// hi my name is andrija
	}

    public bool IsDestroyed()
    {
        return false;
    }
}
