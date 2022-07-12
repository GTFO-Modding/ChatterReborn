using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Utils
{
    public class ComponentList<T> where T : Component
    {
        private Dictionary<int, T> m_comp_list;

        public void CreateList()
        {
            this.m_comp_list = new Dictionary<int, T>();
        }

        public void Clear()
        {
            this.m_comp_list.Clear();
        }

        public void Add(T component)
        {
            int instanceID = component.GetInstanceID();
            if (!this.m_comp_list.ContainsKey(instanceID))
            {
                this.m_comp_list.Add(instanceID, component);
            }
        }

        public T GetComponent(int instanceID)
        {
            T t = default(T);
            this.m_comp_list.TryGetValue(instanceID, out t);
            return t;
        }

        public bool TryGetComponent(int instanceID, out T comp)
        {
            comp = this.GetComponent(instanceID);
            return comp != default(T);
        }

        public void Remove(T component)
        {
            this.m_comp_list.Remove(component.GetInstanceID());
        }
    }
}
