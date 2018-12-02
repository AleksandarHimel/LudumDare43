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
            FullEventDetailsMessage = String.Format("Captain, no work without food! Your crew ate {0} food. \n", foodToReduce);
        }

        public override string eventDescription()
        {
            return "Heave ho! We be eatin' food. \n";
        }
    }
}
