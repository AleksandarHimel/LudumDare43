using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Assets.Events
{
    public abstract class ShipEvent : IEvent
    {
        public string FullEventDetailsMessage = String.Empty;

        public void Execute(MonoBehaviour behaviouralObject)
        {
            Ship shipObject = behaviouralObject as Ship;

            if (shipObject != null)
            {
                ExecuteEventInternal(shipObject);
            }
        }

        public abstract void ExecuteEventInternal(Ship ship);

        protected static Random random = new Random();

        protected static int getRandNum(int min, int max)
        {
            return ShipEvent.random.Next(max-min) + min;
        }

        public string GetFullEventDetailsMessage()
        {
            return FullEventDetailsMessage;
        }

        public abstract string eventDescription();
    }
}
