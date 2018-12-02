using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Events
{
    public class ReduceResourcesEvent : ShipEvent
    {
        private uint foodToReduce;
        private uint woodToReduce;

        public ReduceResourcesEvent(params object[] eventArgs)
        {
            if (eventArgs != null)
            {
                this.foodToReduce = (uint)eventArgs[0];
                this.woodToReduce = (uint)eventArgs[1];
            }
            else
            {
                this.foodToReduce = (uint) UnityEngine.Random.Range(0, 2);
                this.woodToReduce = (uint) UnityEngine.Random.Range(0, 2);
            }
    }

        public override void ExecuteEventInternal(Ship ship)
        {
            ship.Inventory.TryRemoveAmountOfFood(foodToReduce);
            ship.Inventory.TryRemoveAmountOfWood(woodToReduce);
        }
    }
}
