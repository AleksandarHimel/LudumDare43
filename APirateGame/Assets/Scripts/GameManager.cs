using Assets.Events;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour {

        public Ship Ship;
        public EventManager EventManager;
        public PlayerController PlayerController;
        public InputController InputController;
        public MapManager MapManager;
        public UiController UiController;
        public AudioController AudioController;
        public GameConfig GameConfig;

        [Header("Health Settings")]
        public GameState GameState;
        public ShipInventory ShipInventory;

        // TODO: find a better home for this
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

        private void Awake()
        {
            if (_instance == this)
            {
                return;
            }

            _instance = this;

            _gameManagerGameObject = new GameObject("_gameManagerGameObject");
            PlayerController = _gameManagerGameObject.AddComponent<PlayerController>();
            EventManager = _gameManagerGameObject.AddComponent<EventManager>();
            GameState = ScriptableObject.CreateInstance<GameState>();
            MapManager = MapManager.Instance;
            AudioController = _gameManagerGameObject.AddComponent<AudioController>();
            GameConfig = _gameManagerGameObject.AddComponent<GameConfig>();
        }

        // Use this for initialization
        void Start() {

            // TODO merge

            // var shipGameObject = GameObject.Find("ShipGO");
            // Ship = shipGameObject.GetComponent<Ship>();
            // Ship.Inventory = ShipInventory;
            // var shipGameObject = new GameObject("ShipGameObject");
            // Ship = shipGameObject.AddComponent<Ship>();

            InputController.MoveEndButton.onClick.AddListener(ProcessMoveEnd);

            // AssetDatabase.CreateAsset(GameState, "Assets/ScriptableObjectsStatic/GameStateStatic.asset");
            // AssetDatabase.SaveAssets();

            SetIsUserTurn(true);
        }
	
        public void SetIsUserTurn(bool newValue)
        {
            GameState.State = newValue ? GameState.EGameState.PlayerTurn : GameState.EGameState.ComputerTurn;

            if (GameState.State == GameState.EGameState.PlayerTurn)
            {
                ProcessUserTurnStart();
            }
        }

        public void ProcessMoveEnd()
        { 
            // Fade out background music
            AudioController.FadeOutBackgroundMusic();

            Ship.ProcessMoveEnd();
            UiController.ResourcesTextBox.text = string.Format("Resources: food {0}, wood {1}", Ship.Inventory.Food, Ship.Inventory.WoodForFuel);

            SetIsUserTurn(false);
        }

        void Update()
        {
            if (GameState.State == GameState.EGameState.ComputerTurn)
            {
                // TODO Update Map

                // Handle
                var gameplayEvent = MapManager.GetCurrentNode().NodeEvent;
                gameplayEvent.Execute(Ship);

                SetIsUserTurn(true);
            }
        }

        public void ProcessUserTurnStart()
        {
            // Fade out background music
            AudioController.FadeInBackgroundMusic();
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
