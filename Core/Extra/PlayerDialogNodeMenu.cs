using Player;
using System.Collections.Generic;
using UnityEngine;

namespace ChatterReborn.Extra
{
    public class PlayerDialogNodeMenu
    {

        public void AddNode(PlayerDialogNode node)
        {
            if (this.m_nodes == null)
            {
                this.m_nodes = new List<PlayerDialogNode>();
            }

            if (node.ChildMenu != null)
            {
                node.ChildMenu.m_lastMenu = this;
            }

            this.m_nodes.Add(node);

            node.Index = ++m_lastHighestIndex;
            if (!m_firstNode)
            {
                m_firstNode = true;
                this.m_currentNode = node;
            }
        }

        private bool m_firstNode = false;
        public PlayerDialogNode CurrentNode => m_currentNode;

        private PlayerDialogNode m_currentNode;

        public PlayerDialogNodeMenu LastMenu => m_lastMenu;

        private PlayerDialogNodeMenu m_lastMenu;

        public List<PlayerDialogNode> Nodes => m_nodes;

        private List<PlayerDialogNode> m_nodes;

        private int m_lastHighestIndex = -1;

        private int m_currentIndex = 0;

        public void GoDown()
        {
            if (this.m_lastHighestIndex <= 0)
            {
                return;
            }
            this.m_currentIndex++;
            if (this.m_currentIndex > this.m_lastHighestIndex)
            {
                this.m_currentIndex = 0;
            }

            UpdateCurrentNode();
        }

        private void UpdateCurrentNode()
        {
            m_currentNode = this.m_nodes[this.m_currentIndex];
        }

        public void GoUp()
        {
            if (this.m_lastHighestIndex <= 0)
            {
                return;
            }

            this.m_currentIndex--;
            if (this.m_currentIndex < 0)
            {
                this.m_currentIndex = this.m_lastHighestIndex;
            }
            UpdateCurrentNode();
        }

        public void UpdateCycling()
        {
            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                this.GoUp();
            }
            else if (Input.GetKeyDown(KeyCode.PageDown))
            {
                this.GoDown();
            }
        }

        
        public void OnGUI(GUIStyle selectedStyle, GUIStyle normalStyle)
        {
            float positionVertical = 200f;
            for (int i = 0; i < this.Nodes.Count; i++)
            {
                bool isSelected = this.CurrentNode == this.Nodes[i];
                GUI.Label(new Rect(250f, positionVertical, 300, 100), this.Nodes[i].Label, isSelected ? selectedStyle : normalStyle);
                positionVertical += 25f;
            }
        }
        public void Update()
        {
            this.UpdateCycling();
        }


    }
}
