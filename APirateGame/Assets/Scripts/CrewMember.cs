using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrewMember : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler {

    public ShipPart CurrentShipPart;
    public int Health;
    public bool IsUnderPlague = false;
    public int ResourceConsumption = 10;
    public float BarfingProbability = 0.004f;
    public string PirateName;
    public float CrewMemberZPosition = -0.2f;
    public Vector2 CrewMemberMovingBoundingBox = new Vector2(0.1f, 0.1f);
    public Barf PirateBarf;

    private Dictionary<string, CrewMemberAttribute> attributes = new Dictionary<string, CrewMemberAttribute>();

    public bool IsMoving;
    private Vector3 expectedPosition;

    public bool IsDead { get; private set; }

    public IEnumerable<string> AttributeNames { get { return attributes.Keys; } }

    public Ship Ship;

    private Vector3? dragPositionStart = null;
    private Vector2? sizeStart = null;

    private static int deceasedCount = 0;

    public static string[] CrewMemberColors =
    {
        "red",
        "green",
        "blue"
    };

    public void Init(string pirateName)
    {
        PirateName = pirateName;
        Assets.Scripts.Configuration.CrewMemberConfig pirate =
            GameFileConfig.GetInstance().ShipConfig.GetCrewMembers()[pirateName];
        foreach (var atrConfig in pirate.AttributesArray)
        {
            CrewMemberAttribute atr = CrewMemberAttribute.CreateAttribute(atrConfig.Name, atrConfig.Value);
            this.attributes.Add(atr.AttributeName, atr);
        }

        var sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = Resources.Load<Sprite>(string.Format("Sprites/Pirate {0}", pirate.Color));

        

        var shipPartObject = GameObject.Find("ShipPart/" + pirate.InitShipPart);
        this.CurrentShipPart = shipPartObject.GetComponent<ShipPart>();
        
        Vector3 v3 = shipPartObject.transform.localPosition;
        v3.z = CrewMemberZPosition;
        v3.x += 0.1f;
        gameObject.transform.localPosition = v3;

        PirateBarf = new Barf(transform);

        Debug.Log(string.Format("{0} : {1} [{2}]", PirateName, pirate.Color, pirate.InitShipPart));
    }

    void Start()
    {
        Health = 100;
        IsDead = false;
    }

    void FixedUpdate()
    {
        if (IsMoving)
        {
            gameObject.GetComponent<Rigidbody2D>().MovePosition(expectedPosition);
            IsMoving = false;
        }
        else
        {
      
            if (!IsDead && !PirateBarf.IsBarfing && IsUnderPlague && UnityEngine.Random.Range(0f, 1.0f) < BarfingProbability && GameManager.Instance.GameState.State == GameState.EGameState.PlayerTurn)
            {
                PirateBarf.StartBarfing();
            }
            PirateBarf.UpdateBarf(); 
            
        }
    }

    public int GetResourceConcuption()
    {
        if (IsDead)
        {
            return 0;
        }

        int result = ResourceConsumption;

        // People that and are affected by plague eat more food
        result += IsUnderPlague ? GameConfig.Instance.PlagueResourceConsumptionIncrement : 0;
        // People who row need more food
        result += this.CurrentShipPart is Oars ? GameConfig.Instance.RowingActionFoodConsumptionIncrement : 0;

        return result;
    }

    public void ReduceHealth(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            KillCrewMember();
        }
    }

    private void KillCrewMember()
    {
        IsDead = true;
        Ship.OnCrewMemberKilled(this);
        
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        var screamManagerGO = GameObject.Find("ScreamManager");
        screamManagerGO.GetComponent<AudioSource>().Play();

        deceasedCount++;
        var deadPirateGO = GameObject.Find("BackgroundObjects/skeleton" + deceasedCount);
        deadPirateGO.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
    }

    public CrewMemberAttribute GetAttribute(string attributeName)
    {
        CrewMemberAttribute retValue = null;
        attributes.TryGetValue(attributeName, out retValue);

        return retValue;
    }

    public void PlagueThisGuy()
    {
        IsUnderPlague = true;  
    }

    private void SelectCrewMember()
    {
        if (!IsDead)
        {
            Ship.OnCrewMemberSelected(this);
            
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(name + " Game Object Clicked!");
        SelectCrewMember();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SelectCrewMember();
        GameManager.Instance.UiController.ClearStatusBar();

        Debug.Log(name + " game object mouse down");
        var pirateCollider = gameObject.GetComponent<BoxCollider2D>();

        if (dragPositionStart == null)
        {
            dragPositionStart = transform.position;
        }
        if (sizeStart == null)
        {
            sizeStart = pirateCollider.size;
        }

        // Need to reduce pirates bounding box y axis to avoid 
        // snapping his center too far from the ship part
        pirateCollider.size = CrewMemberMovingBoundingBox;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var pirateCollider = gameObject.GetComponent<BoxCollider2D>();

        foreach (ShipPart sp in Ship.FunctioningShipParts)
        {
            var collider = sp.gameObject.GetComponent<BoxCollider2D>();

            if(collider.IsTouching(pirateCollider))
            {
                try
                {
                    Ship.AssignCrewMember(this, sp);

                    Debug.Log("Assigned " + this.name + " to " + sp.FriendlyName);
                    dragPositionStart = null;
                    pirateCollider.size = sizeStart.Value;
                    sizeStart = null;

                    return;
                }
                catch (Exception)
                {
                    Debug.Log("Could not assign " + this.name + " to " + sp.FriendlyName + " (max ppl: " + sp.MaxNumberOfCrewMembers + ")");
                    MoveTo(dragPositionStart.Value);
                    dragPositionStart = null;
                    pirateCollider.size = sizeStart.Value;
                    sizeStart = null;

                    GameManager.Instance.UiController.OnFailedLocationChange(this, sp);

                    return;
                }
            }
        }

        // Are we throwing pirate overboard?
        var watters = GameObject.FindGameObjectsWithTag("Water");
        foreach (var w in watters)
        {
            var waterCollider = w.GetComponent<BoxCollider2D>();
            if (waterCollider.IsTouching(pirateCollider))
            {
                Debug.Log(string.Format("OMG they killed {0}! You bastards!", PirateName));
                Ship.ResetCrewMemberSelection();
                KillCrewMember();
                dragPositionStart = null;

                return;
            }
        }

        Debug.Log(this.name + " moved to nowhere - returning to " + this.CurrentShipPart.FriendlyName);

        MoveTo(dragPositionStart.Value);
        dragPositionStart = null;
        pirateCollider.size = sizeStart.Value;
        sizeStart = null;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log(name + string.Format(" being dragged: {0} {1}", eventData.position.x, eventData.position.y));
        
        var worldPoint = Camera.main.ScreenToWorldPoint(
            new Vector3(eventData.position.x, eventData.position.y, gameObject.transform.position.z - Camera.main.transform.position.z));
        var pendingPosition = new Vector3(worldPoint.x, worldPoint.y, gameObject.transform.position.z);

        MoveTo(new Vector2(pendingPosition.x, pendingPosition.y));
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.gameObject.name + " collided with " + other.gameObject.name);

        if (other.gameObject.GetComponent<ShipPart>() != null)
        {
            Debug.Log(this.gameObject.name + " trying to enter " + other.gameObject.name);
        }
    }

    private bool IsWithinBoundaries(ShipPart sp)
    {
        return true;
    }

    internal void MoveTo(Vector3 worldPosition)
    {
        IsMoving = true;
        expectedPosition = worldPosition;
    }
}
