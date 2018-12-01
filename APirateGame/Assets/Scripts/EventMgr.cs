using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Assets.Events
{
    public class EventMgr
    {
        private static EventMgr _instance;

        public IEvent GenerateEvent(EventEnum eventEnum)
        {
            switch (eventEnum)
            {
                default:
                    return null;
            }
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