using Assets.Events;
using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour {

        public Ship Ship;
        public int Points = 0;
        public EventManager EventManager;
        public PlayerController PlayerController;
        public InputController InputController;
        public MapManager MapManager;
        public UiController UiController;
        public AudioController AudioController;
        public GameConfig GameConfig;
        public int DesiredRiskiness;

        [Header("Health Settings")]
        public GameState GameState;
        public ShipInventory ShipInventory;

        public SpriteRenderer nightBringerSprite;

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
        public float FadeTime = 3f;
        private float t = 0.0f;

        private void Awake()
        {
            if (_instance == this)
            {
                return;
            }

            _instance = this;

            _gameManagerGameObject = new GameObject("_gameManagerGameObject");
            PlayerController = _gameManagerGameObject.AddComponent<PlayerController>();
            EventManager = EventManager.Instance;
            GameState = ScriptableObject.CreateInstance<GameState>();
            MapManager = MapManager.Instance;
            AudioController = _gameManagerGameObject.AddComponent<AudioController>();
            GameConfig = _gameManagerGameObject.AddComponent<GameConfig>();

            GameObject _nightBringer = new GameObject("_nightBringer");
            _nightBringer.transform.position = new Vector3(0, 0, -1);
            nightBringerSprite = _nightBringer.AddComponent<SpriteRenderer>();
            nightBringerSprite.sprite = Resources.Load<Sprite>("Sprites/Background");
            nightBringerSprite.color = new Color(0,0,0,0);
        }

        // Use this for initialization
        void Start() {

            // TODO merge

            // var shipGameObject = GameObject.Find("ShipGO");
            // Ship = shipGameObject.GetComponent<Ship>();
            // Ship.Inventory = ShipInventory;
            // var shipGameObject = new GameObject("ShipGameObject");
            // Ship = shipGameObject.AddComponent<Ship>();
            UiController.UpdateChoices(MapManager.GetPossibleDestinations());
            UiController.ResourcesTextBox.text = string.Format("Resources: food {0}", Ship.Inventory.Food);
            UiController.Points.text = string.Format("Points: {0}", Points);

            InputController.MoveEndButton.onClick.AddListener(ProcessMoveEnd);

            // AssetDatabase.CreateAsset(GameState, "Assets/ScriptableObjectsStatic/GameStateStatic.asset");
            // AssetDatabase.SaveAssets();

            SetIsUserTurn(true);
        }
	
        public void SetIsUserTurn(bool newValue)
        {
            if (GameState.State == GameState.EGameState.PlayerTurn || GameState.State == GameState.EGameState.ComputerTurn)
            {
                GameState.State = newValue ? GameState.EGameState.PlayerTurn : GameState.EGameState.ComputerTurn;

                if (GameState.State == GameState.EGameState.PlayerTurn)
                {
                    ProcessUserTurnStart();
                }
            }
        }

        public void ProcessMoveEnd()
        { 
            // Fade out background music
            AudioController.FadeOutBackgroundMusic();

            Ship.ProcessMoveEnd();
            UiController.ResourcesTextBox.text = string.Format("Resources: food {0}", Ship.Inventory.Food);
            UiController.Points.text = string.Format("Points: {0}", Points);

            GameState.State = GameState.EGameState.BringTheNight; 
        }

        void Update()
        {
            if (GameState.State == GameState.EGameState.ComputerTurn)
            {
                MapManager.GoToNextDestination(DesiredRiskiness);
                // Handle
                var gameplayEvent = MapManager.GetCurrentNode().NodeEvent;
                gameplayEvent.Execute(Ship);
                UiController.UpdateChoices(MapManager.GetPossibleDestinations());

                // Execute Sfx

                // Show user info message

                // todo Wait for user to confirm
                // GameState.State = GameState.EGameState.WaitForUserEventResultConfirm;
                GameState.State = GameState.EGameState.BringTheDawn;
            }
            if (GameState.State == GameState.EGameState.BringTheDawn)
            { 
                while (t < 1.0f)
                {
                    nightBringerSprite.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), t);
                    t += Time.deltaTime / FadeTime;
                    return;
                }
                t = 0.0f;
                SetIsUserTurn(true);
            }
            if (GameState.State == GameState.EGameState.BringTheNight)
            {
                Debug.Log(t);
                while (t < 1.0f)
                {
                    nightBringerSprite.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), t);
                    t += Time.deltaTime / FadeTime;
                    return;
                }
                t = 0.0f;
                SetIsUserTurn(false);
            }
        }

        public void GameOver()
        {
            GameState.State = GameState.EGameState.GameOver;
        }

        public void Victory()
        {
            GameState.State = GameState.EGameState.Victory;
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