using ChatterReborn.Data;
using ChatterReborn.Managers;
using ChatterReborn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Drama_Chatter_States
{
    public class DRAMA_Chatter_Alert : DRAMA_Chatter_Base
    {


        public override void Setup()
        {
            base.Setup();
        }

        public override void OnSpitterExplode(InfectionSpitter infectionSpitter)
        {
            float randomDelay = UnityEngine.Random.Range(0.25f, 0.5f);
            this.m_spitter_explodeCallBack.QueueCallBack(infectionSpitter.m_currentExplodeSpeed + randomDelay, infectionSpitter);
        }
    }
}
