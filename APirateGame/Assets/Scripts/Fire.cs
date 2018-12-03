using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fire : MonoBehaviour {

    protected ShipPart ParentShipPart;

    protected SpriteRenderer spriteRenderer;

    public Fire(ShipPart parentShipPart)
    {
        ParentShipPart = parentShipPart;
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
}
