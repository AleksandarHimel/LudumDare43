﻿using Assets.Events;
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
        public int Points;
        public int DistanceToHome;
        public EventManager EventManager;
        public PlayerController PlayerController;
        public InputController InputController;
        public MapManager MapManager;
        public UiController UiController;
        public AudioController AudioController;
        public GameConfig GameConfig;
        public int DesiredRiskiness = 0;

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
        private string gameOverReason;

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
        void Start()
        {
            Debug.Log("Random seed: " + UnityEngine.Random.seed);

            UiController.UpdateChoices(MapManager.GetPossibleDestinations());
            GameManager.Instance.UiController.OnChoiceChanged(0);

            DesiredRiskiness = UiController.GetActiveRiskiness();
            UiController.ResourcesTextBox.text = string.Format("Resources: food {0}", Ship.Inventory.Food);
            UiController.Points.text = string.Format("");

            InputController.MoveEndButton.onClick.AddListener(ProcessMoveEnd);
            InputController.AcceptEventResult.onClick.AddListener(ProcessUserAcceptedEventResult);
            UiController.OnCrewMemberSelected(null);

            // AssetDatabase.CreateAsset(GameState, "Assets/ScriptableObjectsStatic/GameStateStatic.asset");
            // AssetDatabase.SaveAssets();

            SetIsUserTurn(true);
        }
    
        public void SetIsUserTurn(bool newValue)
        {
            if (!(GameState.State == GameState.EGameState.Victory && GameState.State == GameState.EGameState.GameOver))
            {
                GameState.State = newValue ? GameState.EGameState.PlayerTurn : GameState.EGameState.ComputerTurn;

                if (GameState.State == GameState.EGameState.PlayerTurn)
                {
                    ProcessUserTurnStart();
                }
            }
        }

        public void UpdateDistance()
        {
            int distanceTravelled = Ship.CalculateBoatSpeed();
            // if we went with a low-risk roundabout path the total distance has increased
            
            Debug.Log(
                "Starting distance " + DistanceToHome +
                ", travelled " + distanceTravelled );

            DistanceToHome = Math.Max(0, DistanceToHome - distanceTravelled);
        }

        public int CalculateDistanceByRiskiness(int riskiness)
        {
            int distanceTravelled = GameConfig.Instance.InitialShipSpeed;

            if (riskiness < 0)
            {
                return distanceTravelled / 4;
            }

            riskiness += 1;
            // if we went with a low-risk roundabout path the total distance has increased
            int roundaboutPathPenalty = Math.Max(3 - riskiness, 0) * distanceTravelled / 4;

            return distanceTravelled - roundaboutPathPenalty;
        }

        public void ProcessMoveEnd()
        { 
            // Fade out background music
            AudioController.FadeOutBackgroundMusic();

            // Disable next buton and path pick
            InputController.MoveEndButton.gameObject.SetActive(false);
            UiController.PathChoice.gameObject.SetActive(false);

            Ship.ProcessMoveEnd();
            UpdateDistance();

            GameState.State = GameState.EGameState.BringTheNight; 
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ProcessMoveEnd();
            }

            UiController.DistanceTextBox.text = string.Format("Distance to home: {0} miles", DistanceToHome);
            UiController.Points.text = string.Format("Speed: {0} miles / day\nFood Consumption: {1} / day\nCannon: {2}", 
                Ship.CalculateBoatSpeed(), 
                Ship.CalculateFoodConsumptionBetweenTwoPoints(), 
                Ship.GetCannonBonus());

            if (GameState.State == GameState.EGameState.ComputerTurn || GameState.State == GameState.EGameState.PlayerTurn)
            {
                //Check if there are crew members alive
                if (Ship.AliveCrewMembers.Count() == 0)
                {
                    GameOver("All crew members are dead!");
                }
            }

            if (GameState.State == GameState.EGameState.ComputerTurn)
            {
                MapManager.GoToNextDestination(DesiredRiskiness);
                UiController.ResourcesTextBox.text = string.Format("Resources: food {0}", Ship.Inventory.Food);
                // Handle
                var gameplayEvent = MapManager.GetCurrentNode().NodeEvent;
                gameplayEvent.Execute(Ship);
                UiController.UpdateChoices(MapManager.GetPossibleDestinations());
                GameManager.Instance.UiController.OnChoiceChanged(0);

                // Execute Sfx

                // Show user info message
                ComposedEvent composedEvent = gameplayEvent as ComposedEvent;
                ShipEvent shipEvent = composedEvent.EventsOfInterest.FirstOrDefault() as ShipEvent;

                if (shipEvent != null)
                {
                    string text = "The Goddess of the Sea was merciful! No harm was done to your crew";
                    if (!composedEvent.GetFullEventDetailsMessage().Equals(String.Empty))
                        text = composedEvent.GetFullEventDetailsMessage();

                    ShowMessageBox(shipEvent.eventDescription(), text);

                    GameState.State = GameState.EGameState.WaitForUserEventResultConfirm;
                }
                else
                {
                    GameState.State = GameState.EGameState.BringTheDawn;
                }

                Debug.Log("NEVER HAVE I EVER");
            }
            if (GameState.State == GameState.EGameState.BringTheDawn)
            { 
                while (t > 0.0f)
                {
                    nightBringerSprite.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 1f-t);
                    t -= Time.deltaTime / FadeTime;
                    return;
                }
                t = 0.0f;            

                SetIsUserTurn(true);
            }
            if (GameState.State == GameState.EGameState.BringTheNight)
            {
                while (t < 1.0f)
                {
                    nightBringerSprite.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), t);
                    t += Time.deltaTime / FadeTime;
                    return;
                }
                t = 1.0f;
                SetIsUserTurn(false);
            }
            if (GameState.State == GameState.EGameState.BringingTheEnd)
            {
                while (t < 1.0f)
                {
                    nightBringerSprite.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), t);
                    t += Time.deltaTime / FadeTime;
                    return;
                }
                //UiController.GameOverText.gameObject.SetActive(true);
                t = 1.0f;

                ShowMessageBox("GAME OVER", gameOverReason);
                GameState.State = GameState.EGameState.WaitForUserGameOverConfirm;
            }
            if (GameState.State == GameState.EGameState.GameOver)
            {
                // Load Game over menu
                gameObject.GetComponent<GameSceneManager>().LoadSceneByIndex(4);

                return;
            }
            if (GameState.State == GameState.EGameState.CheckGameState)
            {
                if (Ship.IsDestroyed())
                {
                    GameOver("The Ship is destroyed!");
                    return;
                }

                // First check victory condition, allow player to win if their reached the home with no food left
                if (DistanceToHome <= 0)
                {
                    Victory();
                    return;
                }

                if (Ship.Inventory.Food == 0)
                {
                    GameOver("There is no food left!");
                    return;
                }

                GameState.State = GameState.EGameState.BringTheDawn;
            }
        }

        private void ShowMessageBox(string title, string text)
        {
            UiController.EventCanvas.SetActive(true);
            UiController.StageText.text = title;
            UiController.ScrollRect.viewport.GetComponentInChildren<Text>().text = text;
        }

        public void GameOver(string reason = "")
        {
            Debug.Log("GAME OVER: " + reason);
            gameOverReason = reason;

            InputController.MoveEndButton.gameObject.SetActive(false);
            UiController.PathChoice.gameObject.SetActive(false);

            GameState.State = GameState.EGameState.BringingTheEnd;
        }

        public void Victory()
        {
            GameState.State = GameState.EGameState.Victory;
            InputController.MoveEndButton.gameObject.SetActive(false);
            UiController.PathChoice.gameObject.SetActive(false);
            UiController.VictoryText.gameObject.SetActive(true);

            // Load Victory menu
            gameObject.GetComponent<GameSceneManager>().LoadSceneByIndex(3);
        }

        public void ProcessUserTurnStart()
        {
            // Fade out background music
            AudioController.FadeInBackgroundMusic();

            // Enable next button and path chooser
            InputController.MoveEndButton.gameObject.SetActive(true);
            UiController.PathChoice.gameObject.SetActive(true);
        }

        public void ProcessUserAcceptedEventResult()
        {
            UiController.EventCanvas.SetActive(false);
            if (GameState.State == GameState.EGameState.WaitForUserEventResultConfirm)
                GameState.State = GameState.EGameState.CheckGameState;
            else
                GameState.State = GameState.EGameState.GameOver;
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