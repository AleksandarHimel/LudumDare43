using System;
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

        public string eventDescription()
        {
            string FullEventDescription = String.Empty;
            foreach (var eventOfInterest in EventsOfInterest)
            {
                if (!eventOfInterest.GetFullEventDetailsMessage().Equals(String.Empty))
                    FullEventDescription += eventOfInterest.eventDescription();
            }
            return FullEventDescription;
        }

        public void Execute(MonoBehaviour behaviouralObject)
        {
            foreach (var eventOfInterest in EventsOfInterest)
            {
                eventOfInterest.Execute(behaviouralObject);
            }
        }

        public string GetFullEventDetailsMessage()
        {
            string FullEventDetailsMessage = String.Empty;
            foreach (var eventOfInterest in EventsOfInterest)
            {
                FullEventDetailsMessage += eventOfInterest.GetFullEventDetailsMessage();
            }
            return FullEventDetailsMessage;
        }
    }
}
