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
        public int InitialShipSpeed = 100;

        public double PlagueSpreadingProbability = 0.3;

        public int PlagueResourceConsumptionIncrement = 30;

        public int RowingActionFoodConsumptionIncrement = 30;

        public uint InitialFoodCount = 1500;
        public uint InitialWoodCount = 100;

        public int ChanceForShallowWaters = 10;
        public int ChanceForFire = 20;
        public int ChanceForPirateAttack = 15;
        public int ChanceForWalkThePlank = 5;

        public int MinShallowWaterShipPartDamage = 0;
        public int MaxShallowWaterShipPartDamage = 100;

        public int MinFireShipPartDamage = 0;
        public int MaxFireShipPartDamage = 100;
        public int MinFireCrewMemberDamage = 0;
        public int MaxFireCrewMemberDamage = 100;

        public int MinPirateAttackShipPartDamage = 0;
        public int MaxPirateAttackShipPartDamage = 100;
        public int MinPirateAttackCrewMemberDamage = 0;
        public int MaxPirateAttackCrewMemberDamage = 100;
        public int MinPirateAttackResourceDamage = 0;
        public int MaxPirateAttackResourceDamage = 100;

        public int CannonWeight = 1;
        public int EngineRoomWeight = 3;
        public int HullWeight = 2;
        public int KitchenWeight = 1;
        public int SailsWeight = 1;

        public static GameConfig Instance
        {
            get
            {
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
