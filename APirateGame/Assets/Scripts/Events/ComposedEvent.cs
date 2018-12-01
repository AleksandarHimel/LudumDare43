using System.Collections.Generic;
using UnityEngine;

namespace Assets.Events
{
    public class ComposedEvent : IEvent
    {
        private IList<IEvent> eventsOfInterest;

        public ComposedEvent()
        {
            eventsOfInterest = new List<IEvent>();
        }

        public ComposedEvent AddEvent(EventEnum eventEnum)
        {
            eventsOfInterest.Add(
                EventMgr.Instance.GenerateEvent(eventEnum)
            );

            return this;
        }

        public void Execute(MonoBehaviour behaviouralObject)
        {
            foreach (var eventOfInterest in eventsOfInterest)
            {
                eventOfInterest.Execute(behaviouralObject);
            }
        }
    }
}
