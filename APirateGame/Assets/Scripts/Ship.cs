using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public List<ShipPart> ShipParts { get; private set; }
    public List<CrewMember> CrewMembers { get; private set; }

    public Dictionary<CrewMember, ShipPart> Assignment { get; private set; }

    public double PlagueSpreadingProbability = 0.3;

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
        var cannon = new Cannon(this);
        var engineRoom = new EngineRoom(this);
        var hull = new Hull(this);
        var kitchen = new Kitchen(this);

        ShipParts = new List<ShipPart>
        {
            new Cannon(this),
            new EngineRoom(this),
            new Hull(this),
            new Kitchen(this)
        };


        CrewMembers = new List<CrewMember>
        {
            new CrewMember(cannon),
            new CrewMember(engineRoom),
            new CrewMember(kitchen),
            new CrewMember(kitchen)
        };
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

    public void SpreadPlague()
    {
        foreach (ShipPart shipPart in ShipParts)
        {
            bool isRoomSafe = !shipPart.PresentCrewMembers.Any(crew => crew.IsUnderPlauge);

            if (!isRoomSafe)
            {
                foreach (CrewMember crewMember in shipPart.PresentCrewMembers)
                {
                    if (!crewMember.IsUnderPlauge)
                    {
                        if (Random.RandomRange(0, 1) > PlagueSpreadingProbability)
                        {

                        }
                    }
                }
            }
        }
    }

    void TryApplyPlague(CrewMember crewMember)
    {

    }
}
