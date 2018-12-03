using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrewMember : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler {

    public ShipPart CurrentShipPart;
    public int Health;
    public bool IsUnderPlague;
    public int ResourceConsumption = 10;
    public string PirateName;

    private Dictionary<string, CrewMemberAttribute> attributes = new Dictionary<string, CrewMemberAttribute>();

    public bool IsMoving;
    private Vector3 expectedPosition;

    public bool IsDead { get; private set; }

    public IEnumerable<string> AttributeNames { get { return attributes.Keys; } }

    public Ship Ship;

    private Vector3? dragPositionStart = null;
    private Vector2? sizeStart = null;

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
        v3.z = -0.2f;
        gameObject.transform.localPosition = v3;

        Debug.Log(string.Format("{0} : {1} [{2}]", PirateName, pirate.Color, pirate.InitShipPart));
    }

    void Start()
    {
        Health = 10;
        IsDead = false;
    }

    void FixedUpdate()
    {
        if (IsMoving)
        {
            gameObject.GetComponent<Rigidbody2D>().MovePosition(expectedPosition);
            IsMoving = false;
        }
    }

    public void ReduceHealth(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            IsDead = true;
            Ship.CrewMembers.Remove(this);
            Ship.DeceasedCrewMembers.Add(this);
        }
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
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(name + " Game Object Clicked!");

        GameManager.Instance.UiController.OnCrewMemberSelected(this);

        Ship.OnCrewMemberSelected(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(name + " game object mouse down");
        var pirateCollider = gameObject.GetComponent<BoxCollider2D>();

        if (dragPositionStart == null)
        {
            dragPositionStart = transform.position;
            sizeStart = pirateCollider.size;
        }

        var size = pirateCollider.size;
        size.y = 0.1f;
        pirateCollider.size = size;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var pirateCollider = gameObject.GetComponent<BoxCollider2D>();

        foreach (ShipPart sp in Ship.ShipParts)
        {
            var collider = sp.gameObject.GetComponent<BoxCollider2D>();

            if(collider.IsTouching(pirateCollider))
            {
                try
                {
                    Ship.AssignCrewMember(this, sp);

                    Debug.Log("Assigned " + this.name + " to " + sp.name);
                    dragPositionStart = null;
                    pirateCollider.size = sizeStart.Value;

                    return;
                }
                catch (Exception)
                {
                    Debug.Log("Could not assign " + this.name + " to " + sp.name + " (max ppl: " + sp.MaxNumberOfCrewMembers + ")");
                    MoveTo(dragPositionStart.Value);
                    dragPositionStart = null;
                    pirateCollider.size = sizeStart.Value;

                    GameManager.Instance.UiController.OnFailedLocationChange(this, sp);

                    return;
                }
            }
        }

        Debug.Log(this.name + " moved to nowhere - returning to " + this.CurrentShipPart.name);

        MoveTo(dragPositionStart.Value);
        dragPositionStart = null;
        pirateCollider.size = sizeStart.Value;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log(name + string.Format(" being dragged: {0} {1}", eventData.position.x, eventData.position.y));
        
        var worldPoint = Camera.main.ScreenToWorldPoint(
            new Vector3(eventData.position.x, eventData.position.y, gameObject.transform.position.z - Camera.main.transform.position.z));

        var pendingPosition = new Vector3(worldPoint.x, worldPoint.y, gameObject.transform.position.z);

        foreach (ShipPart sp in Ship.ShipParts)
        {
            if (this.IsWithinBoundaries(sp))
            {
                try
                {
                    Ship.AssignCrewMember(this, sp);
                    this.transform.position = pendingPosition;
                    
                    MoveTo(new Vector2(eventData.pointerCurrentRaycast.worldPosition.x, eventData.pointerCurrentRaycast.worldPosition.y));
                }
                catch (Exception)
                {
                    // could not assign - return back;
                }

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.gameObject.name + " collided with " + other.gameObject.name);

        if (other.gameObject.GetComponent<ShipPart>() != null)
        {
            Debug.Log(this.gameObject.name + " trying to enter " + other.gameObject.name);

            try
            {
                Ship.AssignCrewMember(this, other.gameObject.GetComponent<ShipPart>());
            }
            catch (Exception)
            {
                // could not assign - do something else
            }
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
