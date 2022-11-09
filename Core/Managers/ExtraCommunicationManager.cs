﻿using ChatterReborn.ChatterEvent;
using ChatterReborn.Components;
using ChatterReborn.Data;
using ChatterReborn.Utils;
using GameData;
using HarmonyLib;
using Localization;
using Player;
using SNetwork;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Il2CPPGeneric = Il2CppSystem.Collections.Generic;

namespace ChatterReborn.Managers
{
    public class ExtraCommunicationManager : ChatterManager<ExtraCommunicationManager>
    {
        public static PUI_CommunicationMenu CurrentMenu;
        private CommunicationNode m_startingNode;

        protected override void PostSetup()
        {
            GetCurrentMenu();
            AddNewComs();
            this.m_patcher.Patch<PUI_CommunicationMenu>(nameof(PUI_CommunicationMenu.OnCommunicationReceived), HarmonyPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
            //this.m_patcher.Patch<PUI_CommunicationMenu>(nameof(PUI_CommunicationMenu.click), HarmonyPatchType.Postfix, BindingFlags.Public | BindingFlags.Instance);
        }

        private void GetCurrentMenu()
        {
            if (TryToGetCommunicationMenu(out PUI_CommunicationMenu menu))
            {
                CurrentMenu = menu;
                m_startingNode = menu.m_menu.CurrentNode;
            }
        }

        private CommunicationNode GetFirstPersonActionNodes()
        {
            var node = CreateNode(CustomTextDataBlock.Acknowledgements, false);
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.I_Can_Do_It_Should_I, GD.PlayerDialog.CL_ICanDoItShouldI));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.I_Cant_Do_That, GD.PlayerDialog.CL_ICantDoThat));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.I_Cant_Place_It_There, GD.PlayerDialog.CL_ICantPlaceItThere));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Ill_Follow_Your_Lead, GD.PlayerDialog.CL_IllFollowYourLead));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Ill_Stay_Close_To_You, GD.PlayerDialog.CL_IllStayCloseToYou));            
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.IWillDoIt, GD.PlayerDialog.CL_IWillDoIt));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Will_Do, GD.PlayerDialog.CL_WillDo));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.IAgree, GD.PlayerDialog.CL_IAgree));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.IDisagree, GD.PlayerDialog.CL_IDisagree));
            return node;
        }

        private void GiveTargetsToNode(CommunicationNode node)
        {
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.The_One_In_The_Middle, GD.PlayerDialog.CL_TheOneInTheMiddle));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.The_One_On_The_Left, GD.PlayerDialog.CL_TheOneOnTheLeft));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.The_One_On_The_Right, GD.PlayerDialog.CL_TheOneOnTheRight));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.The_One_On_The_Far_Left, GD.PlayerDialog.CL_TheOneOnTheFarLeft));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.The_One_On_The_Far_Right, GD.PlayerDialog.CL_TheOneOnTheFarRight));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.The_One_Closest_To_Me, GD.PlayerDialog.CL_TheOneClosestToMe));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.The_One_Closest_To_You, GD.PlayerDialog.CL_TheOneClosestToYou));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.The_One_Right_In_Front_Of_Me, GD.PlayerDialog.CL_TheOneRightInFrontOfMe));
        }


        private CommunicationNode GetSecondPersonActionNodes()
        {

            var node = CreateNode(CustomTextDataBlock.SneakKillTargets, false);

            m_Youll_Take_Node = CreateNode(GD.Text.PlayerDialogData.CommunicationList.You_Take, 0, false);
            m_Ill_Take_Node = CreateNode(GD.Text.PlayerDialogData.CommunicationList.Ill_Take, 0, false);
            m_We_Take_Node = CreateNode(GD.Text.PlayerDialogData.CommunicationList.We_Take, 0, false);

            node.m_ChildNodes.Add(m_Youll_Take_Node);
            node.m_ChildNodes.Add(m_Ill_Take_Node);
            node.m_ChildNodes.Add(m_We_Take_Node);

            GiveTargetsToNode(m_Youll_Take_Node);
            GiveTargetsToNode(m_Ill_Take_Node);
            GiveTargetsToNode(m_We_Take_Node);

            m_sneakKillTargetsNode = node;
            return node;
        }

        private CommunicationNode m_Ill_Take_Node;
        private CommunicationNode m_Youll_Take_Node;
        private CommunicationNode m_We_Take_Node;

        private CommunicationNode m_sneakKillTargetsNode;

        private CommunicationNode GetDirectionsNodes()
        {
            var node = CreateNode(CustomTextDataBlock.Directions, false);
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.North, GD.PlayerDialog.CL_North));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.South, GD.PlayerDialog.CL_South));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.West, GD.PlayerDialog.CL_West));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.East, GD.PlayerDialog.CL_East));

            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.LookUp, GD.PlayerDialog.CL_LookUp));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Left, GD.PlayerDialog.CL_Left));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Right, GD.PlayerDialog.CL_Right));

            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.FarLeft, GD.PlayerDialog.CL_FarLeft));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.FarRight, GD.PlayerDialog.CL_FarRight));

            return node;
        }

        private CommunicationNode GetMiscActionNodes()
        {
            var node = CreateNode(CustomTextDataBlock.Combat, false);
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Anyone, GD.PlayerDialog.CL_Anyone));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Everyone, GD.PlayerDialog.CL_Everyone));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Attack, GD.PlayerDialog.CL_Attack));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Defend, GD.PlayerDialog.CL_Defend));            
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.BackUp, GD.PlayerDialog.CL_BackUp));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Flashlights, GD.PlayerDialog.CL_Flashlights));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Lets_Get_The_Fuck_Out, GD.PlayerDialog.CL_LetsGTFO));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Scan, GD.PlayerDialog.CL_Scan));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Tag_Them, GD.PlayerDialog.CL_TagThem));

            return node;
        }

        private CommunicationNode GetPingResources()
        {
            var node = CreateNode(CustomTextDataBlock.PingResources, false);
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.AmmoHere, GD.PlayerDialog.CL_AmmoHere));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.MedPackHere, GD.PlayerDialog.CL_MedPackHere));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.ToolRefillHere, GD.PlayerDialog.CL_ToolRefillHere));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.DisinfectionHere, GD.PlayerDialog.CL_DisinfectionHere));
            return node;
        }

        private CommunicationNode GetPingConsumables()
        {
            var node = CreateNode(CustomTextDataBlock.PingConsumables, false);
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.There_Is_A_Foam_Grenade_Here, GD.PlayerDialog.CL_ThereIsAFoamGrenadeHere));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.There_Is_A_Fog_Repeller_Here, GD.PlayerDialog.CL_ThereIsAFogRepellerHere));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.There_Is_A_Long_Range_Flashlight_Here, GD.PlayerDialog.CL_ThereIsALongRangeFlashlightHere));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.There_Is_A_Red_Syringe_Here, GD.PlayerDialog.CL_ThereIsARedSyringeHere));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.There_Is_A_Yellow_Syringe_Here, GD.PlayerDialog.CL_ThereIsAYellowSyringeHere));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.There_Is_A_Lock_Melter_Here, GD.PlayerDialog.CL_ThereIsALockMelterHere));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.There_Is_A_Trip_Mine_Here, GD.PlayerDialog.CL_ThereIsATripMineHere));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.There_Is_A_Foam_Mine_Here, GD.PlayerDialog.CL_ThereIsAFoamMineHere));
            return node;
        }

        private CommunicationNode GetResourcesNodes()
        {
            var node = CreateNode(CustomTextDataBlock.Resources, false);
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.WeNeedResources, GD.PlayerDialog.CL_WeNeedResources));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.ResourcesHere, GD.PlayerDialog.CL_ResourcesHere));
            node.m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.I_Got_Ammo_Someone_Else_Carry_This, GD.PlayerDialog.CL_IGotAmmoSomeoneElseCarryThis));
            node.m_ChildNodes.Add(GetPingResources());
            node.m_ChildNodes.Add(GetPingConsumables());
            return node;
        }


        private void AddNewComs()
        {
            if (CurrentMenu != null)
            {

                this.DebugPrint("Found the PUI_CommunicationMenu, now adding more nodes!!", eLogType.Message);
                var extrasNode = CreateNode(CustomTextDataBlock.MoreResponses, 0, false);

                CurrentMenu.m_menu.CurrentNode.m_ChildNodes[(int)CommunicationListCategory.Deployables].m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Scan, GD.PlayerDialog.CL_Scan, CommunicationNode.ScriptType.UseTool, true));
                CurrentMenu.m_menu.CurrentNode.m_ChildNodes[(int)CommunicationListCategory.Sneaking].m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Shh, GD.PlayerDialog.CL_Shh));
                CurrentMenu.m_menu.CurrentNode.m_ChildNodes[(int)CommunicationListCategory.Sneaking].m_ChildNodes.Insert(2, CreateNode(GD.Text.PlayerDialogData.CommunicationList.AreYouReady, GD.PlayerDialog.CL_AreYouReady));
                CurrentMenu.m_menu.CurrentNode.m_ChildNodes[(int)CommunicationListCategory.Objective].m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Secure_This_Room, GD.PlayerDialog.CL_SecureThisRoom));
                CurrentMenu.m_menu.CurrentNode.m_ChildNodes[(int)CommunicationListCategory.Responses].m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Be_Right_Back, GD.PlayerDialog.CL_BRB));
                CurrentMenu.m_menu.CurrentNode.m_ChildNodes[(int)CommunicationListCategory.Responses].m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Im_Exhausted, GD.PlayerDialog.CL_ImExhausted));
                CurrentMenu.m_menu.CurrentNode.m_ChildNodes[(int)CommunicationListCategory.Responses].m_ChildNodes.Add(CreateNode(GD.Text.PlayerDialogData.CommunicationList.Limited_Vision, GD.PlayerDialog.CL_LimitedVision));
                extrasNode.m_ChildNodes.Add(GetFirstPersonActionNodes());
                extrasNode.m_ChildNodes.Add(GetSecondPersonActionNodes());
                extrasNode.m_ChildNodes.Add(GetDirectionsNodes());
                extrasNode.m_ChildNodes.Add(GetResourcesNodes());
                extrasNode.m_ChildNodes.Add(GetMiscActionNodes());
                CurrentMenu.m_menu.CurrentNode.m_ChildNodes.Add(extrasNode);
                
                return;
            }

            this.DebugPrint("Could not find PUI_CommunicationMenu from GuiManager.PlayerLayer.GuiLayerBase!!!", eLogType.Error);
        }

        private bool TryToGetCommunicationMenu(out PUI_CommunicationMenu menu)
        {
            menu = GuiManager.PlayerLayer.GuiLayerBase.GetComponentInChildren<PUI_CommunicationMenu>();

            return menu != null;
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

        public static CommunicationNode CreateNode(uint textID, uint dialogID, bool isLastNode = true)
        {
            return CreateNode(textID, dialogID, CommunicationNode.ScriptType.None, isLastNode);
        }

        public static CommunicationNode CreateNode(uint textID, uint dialogID, CommunicationNode.ScriptType scriptType, bool isLastNode = true)
        {
            var m_node3 = new CommunicationNode();
            m_node3.TextId = textID;
            m_node3.DialogID = dialogID;
            m_node3.Script = scriptType;
            m_node3.Icon = new Sprite();
            m_node3.m_ChildNodes = new Il2CppSystem.Collections.Generic.List<CommunicationNode>();
            m_node3.IsLastNode = isLastNode;
            return m_node3;
        }



        public static CommunicationNode CreateNode(CustomTextDataBlock type, uint dialogID, bool isLastNode = true)
        {
            return CreateNode(ExtraTextDataBlockManager.GetCustomBlock(type).persistentID, dialogID, isLastNode);
        }

        public static CommunicationNode CreateNode(CustomTextDataBlock type, bool isLastNode = true)
        {
            return CreateNode(ExtraTextDataBlockManager.GetCustomBlock(type).persistentID, 0, isLastNode);
        }

        public override void On_Registered_LocalPlayerAgent(LocalPlayerAgent localPlayerAgent)
        {
            if (ConfigurationManager.StealthCommandsEnabled)
            {
                localPlayerAgent.gameObject.AddAbsoluteComponent<AutoCommunicator>();
            }       

            
        }


        public override void Update()
        {
            if (FocusStateManager.CurrentState != eFocusState.FPS_CommunicationDialog)
            {
                m_isInSneakKillMenu = false;
                m_sneakKillMenuExited = false;
                m_NodeMenuHasDialog = false;
                return;
            }            

            if (!m_isInSneakKillMenu)
            {
                if (m_sneakKillTargetsNode.TextId == CurrentMenu.m_menu.CurrentNode.TextId)
                {
                    m_isInSneakKillMenu = true;
                }
            }
            else if (!m_sneakKillMenuExited)
            {
               
                if (CurrentMenu.m_menu.CurrentNode.TextId == m_Ill_Take_Node.TextId)
                {
                    m_secondDialogID = GD.PlayerDialog.CL_IllTake;
                    m_textId = GD.Text.PlayerDialogData.CommunicationList.Ill_Take;
                    m_NodeMenuHasDialog = true;
                }

                else if (CurrentMenu.m_menu.CurrentNode.TextId == m_Youll_Take_Node.TextId)
                {
                    m_secondDialogID = GD.PlayerDialog.CL_YouTake;
                    m_textId = GD.Text.PlayerDialogData.CommunicationList.You_Take;
                    m_NodeMenuHasDialog = true;
                }

                else if (CurrentMenu.m_menu.CurrentNode.TextId == m_We_Take_Node.TextId)
                {
                    m_secondDialogID = GD.PlayerDialog.CL_WeTake;
                    m_textId = GD.Text.PlayerDialogData.CommunicationList.We_Take;
                    m_NodeMenuHasDialog = true;
                }
                if (m_NodeMenuHasDialog)
                {
                    m_sneakKillMenuExited = true;
                    PlayerAgent localPlayerAgent = PlayerManager.GetLocalPlayerAgent();
                    if (localPlayerAgent != null)
                    {
                        PlayerDialogManager.WantToStartDialogForced(m_secondDialogID, localPlayerAgent);
                        if (m_textId > 0)
                        {
                            CurrentMenu.m_menu.BroadcastDialog(localPlayerAgent, m_textId, CurrentMenu.m_menu.TryGetCurrentTarget());
                        }
                    }
                }
                
            }
        }


        public static void AddNodeTo(CommunicationListCategory category, CommunicationNode nodeToAdd)
        {
            CurrentMenu.m_buttons[(int)category].Node.m_ChildNodes.Add(nodeToAdd);
        }


        private bool m_isInSneakKillMenu;

        private bool m_sneakKillMenuExited;

        private uint m_secondDialogID;

        private uint m_textId;

        private bool m_NodeMenuHasDialog;
    }
}
