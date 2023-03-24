using ChatterReborn.Data;
using GameData;
using Localization;
using System;
using System.Collections.Generic;

namespace ChatterReborn.Managers
{
    public class ExtraTextDataBlockManager : ChatterManager<ExtraTextDataBlockManager>
    {
        private bool m_initialized = false;
        protected override void Setup()
        {
            AddExtraTextBlocks();
        }

        private void AddExtraTextBlocks()
        {
            m_customTextBlocks = new Dictionary<CustomTextDataBlock, TextDataBlock>();
            AddNewBlock("ChatterReborn.ExtraCommunicationList.MoreResponses.Title", "Extras!", CustomTextDataBlock.MoreResponses);
            AddNewBlock("ChatterReborn.ExtraCommunicationList.MoreResponses.FirstPerson", "Acknowledgements", CustomTextDataBlock.Acknowledgements);
            AddNewBlock("ChatterReborn.ExtraCommunicationList.MoreResponses.SecondPerson", "Pick a Target", CustomTextDataBlock.SneakKillTargets);
            AddNewBlock("ChatterReborn.ExtraCommunicationList.MoreResponses.Directions", "Directions", CustomTextDataBlock.Directions);
            AddNewBlock("ChatterReborn.ExtraCommunicationList.MoreResponses.PickTarget", "..Pick Target", CustomTextDataBlock.PickAttackTarget);
            AddNewBlock("ChatterReborn.ExtraCommunicationList.MoreResponses.Combat", "Combat", CustomTextDataBlock.Combat);
            AddNewBlock("ChatterReborn.ExtraCommunicationList.MoreResponses.Resources", "Resources", CustomTextDataBlock.Resources);
            AddNewBlock("ChatterReborn.ExtraCommunicationList.MoreResponses.PingResources", "..Ping Resources", CustomTextDataBlock.PingResources);
            AddNewBlock("ChatterReborn.ExtraCommunicationList.MoreResponses.PingConsumables", "..Ping Consumables", CustomTextDataBlock.PingConsumables);

        }


        public override void Update()
        {
            if (!TextDataBlock.m_isSetup)
            {
                return;
            }

            UpdateTexts();
        }


        private void UpdateTexts()
        {
            if (m_initialized)
            {
                return;
            }

            m_initialized = true;
            var localizationService = Text.TextLocalizationService.TryCast<GameDataTextLocalizationService>();
            if (localizationService != null)
            {               

                foreach (var block in m_customTextBlocks.Values)
                {
                    if (!localizationService.m_texts.ContainsKey(block.persistentID))
                    {
                        localizationService.m_texts.Add(block.persistentID, block.GetText(localizationService.CurrentLanguage));
                        
                        
                    }
                }
                Text.UpdateAllTexts();
                this.DebugPrint("Updating all Texts in game for Chatter Reborn!", eLogType.Debug);
                return;
            }

            this.DebugPrint("Could not update all Texts in game for Chatter Reborn!", eLogType.Error);
        }


        private string InspectBlockName(string originalName)
        {
            string newName = originalName;
            bool first = true;
            uint uniqueID = 0;
            do
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    uniqueID++;
                    string prevName = newName;
                    newName = originalName + "_" + uniqueID;

                    this.DebugPrint("TextBlock " + prevName + " already exists! Let's check if this one exists -> " + newName, eLogType.Warning);
                }
                
            } while (TextDataBlock.HasBlock(newName));

            this.DebugPrint("TextBlock name is a non-duplicate -> " + newName + ", we are good!", eLogType.Message);

            return newName;
        }

        private void CheckLastPersistentID()
        {
            m_latestID = 0;
            foreach (var block in TextDataBlock.GetAllBlocks())
            {
                m_latestID = Math.Max(m_latestID, block.persistentID);
            }

            this.DebugPrint("Latest Persistent ID for TextDbs -> " + m_latestID, eLogType.Message);
        }

        private uint m_latestID = 0;

        private void AddNewBlock(string name, string text, CustomTextDataBlock type)
        {
            TextDataBlock block = new TextDataBlock();
            block.English = text;
            block.name = InspectBlockName(name);         
            block.SkipLocalization = true;
            block.internalEnabled = true;
            block.persistentID = 0;

            this.DebugPrint("Now adding custom dataBlock " + block.name);

            TextDataBlock.AddBlock(block, -1);
            m_customTextBlocks.Add(type, block);
            
        }


        public static TextDataBlock GetCustomBlock(CustomTextDataBlock type)
        {
            return Current.m_customTextBlocks[type];
        }


        private Dictionary<CustomTextDataBlock, TextDataBlock> m_customTextBlocks;
    }
}
