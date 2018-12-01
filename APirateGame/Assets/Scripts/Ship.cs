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
        ShipParts = new List<ShipPart>
        {
            new Cannon(),
            new EngineRoom(),
            new Hull()
        };


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
        // the ship is destroyed if there are no more crew members remaining
        bool hasLivingCrewMember = false;

        foreach (CrewMember cm in CrewMembers)
        {
            if (!cm.IsDead())
            {
                hasLivingCrewMember = true;
                break;
            }
        }

        if (!hasLivingCrewMember)
        {
            return true;
        }

        // the ship is destroyed if all parts of the ship are destroyed
        bool hasWorkingShipPart = false;

        foreach (ShipPart sp in ShipParts)
        {
            if (!sp.IsDestroyed)
            {
                hasWorkingShipPart = true;
                break;
            }
        }

        if (!hasWorkingShipPart)
        {
            return true;
        }

        return false;
    }
}
