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

        public ReduceResourcesEvent(uint foodToReduce, uint woodToReduce)
        {
            this.foodToReduce = foodToReduce;
            this.woodToReduce = woodToReduce;
        }


        public override void ExecuteEventInternal(Ship ship)
        {
            ship.Inventory.ReduceResources(foodToReduce, woodToReduce);
        }
    }
}
