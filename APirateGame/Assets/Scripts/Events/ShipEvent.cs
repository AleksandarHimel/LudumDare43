using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Events
{
    public abstract class ShipEvent : IEvent
    {
        public void Execute(GameObject gameObject)
        {
            if (gameObject.CompareTag("Ship"))
            {
                ExecuteEventInternal(gameObject);
            }
        }

        public abstract void ExecuteEventInternal(GameObject gameObject);
    }
}
