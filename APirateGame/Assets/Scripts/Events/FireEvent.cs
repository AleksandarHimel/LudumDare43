using Assets.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEvent : MonoBehaviour {

    public class ShallowWaterEvent : ShipEvent
    {
        public readonly uint FireDamageOnShipParts = 2;

        public override void ExecuteEventInternal(MonoBehaviour behaviouralObject)
        {
            Ship shipObject = behaviouralObject as Ship;

            foreach (ShipPart shipPart in shipObject.ShipParts)
            {
                shipPart.TakeDamage(FireDamageOnShipParts);
            }
        }
    }
}
