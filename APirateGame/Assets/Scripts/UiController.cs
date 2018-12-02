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
    public Text StageText;
    public Dropdown PathChoice;
    Dropdown.DropdownEvent ChoiceChangedEvent;
    public Text EventInfo;
    public GameObject EventCanvas;

    // Maps option choice to riskiness
    public Dictionary<string, int> OptionsDictionary = new Dictionary<string, int>();


    void Awake()
    {
        ChoiceChangedEvent = new Dropdown.DropdownEvent();
        ChoiceChangedEvent.AddListener(x => OnChoiceChanged(x));
        PathChoice.onValueChanged = ChoiceChangedEvent;
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
        string text = string.Format("Name: {0}\nHealth: {1}{2}", member.PirateName, member.Health, member.IsUnderPlague ? "\nPLAUGEEEE" : "");

        foreach (string attributeName in member.AttributeNames)
        {
            text += string.Format("\n{0}: {1}", attributeName,( member.GetAttribute(attributeName) == null) ? "\u2620" : member.GetAttribute(attributeName).AttributeValue.ToString());
        }

        Debug.Log(text);
        SelectedItemDetailsTextBox.text = text;
    }

    internal void OnShipPartSelected(ShipPart shipPart)
    {
        string text = string.Format("Name: {0}\nHealth: {1}/{2}", shipPart.name, shipPart.Health, shipPart.MaxHealth);

        var crewMembers = shipPart.PresentCrewMembers;

        text += string.Format("\nCrew members:{0}/{1}", crewMembers.Count(), shipPart.MaxNumberOfCrewMembers);

        if (crewMembers.Count() > 0)
        {
            text += "\n" + crewMembers.Select(c => c.PirateName).Aggregate((c1, c2) => c1 + ", " + c2);
        }

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
        if (GameManager.Instance.Ship.GetScoutingBonus() > 0)
        {
            foreach (MapNodeInformation nodeInfo in nodesInformation)
            {
                string optionText = string.Format("{0} pts: {1}",
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
        }
        else
        {
            string optionText = "We cannot see where we are heading! We might end up anywhere!";
            Dropdown.OptionData optionData = new Dropdown.OptionData(optionText);
            OptionsDictionary[optionText] = -1;
            options.Add(optionData);
        }   

        PathChoice.AddOptions(options);
    }

    public int GetActiveRiskiness()
    {
        return OptionsDictionary[PathChoice.options[PathChoice.value].text];
    }

    public void OnShipSelected()
    {
        SelectedItemDetailsTextBox.text = "That's a pretty cool ship";
    }
}
