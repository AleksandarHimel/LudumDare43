﻿using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ShipPart : MonoBehaviour, IPointerClickHandler {

    protected Ship ParentShip;

    protected SpriteRenderer spriteRenderer;

    public uint MaxNumberOfCrewMembers { get; protected set; }

    public uint Health { get; protected set; }

    public bool IsDestroyed { get { return (Health == 0); } }

    public uint MaxHealth { get; protected set; }

    public int Weight { get; set; }

    public abstract void InitShipPart(Ship ship);

    public string FriendlyName { get { return name.Replace("ShipPart/", ""); } }

    public ShipPart(Ship parentShip)
    {
        ParentShip = parentShip;
    }

    public string Description { get; protected set; }

    /// <summary>
    /// Gets all crew members currently in this ship part.
    /// </summary>
    public IEnumerable<CrewMember> PresentCrewMembers
    {
        get { return ParentShip.AliveCrewMembers.Where(cm => cm.CurrentShipPart == this); }
    }

	// Use this for initialization
	void Awake ()
    {
        //spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        // TODO: add sprite
        // spriteRenderer.sprite = Resources.Load<Sprite>("Path");
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void TakeDamage(uint damage)
    {
        Health -= Math.Min(damage, Health);
		if (IsDestroyed)
		{
			Fire fire = new Fire(this);
			fire.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Fire");
		}
    }

    public abstract bool IsOnBottom();

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.UiController.OnShipPartSelected(this);
    }
}
