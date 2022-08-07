using ChatterReborn.Data;
using GameData;
using Localization;
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
            AddNewBlock("ChatterReborn.ExtraCommunicationList.MoreResponses.SecondPerson", "Sneak Kill Targets", CustomTextDataBlock.SneakKillTargets);
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


        private void AddNewBlock(string name, string text,CustomTextDataBlock type)
        {
            var block = TextDataBlock.AddNewBlock();
            block.English = text;
            TextDataBlock.RenameBlock(block, name);
            block.SkipLocalization = true;
            block.internalEnabled = true;
            m_customTextBlocks.Add(type, block);
            this.DebugPrint("Now adding custom dataBlock " + block.name);
        }


        public static TextDataBlock GetCustomBlock(CustomTextDataBlock type)
        {
            return Current.m_customTextBlocks[type];
        }


        private Dictionary<CustomTextDataBlock, TextDataBlock> m_customTextBlocks;
    }
}
