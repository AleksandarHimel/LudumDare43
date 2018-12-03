using Assets.Events;
using Assets.Scripts;
using System;
using UnityEngine;

namespace Assets.Events
{
    public class PirateAttackEvent : ShipEvent
    {
        public override void ExecuteEventInternal(Ship shipObject)
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

            //Get cannon bonus before the cannon get possibly destroyed
            int cannonBonus = shipObject.GetCannonBonus();

            foreach (ShipPart shipPart in shipObject.FunctioningShipParts)
            {
                //uint damage = (uint)getRandNum(GameConfig.Instance.MinPirateAttackShipPartDamage, GameConfig.Instance.MaxPirateAttackShipPartDamage);
                int damage = UnityEngine.Random.Range(GameConfig.Instance.MinPirateAttackShipPartDamage, GameConfig.Instance.MaxPirateAttackShipPartDamage);
                int counterDamage = UnityEngine.Random.Range(0, cannonBonus);
                damage -= counterDamage;
                damage = (damage < 0) ? 0 : damage;
                shipPart.TakeDamage((uint) damage);
                FullEventDetailsMessage += String.Format("Pirates fired their cannons and {0} took {1} damage \n", shipPart.name, damage);
                Debug.Log(String.Format("CannonBonus:{0}; damage:{1}; counterDamage:{2}; shipPart:{3}", shipObject.GetCannonBonus(), damage, counterDamage, shipPart.GetType().ToString()));

            }

            foreach (CrewMember crewMember in shipObject.AliveCrewMembers)
            {
                int damage = getRandNum(GameConfig.Instance.MinPirateAttackCrewMemberDamage, GameConfig.Instance.MaxPirateAttackCrewMemberDamage);
                crewMember.ReduceHealth(damage);
                FullEventDetailsMessage += String.Format("{0} fought like a true pirate but took {1} damage \n", crewMember.PirateName, damage);
            }

            int foodLooted = getRandNum(GameConfig.Instance.MinPirateAttackResourceDamage, GameConfig.Instance.MaxPirateAttackResourceDamage);
            shipObject.Inventory.ReduceResources((uint)foodLooted, 0);
            if (foodLooted > 0)
                FullEventDetailsMessage += String.Format("Damn pirates! {0} food was looted from your ship \n", foodLooted);
        }

        public override string eventDescription()
        {
            return "Ye be attacked by some pirate ruffians! \n";
        }
    }
}