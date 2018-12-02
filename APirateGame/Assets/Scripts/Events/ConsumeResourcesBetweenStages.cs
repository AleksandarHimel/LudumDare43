using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Events
{
    public class ConsumeResourcesBetweenStages : ShipEvent
    {
        public override void ExecuteEventInternal(Ship ship)
        {
            uint foodToReduce = ship.CalculateFoodConsumptionBetweenTwoPoints();
            ship.Inventory.ReduceResources(foodToReduce, 0);
        }
    }
}
