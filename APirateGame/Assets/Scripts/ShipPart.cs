using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipPart : MonoBehaviour {

    public uint MaxNumberOfCrewMembers { get; protected set; }

    public List<CrewMember> PresentCrewMembers { get; protected set; }

    public uint Health { get; protected set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
