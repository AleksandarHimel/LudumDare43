﻿using Assets.Events;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour {

        public Ship Ship;
        public EventManager EventManager;
        public GameState GameState;
        public PlayerController PlayerController;
        public InputController InputController;

        // TODO: find a better home for this
        public Text ResourcesTextBox;

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
            // InputController = _gameManagerGameObject.AddComponent<InputController>();
            EventManager = _gameManagerGameObject.AddComponent<EventManager>();
            GameState = ScriptableObject.CreateInstance<GameState>();

            // var shipGameObject = new GameObject("ShipGameObject");
            // Ship = shipGameObject.AddComponent<Ship>();

            InputController.MoveEndButton.onClick.AddListener(ProcessMoveEnd);

            AssetDatabase.CreateAsset(GameState, "Assets/ScriptableObjectsStatic/GameStateStatic.asset");
            AssetDatabase.SaveAssets();
        }
	
        public void SetIsUserTurn(bool newValue)
        {
            GameState.State = newValue ? GameState.EGameState.PlayerTurn : GameState.EGameState.ComputerTurn;
        }

        public void ProcessMoveEnd()
        {
            Ship.ProcessMoveEnd();
            ResourcesTextBox.text = string.Format("Resources: food {0}, wood {1}", Ship.Inventory.Food, Ship.Inventory.WoodForFuel);
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
