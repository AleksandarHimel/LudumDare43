using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ship : MonoBehaviour, IPointerClickHandler
{
    private List<ShipPart> ShipParts { get; set; }
    public IEnumerable<ShipPart> DestroyedShipParts { get { return ShipParts.Where(cm => cm.IsDestroyed); } }
    public IEnumerable<ShipPart> FunctioningShipParts { get { return ShipParts.Where(cm => !cm.IsDestroyed); } }
    public List<CrewMember> CrewMembers { get; set; }
    public IEnumerable<CrewMember> DeceasedCrewMembers { get { return CrewMembers.Where(cm => cm.IsDead); } }
    public IEnumerable<CrewMember> AliveCrewMembers { get { return CrewMembers.Where(cm => !cm.IsDead); } }

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

        bool needToUpdateMapChoices = crew.CurrentShipPart is CrowsNest || part is CrowsNest;
        crew.CurrentShipPart = part;

        if (needToUpdateMapChoices)
        {
            GameManager.Instance.UiController.UpdateChoices(GameManager.Instance.MapManager.GetPossibleDestinations());
            GameManager.Instance.UiController.OnChoiceChanged(0);
        }
    }

    // Use this for initialization
    void Awake()
    {
        Inventory = ScriptableObject.CreateInstance<ShipInventory>();
        Inventory.InitialiseResources(GameConfig.Instance.InitialFoodCount, GameConfig.Instance.InitialWoodCount);
        CrewMembers = new List<CrewMember>();
    }

    void Start()
    {
        var cannonGO = GameObject.Find("ShipPart/Cannon");
        var oarsGO = GameObject.Find("ShipPart/Oars");
        // var hullGO = GameObject.Find("ShipPart/Hull");
        var kitchenGO = GameObject.Find("ShipPart/Kitchen");
        var sailsGO = GameObject.Find("ShipPart/Sails");
        var crowsNestGO = GameObject.Find("ShipPart/CrowsNest");

        ShipParts = new List<ShipPart>
        {
            cannonGO.AddComponent<Cannon>(),
            oarsGO.AddComponent<Oars>(),
            // hullGO.AddComponent<Hull>(), // no hull for now
            kitchenGO.AddComponent<Kitchen>(),
            sailsGO.AddComponent<Sails>(),
            crowsNestGO.AddComponent<CrowsNest>(),
        };

        foreach (var shipPart in ShipParts)
        {
            shipPart.InitShipPart(this);
        }

        foreach (var crewMemberConfig in GameFileConfig.GetInstance().ShipConfig.ShipCrew)
        {
            // Create instance of a Pirate
            var crewMemberGO = Instantiate(Resources.Load<GameObject>("Prefabs/Pirate"), transform);
            crewMemberGO.name = "CrewMembers /PlayerCharacter-" + crewMemberConfig.PirateName;

            // Add component
            var component = crewMemberGO.AddComponent<CrewMember>();

            // Init ship parts, name, color and ship part
            component.Init(crewMemberConfig.PirateName);
            component.Ship = this;

            crewMemberGO.GetComponent<BoxCollider2D>().isTrigger = true;

            CrewMembers.Add(component);
        }
    }

    internal void OnCrewMemberSelected(CrewMember crewMember)
    {
        SelectedCrewMember = crewMember;
        GameManager.Instance.UiController.OnCrewMemberSelected(crewMember);
    }

    internal void ResetCrewMemberSelection()
    {
        SelectedCrewMember = null;
        GameManager.Instance.UiController.OnCrewMemberSelected(null);
    }

    internal void OnCrewMemberKilled(CrewMember crewMember)
    {
        if (SelectedCrewMember == crewMember)
        {
            ResetCrewMemberSelection();
        }
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
        
        // Sum up food consumption
        foodConsumption = foodConsumption + CrewMembers.Select(crewMember => crewMember.GetResourceConcuption()).Sum();

        return foodConsumption;
    }

    public int CalculateBoatSpeed()
    {
        double sailingFactor = CrewMembers
                                .Where(crewMember => crewMember!= null && crewMember.CurrentShipPart != null && crewMember.CurrentShipPart is Sails)
                                .Select(crewMember => crewMember.GetAttribute("Sailing") == null ? 1.0 : (double) crewMember.GetAttribute("Sailing").AttributeValue)
                                .DefaultIfEmpty(1.0)
                                .Average();
        
        //int baseSpeed = GameManager.Instance.CalculateDistanceByRiskiness(GameManager.Instance.DesiredRiskiness + 1);
        double boatSpeed = sailingFactor * GameManager.Instance.CalculateDistanceByRiskiness(GameManager.Instance.DesiredRiskiness);

        // ShipParts slows us down
        /*
          int shipPartsWeight = ShipParts
            .Select(shipPart => shipPart.Weight).Sum();
            
        boatSpeed -= shipPartsWeight;
        */

        double rowingSpeedIncrement =
            CrewMembers
            .Where(crewMember => crewMember.CurrentShipPart is Oars)
            // TODO: v-milast check if rowing is valid
            .Select(crewMember => crewMember.GetAttribute("Rowing"))
            .Where(attribute => attribute != null)
            .Select(attribute => attribute.AttributeValue)
            .Sum();

        boatSpeed += rowingSpeedIncrement + GetScoutingBonus();

        return (int)Math.Floor(boatSpeed);
    }

    public uint CalculateFoodConsumptionBetweenTwoPoints()
    {
        int defaultFoodConsumption = CalculateDefaultFoodConsumption();
        //int boatSpeed = CalculateBoatSpeed();

        return (uint)defaultFoodConsumption;
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
        return AliveCrewMembers
            .Where(crewMember => crewMember.CurrentShipPart is CrowsNest)
            // TODO: milast check if scouting is ok
            .Select(crewMember => crewMember.GetAttribute("Scouting"))
            .Where(attribute => attribute != null)
            .Select(attribute => attribute.AttributeValue)
            .Sum();
    }

    public int GetCannonBonus()
    {
        return (int)AliveCrewMembers
            .Where(crewMember => crewMember.CurrentShipPart is Cannon)
            // TODO: milast check if cannon is ok
            .Select(crewMember => crewMember.GetAttribute("Cannon"))
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

        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
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