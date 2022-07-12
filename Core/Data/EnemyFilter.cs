using System;

namespace ChatterReborn.Data
{
    [Flags]
    public enum EnemyFilter : uint
    {
        None = 0,
        Striker = 1 << 0,
        Shooter = 1 << 1,        
        Tank = 1 << 2,
        Shadow = 1 << 3,
        Spitter = 1 << 4,
        Big = 1 << 5,
        Birther = 1 << 6,
        Flyer = 1 << 7,
        SquidWard = 1 << 8,
        Child = 1 << 9,
        Scout = 1 << 10,
        BullRush = 1 << 11
    }
}
