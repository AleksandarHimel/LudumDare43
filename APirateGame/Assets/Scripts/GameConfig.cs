using Assets.Events;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Unity;

namespace Assets.Scripts
{
    public class GameConfig : MonoBehaviour
    {
        public int InitialShipSpeed;

        public double PlagueSpreadingProbability = 0.3;

        public int PlagueResourceConsumptionIncrement = 30;

        public int RowingActionFoodConsumptionIncrement = 30;

        public int MinimumPirateBoatDamageInflicted = 0;

        public int MaximumPirateBoatDamageInflicted = 100;

        public int MinimumPirateCrewMemberDamageInflicted = 0;

        public int MaxiumumPirateCrewMemberDamageInflicted = 100;

        public int MinimumPirateFoodStolen = 0;

        public int MaximumPirateFoodStolen = 100;

        public uint InitialFoodCount = 100;

        public uint InitialWoodCount = 100;

        public static GameConfig Instance
        {
            get {
                return FindObjectOfType<GameConfig>();
            }
        }

        void Start()
        {
            if (_instance == this)
            {
                return;
            }

            _instance = this;
        }

        private static GameConfig _instance;
    }
}
