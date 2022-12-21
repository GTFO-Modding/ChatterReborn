using System;
using System.Collections.Generic;

namespace ChatterReborn.Utils
{
    public partial class WeightHandler<T>
    {

        public void AddValue(T value, float weight)
        {
            this.m_list.Add(new WeightValue<T>
            {
                Value = value,
                Weight = weight
            });
        }

        public static WeightHandler<T> CreateWeightHandler(List<WeightValue<T>> weightValues = null)
        {
            var newhandler = new WeightHandler<T>();
            newhandler.m_list = new List<WeightValue<T>>();
            if (weightValues != null)
            {
                newhandler.m_list.AddRange(weightValues);
            }
            return newhandler;
        }

        public void Clear()
        {
            if (this.m_list == null)
            {
                return;
            }
            this.m_list.Clear();
        }

        private List<WeightValue<T>> m_list;

        public List<WeightValue<T>> List
        {
            get
            {
                return m_list;
            }
            set
            {
                m_list = value;
            }
        }

        public static void DebugLog(object o)
        {
            if (DEBUG_ENABLED)
            {
                ChatterDebug.LogWarning(o);
            }
        }

        public bool HasAny
        {
            get
            {
                return this.Count > 0;
            }
        }

        public int Count
        {
            get
            {
                return this.m_list.Count;
            }
        }


        public WeightValue<T> Best
        {
            get
            {
                if (this.m_list.Count == 0)
                {
                    throw new Exception("You are trying to call the Best value for type " + typeof(T).Name + " which has no items in the array. Please insert a HasAny to prevent this error..");
                }

                float total = 0f;
                for (int i = 0; i < this.m_list.Count; i++)
                {
                    total += this.m_list[i].Weight;
                }

                int index = this.m_list.Count;
                float randomValue = total * UnityEngine.Random.value;
                while (total >= randomValue)
                {
                    index--;
                    total -= this.m_list[index].Weight;
                }
                return m_list[index];
            }
        }

        public bool TryToGetBestValue(out WeightValue<T> val)
        {
            val = default;
            if (!this.HasAny)
            {
                return false;
            }

            val = this.Best;
            return true;
        }

        private static readonly bool DEBUG_ENABLED = false;


    }
}
