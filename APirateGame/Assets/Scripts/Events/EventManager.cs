using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Assets.Events
{
    public class EventManager : MonoBehaviour
    {
        private static EventManager _instance;

        public IEvent GenerateEvent(EventEnum eventEnum, params object[] eventArgs)
        {
            switch (eventEnum)
            {
                case EventEnum.PIRATES_ATTACK:
                    return null;
                case EventEnum.PLAGUE:
                    return new PlagueEvent();
                case EventEnum.SHALLOW_WATER:
                    return new ShallowWaterEvent();
                case EventEnum.DESTROY_CANNON:
                    return null;
                case EventEnum.DESTROY_OBJECT:
                    return null;
                case EventEnum.GAME_OVER:
                    return null;
                case EventEnum.HARM_SHIP_PART:
                    return new HarmShipPartEvent();
                case EventEnum.HARM_CREW_MEMBER:
                    return new HarmCrewMemberEvent();
                case EventEnum.REDUCE_RESOURCES:
                    return new ReduceResourcesEvent(eventArgs);
                default:
                    return null;
            }
        }

        public IEvent GetNextEvent()
        {
            // TODO: logic
            return new PlagueEvent();
        }

        public void ExecuteEvent(EventEnum eventEnum, MonoBehaviour behaviouralObject, params object[] eventArgs)
        {
            GenerateEvent(eventEnum, eventArgs).Execute(behaviouralObject);
        }

        public void RaiseReduceResourcesEvent(MonoBehaviour behaviouralObject, uint foodToReduce, uint woodToReduce)
        {
            GenerateEvent(EventEnum.REDUCE_RESOURCES, foodToReduce, woodToReduce).Execute(behaviouralObject);
        }

        public ComposedEvent ComposeEvent()
        {
            return new ComposedEvent();
        }

        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventManager();
                }

                return _instance;
            }
        }
    }
}