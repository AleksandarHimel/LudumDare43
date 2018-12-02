using System;

namespace Assets.Events
{
    public enum EventEnum
    {
        SHALLOW_WATER = 0,
        MAX_FIRST_TIER = 0,
        PLAGUE = 1,
        MAX_SECOND_TIER = 1,
        PIRATES_ATTACK = 2,
        MAX_THIRD_TIER = 2, 
        WALK_THE_PLANK,
        DESTROY_CANNON,
        DESTROY_OBJECT,
        GAME_OVER,
        HARM_SHIP_PART,
        HARM_CREW_MEMBER,
        REDUCE_RESOURCES,
        EVENT_MAX
    }
}
