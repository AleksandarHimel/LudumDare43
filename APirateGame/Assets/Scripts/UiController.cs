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
    public Text StatusBar;
    public Text Points;
    public Text GameOverText;
    public Text VictoryText;
    public Text StageText;
    public Dropdown PathChoice;
    Dropdown.DropdownEvent ChoiceChangedEvent;
    public GameObject EventCanvas;
    public ScrollRect ScrollRect;

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
        string text;
        if (member == null)
        {
            text = "No Pirate selected\nSelect or move one\nalthough he may not like it...";
        }
        else
        {
            text = string.Format("Name: {0}\nHealth: {1}{2}", member.PirateName, member.Health, member.IsUnderPlague ? "\nPLAUGEEEE" : "");

            foreach (string attributeName in member.AttributeNames)
            {
                text += string.Format("\n{0}: {1}", attributeName, (member.GetAttribute(attributeName) == null) ? "\u2620" : member.GetAttribute(attributeName).AttributeValue.ToString());
            }
        }

        //Debug.Log(text);
        SelectedItemDetailsTextBox.text = text;
    }

    public void OnShipPartSelected(ShipPart shipPart)
    {
        string name = shipPart.name.Replace("ShipPart/", ""); // TODO: get name in a less hacky way
        string text = string.Format("Name: {0}\nHealth: {1}/{2}", name, shipPart.Health, shipPart.MaxHealth);

        var crewMembers = shipPart.PresentCrewMembers;

        text += string.Format("\nCrew members:{0}/{1}", crewMembers.Count(), shipPart.MaxNumberOfCrewMembers);

        if (crewMembers.Count() > 0)
        {
            text += "\n" + crewMembers.Select(c => c.PirateName).Aggregate((c1, c2) => c1 + ", " + c2);
        }

        SelectedItemDetailsTextBox.text = text;

        StatusBar.color = Color.black;
        StatusBar.text = shipPart.Description;
    }

    public void OnFailedLocationChange(CrewMember crewMember, ShipPart shipPart)
    {
        StatusBar.color = Color.red;
        StatusBar.text = string.Format("Can't move {0} to {1}, it is {2}", crewMember.PirateName, shipPart.name,
            (shipPart.IsDestroyed ? "destroyed" : "full"));
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
                string optionText = string.Format("Move at least {0} miles: {1}",
                      GameManager.Instance.CalculateDistanceByRiskiness(nodeInfo.Riskiness),

                      String.Join(" or ",
                      nodeInfo
                        .PossibleEncounter
                        .Select(possibleEncounter => EventManager.Instance.GetEventDescription(possibleEncounter)).ToArray())
                    );
                Debug.Log(optionText);
                Dropdown.OptionData optionData = new Dropdown.OptionData(optionText);

                OptionsDictionary[optionText] = nodeInfo.Riskiness;
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
        StatusBar.text = "";
    }
}
