using Assets.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public List<ShipPart> ShipParts { get; private set; }
    public List<CrewMember> CrewMembers { get; private set; }

    public ShipInventory Inventory;

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
        // Instantiate some type of ship 4 example:
        // For each ship type there should be specific game object...
        var cannonGO = new GameObject("ShipPart/Cannon");
        cannonGO.transform.parent = gameObject.transform.parent;

        var engineRoomGO = new GameObject("ShipPart/EngineRoom");
        engineRoomGO.transform.parent = gameObject.transform.parent;

        var hullGO = new GameObject("ShipPart/Hull");
        engineRoomGO.transform.parent = gameObject.transform.parent;

        var kitchenGO = new GameObject("ShipPart/Kitchen");
        engineRoomGO.transform.parent = gameObject.transform.parent;

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
            var crewMemberGO = new GameObject("CrewMembers/PlayerCharacter-" + Guid.NewGuid());
            var component = crewMemberGO.AddComponent<CrewMember>();
            component.CurrentShipPart = shipPart;
            CrewMembers.Add(component);
        }

        Inventory = ScriptableObject.CreateInstance<ShipInventory>();
        Inventory.InitialiseResources(100, 100);

        AssetDatabase.CreateAsset(Inventory, "Assets/ScriptableObjectsStatic/ShipInventory.asset");
        AssetDatabase.SaveAssets();
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
}