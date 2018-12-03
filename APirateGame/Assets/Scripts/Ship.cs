using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ship : MonoBehaviour, IPointerClickHandler
{
    public List<ShipPart> ShipParts { get; private set; }
    public List<CrewMember> CrewMembers { get; private set; }
    public List<CrewMember> DeceasedCrewMembers { get; private set; }

    public ShipInventory Inventory { get; set; }
    public CrewMember SelectedCrewMember { get; private set; }

    // Assign crew member to the ship part
    public void AssignCrewMember(CrewMember crew, ShipPart part)
    {
        if (crew.CurrentShipPart == part)
        {
            return;
        }

        if (part.MaxNumberOfCrewMembers <= part.PresentCrewMembers.Count())
        {
            throw new Exception("Ship part is full!");
        }

        crew.CurrentShipPart = part;
        GameManager.Instance.UiController.UpdateChoices(GameManager.Instance.MapManager.GetPossibleDestinations());
    }

    // Use this for initialization
    void Awake()
    {
        Inventory = ScriptableObject.CreateInstance<ShipInventory>();
        Inventory.InitialiseResources(GameConfig.Instance.InitialFoodCount, GameConfig.Instance.InitialWoodCount);
        DeceasedCrewMembers = new List<CrewMember>();

        float shipPartZ = -0.2f;

        // Instantiate some type of ship 4 example:
        // For each ship type there should be specific game object...
        var cannonGO = Instantiate(Resources.Load<GameObject>("Prefabs/ShipPart"), transform);
        cannonGO.name = "ShipPart/Cannon";
        cannonGO.transform.parent = gameObject.transform;
        cannonGO.transform.localPosition = new Vector3(-5, -2, shipPartZ);

        var engineRoomGO = Instantiate(Resources.Load<GameObject>("Prefabs/ShipPart"), transform);
        engineRoomGO.name = "ShipPart/EngineRoom";
        engineRoomGO.transform.parent = gameObject.transform;
        engineRoomGO.transform.localPosition = new Vector3(-5, 0, shipPartZ);

        var hullGO = Instantiate(Resources.Load<GameObject>("Prefabs/ShipPart"), transform);
        hullGO.name = "ShipPart/Hull";
        hullGO.transform.parent = gameObject.transform;
        hullGO.transform.localPosition = new Vector3(2, 0, shipPartZ);

        var kitchenGO = Instantiate(Resources.Load<GameObject>("Prefabs/ShipPart"), transform);
        kitchenGO.name = "ShipPart/Kitchen";
        kitchenGO.transform.parent = gameObject.transform;
        kitchenGO.transform.localPosition = new Vector3(0, 2, shipPartZ);

        var sailsGO = Instantiate(Resources.Load<GameObject>("Prefabs/ShipPart"), transform);
        sailsGO.name = "ShipPart/Sails";
        sailsGO.transform.parent = gameObject.transform;
        sailsGO.transform.localPosition = new Vector3(4, 2, shipPartZ);

        var crowsNestGO = Instantiate(Resources.Load<GameObject>("Prefabs/ShipPart"), transform);
        crowsNestGO.name = "ShipPart/CrowsNest";
        crowsNestGO.transform.parent = gameObject.transform;
        crowsNestGO.transform.localPosition = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), shipPartZ);

        ShipParts = new List<ShipPart>
        {
            cannonGO.AddComponent<Cannon>(),
            engineRoomGO.AddComponent<EngineRoom>(),
            hullGO.AddComponent<Hull>(),
            kitchenGO.AddComponent<Kitchen>(),
            sailsGO.AddComponent<Sails>(),
            crowsNestGO.AddComponent<Sails>(),
        };
    }

    void Start()
    {
        //foreach (ShipPart sp in ShipParts)
        //{
        //    var _collider = sp.gameObject.AddComponent<BoxCollider2D>();
        //    _collider.size = new Vector2(20, 20);
        //}

        // TODO: this is temp, depending on crew member size compared to ship part count
        foreach (var shipPart in ShipParts)
        {
            shipPart.InitShipPart(this);
        }

        CrewMembers = new List<CrewMember>();
        foreach (var crewMemberConfig in GameFileConfig.GetInstance().ShipConfig.ShipCrew)
        {
            // Create instance of a Pirate
            var crewMemberGO = Instantiate(Resources.Load<GameObject>("Prefabs/Pirate"), transform);
            crewMemberGO.name = "CrewMembers /PlayerCharacter-" + crewMemberConfig.PirateName;

            // TODO: read positions of crew members relative to boat
            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
            crewMemberGO.transform.localPosition = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), -0.25f);
            crewMemberGO.GetComponent<BoxCollider2D>().isTrigger = true;

            // Add component
            var component = crewMemberGO.AddComponent<CrewMember>();

            // Init ship parts, name, color and ship part
            component.Init(crewMemberConfig.PirateName);
            component.Ship = this;
            
            //bool fAssigned = false;
            //while (!fAssigned)
            //{
            //    try
            //    {
            //        component.CurrentShipPart = GetRandomLiveShipPart();
            //        AssignCrewMember(component, component.CurrentShipPart);
            //        fAssigned = true;
            //    }
            //    catch { }
            //}

            CrewMembers.Add(component);
        }
    }

    internal void OnCrewMemberSelected(CrewMember crewMember)
    {
        SelectedCrewMember = crewMember;
    }

    internal void ProcessMoveEnd()
    {
        Debug.Log("Ship processing move end");
        Inventory.TryRemoveAmountOfFood(CalculateFoodConsumptionBetweenTwoPoints());
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

    /// <summary>
    /// Default food consumption is food consumption when boat is going only by wind
    /// </summary>
    /// <returns></returns>
    public int CalculateDefaultFoodConsumption()
    {
        int foodConsumption = 0;
        
        // Default food consumption
        foodConsumption = foodConsumption + CrewMembers.Select(crewMember => crewMember.ResourceConsumption).Sum();

        // People under plague eat more food
        foodConsumption = foodConsumption + 
            CrewMembers.Where(crewMember => crewMember.IsUnderPlague).Count() * GameConfig.Instance.PlagueResourceConsumptionIncrement;

        // People that and are rowing in are in engine room eat more food
        foodConsumption = foodConsumption +
            CrewMembers.Where(crewMember => crewMember.CurrentShipPart is EngineRoom).Count() * GameConfig.Instance.RowingActionFoodConsumptionIncrement;

        return foodConsumption;
    }

    public int CalculateBoatSpeed()
    {
        int boatSpeed = GameConfig.Instance.InitialShipSpeed;

        // ShipParts slows us down
        int shipPartsWeight = ShipParts
            .Select(shipPart => shipPart.Weight).Sum();

        boatSpeed -= shipPartsWeight;

        double rowingSpeedIncrement =
            CrewMembers
            .Where(crewMember => crewMember.CurrentShipPart is EngineRoom)
            // TODO: v-milast check if rowing is valid
            .Select(crewMember => crewMember.GetAttribute("Rowing"))
            .Where(attribute => attribute != null)
            .Select(attribute => attribute.AttributeValue)
            .Sum();

        boatSpeed += (int)Math.Floor(rowingSpeedIncrement+GetScoutingBonus());

        return boatSpeed;
    }

    public uint CalculateFoodConsumptionBetweenTwoPoints()
    {
        int defaultFoodConsumption = CalculateDefaultFoodConsumption();
        int boatSpeed = CalculateBoatSpeed();

        return (uint)Math.Floor(1.0 * defaultFoodConsumption / (boatSpeed  / 100.0));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(name + " Game Object Clicked!");

        GameManager.Instance.UiController.OnShipSelected();
        
        if (eventData.button == PointerEventData.InputButton.Right && SelectedCrewMember != null)
        {
            SelectedCrewMember.MoveTo(eventData.pointerCurrentRaycast.worldPosition);
        }
    }

    public double GetScoutingBonus()
    {
        return CrewMembers
            .Where(crewMember => crewMember.CurrentShipPart is EngineRoom)
            // TODO: milast check if scouting is ok
            .Select(crewMember => crewMember.GetAttribute("Scouting"))
            .Where(attribute => attribute != null)
            .Select(attribute => attribute.AttributeValue)
            .Sum();
    }

    public ShipPart GetRandomLiveShipPart()
    {
        // Count available ship parts
        int countParts = 0;
        
        foreach (var shipPart in ShipParts)
        {
            if (!shipPart.IsDestroyed)
            {
                countParts++;
            }
        }

        int chosenShipPart = UnityEngine.Random.Range(0, countParts-1);

        var returnPart = new System.Object();

        foreach (var shipPart in ShipParts)
        {
            if (!shipPart.IsDestroyed)
            {
                if (chosenShipPart == 0)
                {
                    returnPart = shipPart;
                    break;
                }

                chosenShipPart--;
            }
        }

        return (ShipPart) returnPart;
    }
}