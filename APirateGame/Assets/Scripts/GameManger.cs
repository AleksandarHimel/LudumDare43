using Assets.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour {

    public Ship ship;
    public EventMgr eventManager;

    private bool isUserTurn;
    private int currentDay;

    // Use this for initialization
    void Start () {
        // Create all important parts
        currentDay = 0;
        ship = new Ship();
        eventManager = new EventMgr();
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
            return;
        }

        ExecuteEncounters(ship);

        VerifyGameState();

        SetIsUserTurn(false);
    }

    private void VerifyGameState()
    {
        throw new NotImplementedException();
    }

    private void ExecuteEncounters(Ship ship)
    {
        throw new NotImplementedException();
    }
}
