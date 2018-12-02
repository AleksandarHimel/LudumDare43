using Assets.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ship : MonoBehaviour, IPointerClickHandler
{
    public List<ShipPart> ShipParts { get; private set; }
    public List<CrewMember> CrewMembers { get; private set; }

    public ShipInventory Inventory { get; set; }

    public double PlagueSpreadingProbability = 0.3;

    // Assign crew member to the ship part
    public void AssignCrewMember(CrewMember crew, ShipPart part)
    {
        if (part.MaxNumberOfCrewMembers == part.PresentCrewMembers.Count())
        {
            throw new Exception("Ship part is full!");
        }

        crew.CurrentShipPart = part;
    }

	// Use this for initialization
	void Start ()
    {
        Inventory = new ShipInventory(100, 100);

        // Instantiate some type of ship 4 example:
        // For each ship type there should be specific game object...
        var cannonGO = new GameObject("ShipPart/Cannon");
        cannonGO.transform.parent = gameObject.transform;
        cannonGO.transform.localPosition = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), -0.37f);

        var engineRoomGO = new GameObject("ShipPart/EngineRoom");
        engineRoomGO.transform.parent = gameObject.transform;
        engineRoomGO.transform.localPosition = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), -0.37f);

        var hullGO = new GameObject("ShipPart/Hull");
        engineRoomGO.transform.parent = gameObject.transform;
        hullGO.transform.localPosition = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), -0.37f);

        var kitchenGO = new GameObject("ShipPart/Kitchen");
        engineRoomGO.transform.parent = gameObject.transform;
        kitchenGO.transform.localPosition = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), -0.37f);

        ShipParts = new List<ShipPart>
        {
            cannonGO.AddComponent<Cannon>(),
            engineRoomGO.AddComponent<EngineRoom>(),
            hullGO.AddComponent<Hull>(),
            kitchenGO.AddComponent<Kitchen>()
        };

        CrewMembers = new List<CrewMember>();
        // TODO: this is temp, depending on crew member size compared to ship part count
        foreach (var shipPart in ShipParts)
        {
            // Create instance of a Pirate
            var crewMemberGO = Instantiate(Resources.Load<GameObject>("Prefabs/Pirate"), transform);
            crewMemberGO.name = "CrewMembers /PlayerCharacter-" + Guid.NewGuid();

            // TODO: read positions of crew members relative to boat
            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
            crewMemberGO.transform.localPosition = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), -0.37f);

            // Add component
            var component = crewMemberGO.AddComponent<CrewMember>();
            component.CurrentShipPart = shipPart;
            CrewMembers.Add(component);
        }
    }

    internal void ProcessMoveEnd()
    {
        Debug.Log("Ship processing move end");
        Inventory.TryRemoveAmountOfFood(1);
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
            if (!cm.IsDead)
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
            bool isRoomSafe = !shipPart.PresentCrewMembers.Any(crew => crew.IsUnderPlague);

            if (!isRoomSafe)
            {
                foreach (CrewMember crewMember in shipPart.PresentCrewMembers)
                {
                    if (!crewMember.IsUnderPlague)
                    {
                        if (UnityEngine.Random.Range(0, 1) > PlagueSpreadingProbability)
                        {
                            EventManager.Instance.ExecuteEvent(EventEnum.PLAGUE, crewMember);
                        }
                    }
                }
            }
        }
    }

    void TryApplyPlague(CrewMember crewMember)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(name + " Game Object Clicked!");
    }
}