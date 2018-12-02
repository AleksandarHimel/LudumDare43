using System.Collections.Generic;
using UnityEngine;

namespace Assets.Events
{
    public class ComposedEvent : IEvent
    {
        public IList<IEvent> EventsOfInterest;

        public ComposedEvent()
        {
            EventsOfInterest = new List<IEvent>();
        }

        public ComposedEvent AddEvent(EventEnum eventEnum)
        {
            EventsOfInterest.Add(
                EventManager.Instance.GenerateEvent(eventEnum)
            );

            return this;
        }

        public void Execute(MonoBehaviour behaviouralObject)
        {
            foreach (var eventOfInterest in EventsOfInterest)
            {
                eventOfInterest.Execute(behaviouralObject);
            }
        }
    }
}
