using ChatterReborn.Attributes;
using ChatterReborn.Utils;
using Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Components
{
    [IL2CPPType(AddComponentOnStart = false,  DontDestroyOnLoad = false)]
    public class PlayerBreathingTester : MonoBehaviour
    {
        public PlayerBreathingTester(IntPtr p) : base(p)
        {

        }


        

        void OnGUI()
        {
            GUI.Label(new Rect(150f, 750f, 200f, 20f), "Breathing Level - 0");
            GUI.Label(new Rect(150f, 800f, 200f, 20f), "Breathing Level - 1");
            GUI.Label(new Rect(150f, 850f, 200f, 20f), "Breathing Level - 2");
            GUI.Label(new Rect(150f, 900f, 200f, 20f), "Breathing Level - 3");
        }


        
    }
}
