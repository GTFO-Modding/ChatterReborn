using ChatterReborn.Attributes;
using ChatterReborn.Extra;
using ChatterReborn.Data;
using ChatterReborn.Utils;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Managers
{
    public class DialogItemManager : ChatterManager<DialogItemManager>
    {
        protected override void Setup()
        {
            this.m_pickupDialogs = new Dictionary<int, BasePickUpDialog>();
        }

        protected override void OnLevelCleanup()
        {
            this.m_pickupDialogs.Clear();
        }


        public static void OnWield(Item wield)
        {
            BasePickUpDialog basePickUpDialog;
            if (Current.m_pickupDialogs.TryGetValue(wield.GetInstanceID(), out basePickUpDialog))
            {
                basePickUpDialog.OnWield();
            }
        }

        public override void Update()
        {
            for (int i = 0; i < AllItems.Count; i++)
            {
                BasePickUpDialog item = AllItems[i];
                if (item != null)
                {
                    item.Update();
                }
            }
        }

        public void SetupPickUpInstance(Item item, BasePickUpDialog pickUpDialog)
        {
            int ID = item.GetInstanceID();

            if (this.m_pickupDialogs.ContainsKey(ID))
            {
                ChatterDebug.LogWarning("An ItemPickUpDialog is attempted to be added that already exist!!");
                return;
            }

            pickUpDialog.Setup();

            this.m_pickupDialogs.Add(ID, pickUpDialog);
        }

        public static List<BasePickUpDialog> AllItems
        {
            get
            {
                return Current.m_pickupDialogs.Values.ToList();
            }
        }
        private Dictionary<int, BasePickUpDialog> m_pickupDialogs;

    }
}
