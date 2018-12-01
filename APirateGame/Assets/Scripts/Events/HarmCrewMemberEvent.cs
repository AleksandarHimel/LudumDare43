﻿using Assets.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Events
{
    public class HarmCrewMemberEvent : IEvent
    {
        public readonly int damageToInflict = 30;

        public void Execute(MonoBehaviour behaviouralObject)
        {
            CrewMember crewMember = behaviouralObject as CrewMember;

            if (crewMember != null)
            {
                crewMember.ReduceHealth(damageToInflict);
            }
        }
    }
}
