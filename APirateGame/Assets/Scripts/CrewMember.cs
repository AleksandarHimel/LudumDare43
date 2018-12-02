using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrewMember : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IDragHandler {

    public ShipPart CurrentShipPart;
    public int Health;
    public bool IsUnderPlague;
    public int ResourceConsumption;
    public string PirateName;

    private Dictionary<string, CrewMemberAttribute> attributes = new Dictionary<string, CrewMemberAttribute>();

    public bool IsDead { get; private set; }

    public Ship ship;

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

        Debug.Log(string.Format("{0} : {1} [{2}]", PirateName, pirate.Color, pirate.InitShipPart));
    }

    void Start()
    {
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
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(name + string.Format(" being dragged: {0} {1}", eventData.position.x, eventData.position.y));
        
        var something = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.transform.position.z - gameObject.transform.position.z));

        this.gameObject.transform.position = new Vector3(something.x, something.y, gameObject.transform.position.z);
    }
}
