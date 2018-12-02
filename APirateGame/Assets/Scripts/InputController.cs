using UnityEngine;
using UnityEngine.UI;

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

    public Button MoveEndButton;

	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
	{
	    OnScreenMousePosition = Input.mousePosition;
        
        // TODO: missing camera handle here
        // InWorldMousePosition = ...;
	}
}
