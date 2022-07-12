using ChatterReborn.Utils;
using GameData;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Drama_Chatter_States
{
    public class DRAMA_Chatter_Encounter : DRAMA_Chatter_Combat
    {
        protected override bool StartCombatDialogue => false;
    }
}
