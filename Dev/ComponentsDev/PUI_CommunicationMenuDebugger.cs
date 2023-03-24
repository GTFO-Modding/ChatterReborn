using ChatterReborn.Attributes;
using ChatterReborn.Utils;
using GameData;
using Player;
using UnityEngine;
using Il2CPPGeneric = Il2CppSystem.Collections.Generic;

namespace ChatterRebornDev.ComponentsDev
{
    [IL2CPPType(AddComponentOnStart = false, DontDestroyOnLoad = true)]
    public class PUI_CommunicationMenuDebugger : MonoBehaviour
    {

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                CheckMenuPrint();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                AddCustomNode();
            }
            

        }


        private void AddCustomNode()
        {


            PUI_CommunicationMenu menu = PUICommunicationMenu;
            if (menu == null)
            {
                ChatterDebug.LogError("Couldn't get PUI_CommunicationMenu from GuiLayerBase!!");
                return;
            }


            if (menu.m_menu == null)
            {
                ChatterDebug.LogError("ExtraCommunicationManager.CurrentMenu m_menu is null!!");
                return;
            }

            if (menu.m_menu.CurrentNode == null)
            {
                ChatterDebug.LogError("ExtraCommunicationManager.CurrentMenu m_menuCurrentNode is null!!");
                return;
            }


        }

        private void CheckMenuPrint()
        {
            PUI_CommunicationMenu menu = PUICommunicationMenu;
            if (menu == null)
            {
                ChatterDebug.LogError("Couldn't get PUI_CommunicationMenu from GuiLayerBase!!");
                return;
            }


            if (menu.m_menu == null)
            {
                ChatterDebug.LogError("ExtraCommunicationManager.CurrentMenu m_menu is null!!");
                return;
            }

            if (menu.m_menu.CurrentNode == null)
            {
                ChatterDebug.LogError("ExtraCommunicationManager.CurrentMenu m_menuCurrentNode is null!!");
                return;
            }


            m_information = "";
            PrintAllNodes(menu.m_menu.CurrentNode.m_ChildNodes, "\t");

            ChatterDebug.LogMessage(m_information);
        }

        private static PUI_CommunicationMenu PUICommunicationMenu => GuiManager.PlayerLayer.GuiLayerBase.GetComponentInChildren<PUI_CommunicationMenu>();

        private string m_information = "";

        void PrintAllNodes(Il2CPPGeneric.List<CommunicationNode> nodes, string tabs = "")
        {
            foreach (var node in nodes)
            {

                m_information += "\n" + tabs + "Script: " + node.Script;
                m_information += "\n" + tabs + "DialogID: " + node.DialogID + " DialogName -> " + PlayerDialogDataBlock.GetBlockName(node.DialogID);
                m_information += "\n" + tabs + "TextId: " + node.TextId + " TextName -> " + TextDataBlock.GetBlockName(node.TextId);
                m_information += "\n" + tabs + "IsLastNode: " + node.IsLastNode;


                if (node.Icon != null)
                {
                    m_information += m_information + "\n" + tabs + "\t" + "Icon PackageRotation: " + node.Icon.packingRotation;
                    m_information += m_information + "\n" + tabs + "\t" + "Icon Name: " + node.Icon.name;
                    m_information += m_information + "\n" + tabs + "\t" + "Icon Texture: " + node.Icon.texture;

                    if (node.Icon.associatedAlphaSplitTexture != null)
                    {
                        m_information += m_information + "\n" + tabs + "\t" + "Icon associatedAlphaSplitTexture: " + node.Icon.associatedAlphaSplitTexture;
                        m_information += m_information + "\n" + tabs + "\t\t" + "Icon associatedAlphaSplitTexture: " + node.Icon.associatedAlphaSplitTexture;
                    }
                    
                }

                if (node.m_ChildNodes != null && node.m_ChildNodes.Count > 0 && !node.IsLastNode)
                {
                    m_information += "\n" + tabs + "ChildNodes";
                    m_information += "\n" + tabs + "[";
                    PrintAllNodes(node.m_ChildNodes, tabs + "\t");
                    m_information += "\n" + tabs + "]";
                }

                m_information += tabs + "\n";

                
            }
        }
    }

}
