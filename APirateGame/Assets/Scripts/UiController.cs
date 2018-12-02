using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Provides a home for various UI doodads. Should mostly be dumb, with all the logic in GameManager.
/// </summary>
public class UiController : MonoBehaviour
{
    public Text ResourcesTextBox;
    public Text SelectedItemDetailsTextBox;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnCrewMemberSelected(CrewMember member)
    {
        string text = string.Format("Health: {0}{1}", member.Health, member.IsUnderPlague ? "\nPLAUGEEEE" : "");

        foreach (string attributeName in CrewMemberAttribute.s_allowedAttributes.Keys)
        {
            text += string.Format("\n{0}: {1}", attributeName, member.GetAttribute(attributeName));
        }

        Debug.Log(text);
        SelectedItemDetailsTextBox.text = text;
    }

    public void OnShipSelected()
    {
        SelectedItemDetailsTextBox.text = "That's a pretty cool ship";
    }
}
