using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu]
    public class GameState : ScriptableObject
    {
        public enum EGameState : uint
        {
            PreGame,
            PlayerTurn,
            ComputerTurn,
            Victory,
            GameOver,
            BringTheNight,
            BringTheDawn,
            WaitForUserEventResultConfirm,
        }

        public EGameState State = EGameState.PreGame;
        public int CurrentDay;
    }
}
