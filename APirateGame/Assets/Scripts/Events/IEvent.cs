using UnityEngine;

namespace Assets.Events
{
    public interface IEvent
    {
        void Execute(GameObject gameObject);
    }
}
