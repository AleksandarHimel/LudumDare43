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
            WaitForUserGameOverConfirm,
            CheckGameState,
            BringingTheEnd,
        }

        public EGameState State = EGameState.PreGame;
        public int CurrentDay;
    }
}
