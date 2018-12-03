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

        public int PlagueResourceConsumptionIncrement = 3;

        public int RowingActionFoodConsumptionIncrement = 2;

        public uint InitialFoodCount = 200;
        public uint InitialWoodCount = 100;

        public int ChanceForShallowWaters = 10;
        public int ChanceForFire = 20;
        public int ChanceForPirateAttack = 15;
        public int ChanceForWalkThePlank = 5;

        public int MinShallowWaterShipPartDamage = 0;
        public int MaxShallowWaterShipPartDamage = 10;

        public int MinFireShipPartDamage = 0;
        public int MaxFireShipPartDamage = 100;
        public int MinFireCrewMemberDamage = 0;
        public int MaxFireCrewMemberDamage = 100;

        public int MinPirateAttackShipPartDamage = 0;
        public int MaxPirateAttackShipPartDamage = 10;
        public int MinPirateAttackCrewMemberDamage = 0;
        public int MaxPirateAttackCrewMemberDamage = 100;
        public int MinPirateAttackResourceDamage = 0;
        public int MaxPirateAttackResourceDamage = 30;

        public int CannonWeight = 1;
        public int OarsWeight = 3;
        public int HullWeight = 2;
        public int KitchenWeight = 1;
        public int SailsWeight = 1;
        public int CrowsNestWeight = 1;

        public int PointRequiredForVictory = 10;

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
