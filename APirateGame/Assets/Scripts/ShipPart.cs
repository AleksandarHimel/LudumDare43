using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ShipPart : MonoBehaviour {

    protected readonly Ship ParentShip;

    public uint MaxNumberOfCrewMembers { get; protected set; }

    public uint Health { get; protected set; }

    public bool IsDestroyed { get { return (Health == 0); } }

    public uint MaxHealth { get; protected set; }

    public ShipPart(Ship parentShip)
    {
        ParentShip = parentShip;
    }

    /// <summary>
    /// Gets all crew members currently in this ship part.
    /// </summary>
    public IEnumerable<CrewMember> PresentCrewMembers
    {
        get { return ParentShip.CrewMembers.Where(cm => cm.CurrentShipPart == this); }
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void TakeDamage(uint damage)
    {
        Health -= Math.Min(damage, Health);
    }
}
