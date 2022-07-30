using ChatterReborn.Attributes;
using ChatterReborn.Data;
using ChatterReborn.Utils;
using UnityEngine;

namespace ChatterReborn.Managers
{
    public class CoolDownManager : ChatterManager<CoolDownManager>
    {
        protected override void Setup()
        {
            this.m_cooldowns = new DictionaryExtended<string, float>();
            base.Setup();
        }

        private void ResetCoolDowns()
        {
            this.m_cooldowns.Clear();
        }

        public override void OnLevelCleanUp()
        {            
            ResetCoolDowns();
        }


        public static void ApplyCooldown(string key, DelayValue timer)
        {
            float goalTimer = timer.totalTimer;
            if (!Current.m_cooldowns.ContainsKey(key))
            {
                Current.m_cooldowns.Add(key, Time.time + goalTimer);
                return;
            }

            Current.m_cooldowns[key] = Time.time + goalTimer;
        }

        public static void ApplyCooldown(CoolDownType type, DelayValue timer)
        {
            ApplyCooldown(type.ToString(), timer);
        }

        public static bool HasCooldown(CoolDownType type)
        {
            return HasCooldown(type.ToString());
        }

        public static bool HasCooldown(string key)
        {
            return Current.m_cooldowns.ContainsKey(key) && Current.m_cooldowns[key] > Time.time;
        }




        private DictionaryExtended<string, float> m_cooldowns;

    }
}
