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

    void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();

        var random = new System.Random(System.DateTime.Now.Millisecond);
        var color = CrewMemberColors[random.Next(3)];
        sprite.sprite = Resources.Load<Sprite>(string.Format("Sprites/Pirate {0}", color));

        Debug.Log("Start Loading attributes");

        foreach(var atr in GameFileConfig.GetInstance().GetAttributesForCrewMember("Jack"))
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
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(name + string.Format(" being dragged: {0} {1}", eventData.position.x, eventData.position.y));
        
        var something = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.transform.position.z - gameObject.transform.position.z));

        this.gameObject.transform.position = new Vector3(something.x, something.y, gameObject.transform.position.z);
    }
}
