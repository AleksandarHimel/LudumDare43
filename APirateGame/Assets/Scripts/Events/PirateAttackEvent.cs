﻿using Assets.Events;
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

            var cannonBoomManagerGO = GameObject.Find("CannonBoom");
            cannonBoomManagerGO.GetComponent<AudioSource>().Play();

            bool fVictory = true;

            foreach (ShipPart shipPart in shipObject.FunctioningShipParts)
            {
                //uint damage = (uint)getRandNum(GameConfig.Instance.MinPirateAttackShipPartDamage, GameConfig.Instance.MaxPirateAttackShipPartDamage);
                int damage = UnityEngine.Random.Range(GameConfig.Instance.MinPirateAttackShipPartDamage, GameConfig.Instance.MaxPirateAttackShipPartDamage);
                int counterDamage = cannonBonus;
                damage -= cannonBonus;
                damage = (damage < 0) ? 0 : damage;
                fVictory &= (damage == 0);
                shipPart.TakeDamage((uint) damage);
                FullEventDetailsMessage += String.Format("Pirates fired their cannons and {0} took {1} damage \n", shipPart.FriendlyName, damage);
                Debug.Log(String.Format("CannonBonus:{0}; damage:{1}; counterDamage:{2}; shipPart:{3}", shipObject.GetCannonBonus(), damage, counterDamage, shipPart.GetType().ToString()));
            }

            if (fVictory)
            {
                FullEventDetailsMessage += String.Format("Miracle!!! We drove the pirates away by the might of our cannons!");
                return;
            }
            
            foreach (CrewMember crewMember in shipObject.AliveCrewMembers)
            {
                int damage = getRandNum(GameConfig.Instance.MinPirateAttackCrewMemberDamage, GameConfig.Instance.MaxPirateAttackCrewMemberDamage);
                damage -= cannonBonus;
                damage = (damage < 0) ? 0 : damage;
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