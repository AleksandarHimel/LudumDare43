﻿using Assets.Events;
using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour {

        public Ship Ship;
        public EventManager EventManager;
        public PlayerController PlayerController;
        public InputController InputController;

        [Header("Health Settings")]
        public GameState GameState;
        public ShipInventory ShipInventory;

        private GameObject _gameManagerGameObject;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }

                return _instance;
            }
        }

        private static GameManager _instance;

        // Use this for initialization
        void Start() {
            if (_instance == this)
            {
                return;
            }

            _instance = this;

            _gameManagerGameObject = new GameObject("_gameManagerGameObject");
            PlayerController = _gameManagerGameObject.AddComponent<PlayerController>();
            InputController = _gameManagerGameObject.AddComponent<InputController>();
            EventManager = _gameManagerGameObject.AddComponent<EventManager>();
            GameState = ScriptableObject.CreateInstance<GameState>();

            var shipGameObject = GameObject.Find("ShipGO");
            Ship = shipGameObject.GetComponent<Ship>();
            Ship.Inventory = ShipInventory;
        }
	
        public void SetIsUserTurn(bool newValue)
        {
            GameState.State = newValue ? GameState.EGameState.PlayerTurn : GameState.EGameState.ComputerTurn;
        }

        void Update()
        {
            if (GameState.State == GameState.EGameState.ComputerTurn)
            {
                // Handle
                var gameplayEvent = EventManager.Instance.GetNextEvent();
                gameplayEvent.Execute(Ship);

                GameState.State = GameState.EGameState.PlayerTurn;
            }
        }

        private void VerifyGameState()
        {
            throw new NotImplementedException();
        }

        private void ExecuteEncounters(Ship ship)
        {
            
        }
    }
}
