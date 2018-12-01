using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Events
{
    public abstract class ShipEvent : IEvent
    {
        public void Execute(MonoBehaviour behaviouralObject)
        {
            Ship shipObject = behaviouralObject as Ship;

            if (shipObject != null)
            {
                ExecuteEventInternal(shipObject);
            }
        }

        public abstract void ExecuteEventInternal(Ship ship);
    }
}
