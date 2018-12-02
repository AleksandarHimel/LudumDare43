using System;
using UnityEngine;

namespace Assets.Events
{
    public interface IEvent
    {
        void Execute(MonoBehaviour behaviouralObject);        
    }    
}
