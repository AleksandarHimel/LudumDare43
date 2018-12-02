﻿using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrewMember : MonoBehaviour, IPointerClickHandler {

    public ShipPart CurrentShipPart;
    public int Health;
    public bool IsUnderPlague;
    public int ResourceConsumption;
    public string Name;

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

        foreach(var atr in ShipConfig.GetInstance().GetAttributesForCrewMember("Jack"))
        {
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
}
