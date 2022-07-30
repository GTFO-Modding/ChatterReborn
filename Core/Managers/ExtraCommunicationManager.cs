using ChatterReborn.Attributes;
using ChatterReborn.Components;
using ChatterReborn.Data;
using GameData;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ChatterReborn.Utils;
using SNetwork;
using ChatterReborn.ChatterEvent;

namespace ChatterReborn.Managers
{
    public class ExtraCommunicationManager : ChatterManager<ExtraCommunicationManager>
    {
        public static PUI_CommunicationMenu CurrentMenu;

        protected override void PostSetup()
        {
            this.m_patcher.Patch<PUI_CommunicationMenu>(nameof(PUI_CommunicationMenu.Setup), ChatterPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            this.m_patcher.Patch<PUI_CommunicationMenu>(nameof(PUI_CommunicationMenu.OnCommunicationReceived), ChatterPatchType.Postfix, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }


        static void PUI_CommunicationMenu__Setup__Postfix(PUI_CommunicationMenu __instance)
        {
            CurrentMenu = __instance;
        }


        static void PUI_CommunicationMenu__OnCommunicationReceived__Postfix(SNet_Player src, uint textId, SNet_Player dst)
        {
            ChatterEventListenerHandler<TextCommandEvent>.PostEvent(new TextCommandEvent
            {
                source = src,
                textId = textId,
                destination = dst
            });
        }

        public static CommunicationNode CreateNode(uint textID, uint dialogID)
        {
            var m_node3 = new CommunicationNode();
            m_node3.TextId = textID;
            m_node3.DialogID = dialogID;
            m_node3.Script = CommunicationNode.ScriptType.None;
            m_node3.Icon = new Sprite();
            m_node3.m_ChildNodes = new Il2CppSystem.Collections.Generic.List<CommunicationNode>();
            m_node3.IsLastNode = true;
            return m_node3;
        }

        public override void On_Registered_LocalPlayerAgent(LocalPlayerAgent localPlayerAgent)
        {
            if (ConfigurationManager.StealthCommandsEnabled)
            {
                localPlayerAgent.gameObject.AddAbsoluteComponent<AutoCommunicator>();
            }       

            
        }


        public static void AddNodeTo(CommunicationListCategory category, CommunicationNode nodeToAdd)
        {
            CurrentMenu.m_buttons[(int)category].Node.m_ChildNodes.Add(nodeToAdd);
        }
    }
}
