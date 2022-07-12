using ChatterReborn.Attributes;
using ChatterReborn.Extra;
using ChatterReborn.Utils;
using GameData;
using Player;
using System;
using System.Collections.Generic;
using UnhollowerBaseLib.Attributes;
using UnityEngine;

namespace ChatterReborn.ComponentsDev
{
    [IL2CPPType(AddComponentOnStart = true, DontDestroyOnLoad = true)]
    public class DialogNodeTester : MonoBehaviour
    {
        public DialogNodeTester(IntPtr pointer) : base(pointer)
        {
        }

        [HideFromIl2Cpp]
        private void BuildChildNodes(PlayerDialogNode node, int index)
        {      
            int nextNodeIndex = index + 1;
            if (nextNodeIndex > this.m_nodes.Count - 1)
            {
                nextNodeIndex = 0;
            }
            node.NextNode = this.m_nodes[nextNodeIndex];
            int previousIndex = index - 1;
            if (previousIndex < 0)
            {
                previousIndex = this.m_nodes.Count - 1;
            }
            node.PreviousNode = this.m_nodes[previousIndex];          
        }

        void Awake()
        {
            SetupNodes();
            SetupStyles();
        }

        private void SetupStyles()
        {
            this.m_selectedStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 20
            };
            this.m_selectedStyle.normal.textColor = Color.cyan;

            this.m_normalStyle = new GUIStyle
            {
                fontSize = 15
            };
            this.m_normalStyle.normal.textColor = Color.white;
        }

        private void SetupNodes()
        {
            this.m_nodes = new List<PlayerDialogNode>();
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "I can do it, should I?",
                DialogID = GD.PlayerDialog.CL_ICanDoItShouldI,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "I can't do that",
                DialogID = GD.PlayerDialog.CL_ICantDoThat,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "I Will do it",
                DialogID = GD.PlayerDialog.CL_IWillDoIt,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "Will do",
                DialogID = GD.PlayerDialog.CL_WillDo,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "I'm on my way",
                DialogID = GD.PlayerDialog.CL_ImOnMyWay,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "Let's GTFO",
                DialogID = GD.PlayerDialog.CL_LetsGTFO,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "Yes",
                DialogID = GD.PlayerDialog.CL_Yes,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "No",
                DialogID = GD.PlayerDialog.CL_No,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "I understand",
                DialogID = GD.PlayerDialog.CL_IUnderstand,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "Tag them",
                DialogID = GD.PlayerDialog.CL_TagThem,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "I'm exhausted",
                DialogID = GD.PlayerDialog.CL_ImExhausted,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "I'll stay close to you",
                DialogID = GD.PlayerDialog.CL_IllStayCloseToYou,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "I'll follow your lead",
                DialogID = GD.PlayerDialog.CL_IllFollowYourLead,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "Pick this up",
                DialogID = GD.PlayerDialog.CL_PickThisUp,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "You take",
                DialogID = GD.PlayerDialog.CL_YouTake,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "The one closest to me",
                DialogID = GD.PlayerDialog.CL_TheOneClosestToMe,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "The one closest to you",
                DialogID = GD.PlayerDialog.CL_TheOneClosestToYou,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "The one in the middle",
                DialogID = GD.PlayerDialog.CL_TheOneInTheMiddle,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "The one in the far left",
                DialogID = GD.PlayerDialog.CL_TheOneOnTheFarLeft,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "The one in the far right",
                DialogID = GD.PlayerDialog.CL_TheOneOnTheFarRight,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "The one in the left",
                DialogID = GD.PlayerDialog.CL_TheOneOnTheLeft,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "The one in the right",
                DialogID = GD.PlayerDialog.CL_TheOneOnTheRight,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "The one in right in front of me",
                DialogID = GD.PlayerDialog.CL_TheOneRightInFrontOfMe,
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "Hear Hunter Group",
                DialogID = GD.PlayerDialog.hear_hunter_group,
                Description = "Upon a new survival wave"
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "Attracted monsters (Accident)",
                DialogID = GD.PlayerDialog.attracted_monsters_accident,
                Description = "Upon alerting monsters accidentally"
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "Attracted monsters (Intentional)",
                DialogID = GD.PlayerDialog.attracted_monsters_intentional,
                Description = "Upon alerting monsters intentionally"
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "Cough Soft",
                DialogID = GD.PlayerDialog.cough_soft,
                Description = "Exploration dialogue"
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "Cough Hard",
                DialogID = GD.PlayerDialog.cough_hard,
                Description = "Exploration dialogue"
            });
            this.m_nodes.Add(new PlayerDialogNode
            {
                Label = "Sneeze",
                DialogID = GD.PlayerDialog.sneeze,
                Description = "Exploration dialogue"
            });


            this.m_nodes.Clear();
            foreach (var block in PlayerDialogDataBlock.Wrapper.Blocks)
            {
                this.m_nodes.Add(new PlayerDialogNode
                {
                    Label = block.name,
                    DialogID = block.persistentID
                });
            }
        }

        private void Start()
        {
            PostSetupNodes();
        }

        private void PostSetupNodes()
        {
            for (int i = 0; i < m_nodes.Count; i++)
            {
                PlayerDialogNode node = this.m_nodes[i];
                this.BuildChildNodes(node, i);
            }
            this.m_currentNode = this.m_nodes[0];
        }

        [HideFromIl2Cpp]
        private void ScrollThroughDialogues(bool backwards)
        {
            if (backwards)
            {
                if (this.m_currentNode.PreviousNode == null)
                {
                    return;
                }
                this.m_currentNode = this.m_currentNode.PreviousNode;
                return;
            }
            else if (this.m_currentNode.NextNode == null)
            {
                return;
            }
            this.m_currentNode = this.m_currentNode.NextNode;
        }



        private GUIStyle m_normalStyle;

        private GUIStyle m_selectedStyle;

        private void OnGUI()
        {
            float positionVertical = 200f;
            List<DialogOption> options = this.m_currentNode.OptionsList;
            for (int i = 0; i < options.Count; i++)
            {
                DialogOption option = options[i];
                GUI.Label(new Rect(250f, positionVertical, 300, 100), option.Label, option.Selected ? this.m_selectedStyle : this.m_normalStyle);
                positionVertical += 25f;
            }            
        }

        private void Update()
        {
            bool leftBracketDown = Input.GetKeyDown(KeyCode.LeftBracket);
            if (leftBracketDown || Input.GetKeyDown(KeyCode.RightBracket))
            {
                this.ScrollThroughDialogues(leftBracketDown);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (PlayerManager.TryGetLocalPlayerAgent(out var playerAgent))
                {
                    PlayerDialogManager.WantToStartDialog(this.m_currentNode.DialogID, -1, false, true);
                }                
            }
        }
        public List<PlayerDialogNode> m_nodes;

        private PlayerDialogNode m_currentNode;

        [HideFromIl2Cpp]
        private uint DialogID
        {
            get
            {
                return this.m_currentNode.DialogID;
            }
        }

    }
}
