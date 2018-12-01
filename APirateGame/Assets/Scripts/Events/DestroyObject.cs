using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Events
{
    public class DestroyObject : IEvent
    {
        public void Execute(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
    }
}
