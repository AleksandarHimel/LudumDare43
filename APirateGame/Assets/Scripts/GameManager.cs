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
        public int Points;
        public int DistanceToHome;
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
        void Start()
        {
            Debug.Log("Random seed: " + UnityEngine.Random.seed);

            UiController.UpdateChoices(MapManager.GetPossibleDestinations());
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

        public void ProcessMoveEnd()
        { 
            // Fade out background music
            AudioController.FadeOutBackgroundMusic();

            // Disable next buton and path pick
            InputController.MoveEndButton.gameObject.SetActive(false);
            UiController.PathChoice.gameObject.SetActive(false);

            Ship.ProcessMoveEnd();

            int distanceTravelled = Ship.CalculateBoatSpeed();
            // if we went with a low-risk roundabout path the total distance has increased
            int roundaboutPathPenalty = Math.Max(3 - GameManager.Instance.DesiredRiskiness, 0) * distanceTravelled / 4;

            Debug.Log(
                "Starting distance " + DistanceToHome + 
                ", travelled " + distanceTravelled + 
                ", penalty " + roundaboutPathPenalty);

            DistanceToHome = Math.Max(0, DistanceToHome - distanceTravelled + roundaboutPathPenalty);

            if (DistanceToHome == 0)
            {
                Victory();
                return;
            }

            if (Ship.Inventory.Food == 0 || Ship.IsDestroyed())
            {
                GameOver();
                return;
            }

            GameState.State = GameState.EGameState.BringTheNight; 
        }

        void Update()
        {
            UiController.Points.text = string.Format("Distance to home: {0} miles\nSpeed: {1} miles / day\nFood Consumption: {2} / day", DistanceToHome, Ship.CalculateBoatSpeed(), Ship.CalculateDefaultFoodConsumption());

            if (GameState.State == GameState.EGameState.ComputerTurn)
            {
                MapManager.GoToNextDestination(DesiredRiskiness - 1);
                Points = Points + MapManager.Instance.GetCurrentNode().Riskiness + 1;
                Debug.Log("POINTS" + Points);
                UiController.ResourcesTextBox.text = string.Format("Resources: food {0}", Ship.Inventory.Food);
                
                // Handle
                var gameplayEvent = MapManager.GetCurrentNode().NodeEvent;
                gameplayEvent.Execute(Ship);
                UiController.UpdateChoices(MapManager.GetPossibleDestinations());

                // Execute Sfx

                // Show user info message
                ComposedEvent composedEvent = gameplayEvent as ComposedEvent;
                ShipEvent shipEvent = composedEvent.EventsOfInterest.FirstOrDefault() as ShipEvent;

                if (shipEvent != null)
                {
                    UiController.EventCanvas.SetActive(true);
                    UiController.StageText.text = shipEvent.eventDescription();
                    UiController.ScrollRect.viewport.GetComponentInChildren<Text>().text = composedEvent.GetFullEventDetailsMessage();

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
            InputController.MoveEndButton.gameObject.SetActive(false);
            UiController.PathChoice.gameObject.SetActive(false);
            UiController.GameOverText.gameObject.SetActive(true);
        }

        public void Victory()
        {
            GameState.State = GameState.EGameState.Victory;
            InputController.MoveEndButton.gameObject.SetActive(false);
            UiController.PathChoice.gameObject.SetActive(false);
            UiController.VictoryText.gameObject.SetActive(true);
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
            GameState.State = GameState.EGameState.BringTheDawn;
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