using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrewMember : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IDragHandler {

    public ShipPart CurrentShipPart;
    public int Health;
    public bool IsUnderPlague;
    public int ResourceConsumption = 10;
    public string Name;
    public string Color;

    private Dictionary<string, CrewMemberAttribute> attributes = new Dictionary<string, CrewMemberAttribute>();

    public bool IsDead { get; private set; }

    public Ship ship;

    private Vector3? dragPositionStart = null;

    public static string[] CrewMemberColors =
    {
        "red",
        "green",
        "blue"
    };

    void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();

        var random = new System.Random(System.DateTime.Now.Millisecond);
        sprite.sprite = Resources.Load<Sprite>(string.Format("Sprites/Pirate {0}", Color));

        Debug.Log("Start Loading attributes");

        foreach(var atr in ShipConfig.GetInstance().GetAttributesForCrewMember(Name))
        {
            Debug.Log(string.Format("{0} : {1}", atr.AttributeName, atr.AttributeValue));
            this.attributes.Add(atr.AttributeName, atr);
        }

        Debug.Log("Finished Loading attributes");

        Health = 10;
        IsDead = false;
    }

    public void ReduceHealth(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            IsDead = true;
            ship.CrewMembers.Remove(this);
            ship.DeceasedCrewMembers.Add(this);
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
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(name + " game object mouse down");

        if (dragPositionStart == null)
        {
            dragPositionStart = transform.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragPositionStart = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(name + string.Format(" being dragged: {0} {1}", eventData.position.x, eventData.position.y));
        
        var worldPoint = Camera.main.ScreenToWorldPoint(
            new Vector3(eventData.position.x, eventData.position.y, Camera.main.transform.position.z - gameObject.transform.position.z));

        var pendingPosition = new Vector3(worldPoint.x, worldPoint.y, gameObject.transform.position.z);

        foreach (ShipPart sp in ship.ShipParts)
        {
            if (this.IsWithinBoundaries(sp))
            {
                try
                {
                    ship.AssignCrewMember(this, sp);
                    this.transform.position = pendingPosition;
                }
                catch (Exception)
                {
                    // could not assign - return back;
                }

            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(this.gameObject.name + " collided with " + collision.gameObject.name);

        if (collision.gameObject.GetComponent<ShipPart>() != null)
        {
            Debug.Log(this.gameObject.name + " trying to enter " + collision.gameObject.name);

            try
            {
                ship.AssignCrewMember(this, collision.gameObject.GetComponent<ShipPart>());
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
}
