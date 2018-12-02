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
                    return new PirateAttackEvent();
                case EventEnum.SHALLOW_WATER:
                    return new ShallowWaterEvent();
                case EventEnum.RESTLESS_WATERS:
                    return new RestlessWaterEvent();
                case EventEnum.SPREAD_THE_PLAGUE:
                    return new SpreadThePlagueEvent();
                case EventEnum.WALK_THE_PLANK:
                    return new WalkThePlankEvent();
                case EventEnum.CONSUME_RESOURCES_BETWEEEN_STAGES:
                    return new ConsumeResourcesBetweenStages();
                default:
                    return null;
            }
        }

        public IEvent GetNextEvent()
        {
            // TODO: logic
            return new ComposedEvent();
        }

        public void ExecuteEvent(EventEnum eventEnum, MonoBehaviour behaviouralObject, params object[] eventArgs)
        {
            GenerateEvent(eventEnum, eventArgs).Execute(behaviouralObject);
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