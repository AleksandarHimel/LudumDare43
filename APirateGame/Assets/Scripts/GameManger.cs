using Assets.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour {

    public Ship ship;

    public EventMgr eventManager;
    public MapManager mapManager;

    private bool isUserTurn;
    private int currentDay;

    // Use this for initialization
    void Start ()
    {
        // Create all important parts
        currentDay = 0;
        eventManager = new EventMgr();
        mapManager = new MapManager();
        SetIsUserTurn(true);
    }
	
    public void SetIsUserTurn(bool newValue)
    {
        isUserTurn = newValue;
        TurnStart();
    }

    private void TurnStart()
    {
        if (isUserTurn)
        {

            currentDay++;
        }

        ExecuteEncounters(ship);

        VerifyGameState();

        SetIsUserTurn(false);
    }

    private void VerifyGameState()
    {
        return;
    }

    private void ExecuteEncounters(Ship ship)
    {
        EventEnum eventEnum = mapManager.GetCurrentNode().Encounter;
        eventManager.GenerateEvent(eventEnum).Execute(ship);
    }
}
