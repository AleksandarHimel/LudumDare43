using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipPart : MonoBehaviour {

    public uint MaxNumberOfCrewMembers { get; protected set; }

    public List<CrewMember> PresentCrewMembers { get; protected set; }

    public uint Health { get; protected set; }

    public bool IsDestroyed { get { return (Health == 0); } }

    public uint MaxHealth { get; protected set; }

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
