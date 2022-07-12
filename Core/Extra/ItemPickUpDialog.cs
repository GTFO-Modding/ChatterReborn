using ChatterReborn.Managers;
using ChatterReborn.Utils;
using GameData;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Extra
{
    public class ItemPickUpDialog<T> : BasePickUpDialog where T : Item
    {
        public ItemPickUpDialog(T interactableItem)
        {
            this.m_item_pickup = interactableItem;
            this.m_dataBlock = interactableItem.ItemDataBlock;
        }

        public override void Setup()
        {
            this.BaseSetup();
        }


        protected T m_item_pickup;


    }
}
