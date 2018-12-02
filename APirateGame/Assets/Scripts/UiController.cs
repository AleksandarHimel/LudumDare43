using Assets.Events;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Provides a home for various UI doodads. Should mostly be dumb, with all the logic in GameManager.
/// </summary>
public class UiController : MonoBehaviour
{
    public Text ResourcesTextBox;
    public Text SelectedItemDetailsTextBox;
    public Text Points;
    public Text GameOverText;
    public Text VictoryText;
    public Dropdown PathChoice;
    Dropdown.DropdownEvent ChoiceChangedEvent;
    // Maps option choice to riskiness
    public Dictionary<string, int> OptionsDictionary = new Dictionary<string, int>();


    void Awake()
    {
        ChoiceChangedEvent = new Dropdown.DropdownEvent();
        ChoiceChangedEvent.AddListener(x => OnChoiceChanged(x));
    }

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

    public void OnChoiceChanged(int x)
    {
        GameManager.Instance.DesiredRiskiness = OptionsDictionary[PathChoice.options[x].text];
    }

    public void UpdateChoices(IEnumerable<MapNodeInformation> nodesInformation)
    {
        PathChoice.ClearOptions();
        OptionsDictionary.Clear();

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        foreach (MapNodeInformation nodeInfo in nodesInformation)
        {
            string optionText = string.Format("{0} pst: {1}",
                  nodeInfo.Riskiness + 1,
                  String.Join(" or ",
                  nodeInfo
                    .PossibleEncounter
                    .Select(possibleEncounter => EventManager.Instance.GetEventDescription(possibleEncounter)).ToArray())
                );
            Debug.Log(optionText);
            Dropdown.OptionData optionData = new Dropdown.OptionData(optionText);

            OptionsDictionary[optionText] = nodeInfo.Riskiness + 1;
            options.Add(optionData);
        }

        PathChoice.AddOptions(options);
    }

    public void OnShipSelected()
    {
        SelectedItemDetailsTextBox.text = "That's a pretty cool ship";
    }
}
