using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    /// <summary>
    /// Mouse position represented relative to display 
    /// </summary>
    public Vector3 OnScreenMousePosition;

    /// <summary>
    /// Mouse position relative to world
    /// </summary>
    public Vector3 InWorldMousePosition;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update ()
	{
	    OnScreenMousePosition = Input.mousePosition;
        
        // TODO: missing camera handle here
        // InWorldMousePosition = ...;
	}
}
