using Assets.Events;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameConfig : MonoBehaviour
    {
        public int InitialShipSpeed;

        public double PlagueSpreadingProbability = 0.3;

        public int PlagueResourceConsumptionIncrement = 30;

        public int RowingActionFoodConsumptionIncrement = 30;

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

        public static GameConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameConfig();
                }

                return _instance;
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
