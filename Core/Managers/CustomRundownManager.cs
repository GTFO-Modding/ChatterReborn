using ChatterReborn.Data;
using ChatterReborn.Element;
using ChatterReborn.Utils;
using GameData;
using System.Collections.Generic;

namespace ChatterReborn.Managers
{
    public class CustomRundownManager : ChatterManager<CustomRundownManager>
    {
        public static string GetRundownTitle
        {
            get
            {
                string customRundownTitle = RundownDataBlock.GetBlock(1).StorytellingData.Title;
                ExtendedStringUtils.EliminateInvalidCharacters(ref customRundownTitle);
                return customRundownTitle;
            }
        }

        protected override void Setup()
        {
            this.m_IsCustomRundown = Globals.Global.RundownIdToLoad == 1;
            this.m_enemyFilterElements = new List<EnemyFilterElement>();
        }

        protected override void PostSetup()
        {
            PrepareCustomRundown();
        }

        private void PrepareCustomRundown()
        {
            if (this.m_IsCustomRundown)
            {
                if (!CustomElementManager.Current.Loaded)
                {
                    this.DebugPrint("This is a custom rundown but the custom elements haven't been loaded!!");
                    return;
                }
                this.DebugPrint("Preparing for Custom rundown!!", eLogType.Message);
                PrepareRundown();
                PrepareEnemyFilters();
            }
        }

        private void PrepareEnemyFilters()
        {
            if (this.m_rundownKey == CustomRundown.None)
            {
                return;
            }

            foreach (var element in CustomElementHolderBase<EnemyFilterElement>.GetAllElements())
            {
                if (element.CustomRundownKey == this.m_rundownKey)
                {
                    m_enemyFilterElements.Add(element);
                    this.DebugPrint("Prepared EnemyFilters for Custom enemies : " + element.name, eLogType.Message);
                }
            }

            
        }

        private void PrepareRundown()
        {
            foreach (var element in CustomElementHolderBase<CustomRundownElement>.GetAllElements())
            {
                if (element.name == GetRundownTitle)
                {
                    this.m_rundownKey = element.CustomRundownKey;
                    break;
                }
            }
        }

        private CustomRundown m_rundownKey;

        private List<EnemyFilterElement> m_enemyFilterElements;

        public static List<EnemyFilterElement> EnemyFilterElements => Current.m_enemyFilterElements;

        private bool m_IsCustomRundown;

        public static bool IsCustomRundown => Current.m_IsCustomRundown;
    }
}
