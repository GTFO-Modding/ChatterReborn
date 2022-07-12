using ChatterReborn.Attributes;
using ChatterReborn.Utils;
using GameData;
using Gear;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Managers
{

    public class ItemManager : ChatterManager<ItemManager>
    {
        protected override void Setup()
        {
            this.m_pickups = new ComponentList<Item>();
            this.m_pickups.CreateList();
            base.Setup();
        }

        public static void RegisterItem(Item item)
        {
            ChatterDebug.LogWarning("Registering item " + item.name + " ItemDataBlock " + ItemDataBlock.GetBlockName(item.ItemDataBlock.persistentID));
            Current.m_pickups.Add(item);
        }


        protected override void OnLevelCleanup()
        {
            this.m_pickups.Clear();
        }

        private ComponentList<Item> m_pickups;

    }
}
