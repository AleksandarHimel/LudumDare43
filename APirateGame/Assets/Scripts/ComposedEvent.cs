using System.Collections.Generic;

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

        public void Execute()
        {
            foreach (var eventOfInterest in eventsOfInterest)
            {
                eventOfInterest.Execute();
            }
        }
    }
}
