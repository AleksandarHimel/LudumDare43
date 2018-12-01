using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Assets.Events
{
    public class EventMgr
    {
        private static EventMgr _instance;

        public IEvent GenerateEvent(EventEnum eventEnum, params object[] eventArgs)
        {
            switch (eventEnum)
            {
                case EventEnum.PIRATES_ATTACK:
                    return null;
                case EventEnum.PLAGUE:
                    return new PlaugeEvent();
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
                    uint foodToReduce = (uint)eventArgs[0];
                    uint woodToReduce = (uint)eventArgs[1];

                    return new ReduceResourcesEvent(foodToReduce, woodToReduce);
                default:
                    return null;
            }
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

        public static EventMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventMgr();
                }

                return _instance;
            }
        }
    }
}