using ChatterReborn.Attributes;
using ChatterReborn.Extra;
using ChatterReborn.Utils;
using GameData;
using Player;
using System;
using UnityEngine;

namespace ChatterReborn.ComponentsDev
{
    [IL2CPPType(AddComponentOnStart = false, DontDestroyOnLoad = true)]
    public class PlayerDialogNodeMenuCom : MonoBehaviour
    {
        public PlayerDialogNodeMenuCom(IntPtr pointer) : base(pointer)
        {
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

        private PlayerDialogNodeMenu m_current_menu;

        private PlayerDialogNodeMenu m_master_menu;

        private PlayerDialogNodeMenu m_exploration_menu;
        private PlayerDialogNodeMenu m_combat_menu;

        private PlayerDialogNodeMenu m_apex_combat;
        private PlayerDialogNodeMenu m_misc_menu;
        private PlayerDialogNodeMenu m_decon_unit_menu;

        private void SetupNodes()
        {
            
            SetupExplorationMenu();
            SetupCombatMenu();
            SetupApexMenu();
            SetupMiscMenu();
            SetupDeconUnitMenu();

            SetupMasterMenu();
        }

        private void SetupDeconUnitMenu()
        {
            m_decon_unit_menu = new PlayerDialogNodeMenu();
            m_decon_unit_menu.AddNode(new PlayerDialogNode
            {
                Label = "Decon Unit Briefing",
                DialogID = GD.PlayerDialog.decon_unit_briefing
            });
            m_decon_unit_menu.AddNode(new PlayerDialogNode
            {
                Label = "Decon Unit About to Grab",
                DialogID = GD.PlayerDialog.decon_unit_about_to_grab
            });
            m_decon_unit_menu.AddNode(new PlayerDialogNode
            {
                Label = "Decon Unit Grabbed",
                DialogID = GD.PlayerDialog.decon_unit_grabbed
            });
            m_decon_unit_menu.AddNode(new PlayerDialogNode
            {
                Label = "Decon Unit Stay Close",
                DialogID = GD.PlayerDialog.decon_unit_stay_close_reminder
            });
            m_decon_unit_menu.AddNode(new PlayerDialogNode
            {
                Label = "Decon Unit Left Behind",
                DialogID = GD.PlayerDialog.decon_unit_left_behind
            });
        }

        private void SetupMiscMenu()
        {
            m_misc_menu = new PlayerDialogNodeMenu();
            m_misc_menu.AddNode(new PlayerDialogNode
            {
                Label = "Cut First Lock",
                DialogID = GD.PlayerDialog.cut_lock_first
            });
            m_misc_menu.AddNode(new PlayerDialogNode
            {
                Label = "Cut Final Lock",
                DialogID = GD.PlayerDialog.cut_lock_final
            });
            m_misc_menu.AddNode(new PlayerDialogNode
            {
                Label = "Snatcher Warning",
                DialogID = GD.PlayerDialog.warn_about_tentacles
            });
            m_misc_menu.AddNode(new PlayerDialogNode
            {
                Label = "Waypoint to checkpoint activated",
                DialogID = GD.PlayerDialog.waypoint_to_checkpoint_activated
            });
            m_misc_menu.AddNode(new PlayerDialogNode
            {
                Label = "Waypoint to elevator activated",
                DialogID = GD.PlayerDialog.waypoint_to_elevator_activated
            });
            
        }

        private void SetupApexMenu()
        {
            m_apex_combat = new PlayerDialogNodeMenu();
            m_apex_combat.AddNode(new PlayerDialogNode
            {
                Label = "Attracted Monsters (Accident)",
                DialogID = GD.PlayerDialog.attracted_monsters_accident
            });
            m_apex_combat.AddNode(new PlayerDialogNode
            {
                Label = "Attracted Monsters (Intentional)",
                DialogID = GD.PlayerDialog.attracted_monsters_intentional
            });
            m_apex_combat.AddNode(new PlayerDialogNode
            {
                Label = "Apex Door Fight Anticipation",
                DialogID = GD.PlayerDialog.apex_door_fight_anticipation
            });
            m_apex_combat.AddNode(new PlayerDialogNode
            {
                Label = "Apex Door To CheckPoint Spot",
                DialogID = GD.PlayerDialog.apex_door_to_checkpoint_spot
            });
            m_apex_combat.AddNode(new PlayerDialogNode
            {
                Label = "Apex Door To Elevator Spot",
                DialogID = GD.PlayerDialog.apex_door_to_elevator_spot
            });
        }

        private void SetupCombatMenu()
        {
            m_combat_menu = new PlayerDialogNodeMenu();

            m_combat_menu.AddNode(new PlayerDialogNode
            {
                Label = "Combat Start",
                DialogID = GD.PlayerDialog.combat_start
            });

            m_combat_menu.AddNode(new PlayerDialogNode
            {
                Label = "Combat Chatter",
                DialogID = GD.PlayerDialog.idle_combat
            });

            m_combat_menu.AddNode(new PlayerDialogNode
            {
                Label = "Taking Damage depleted mag",
                DialogID = GD.PlayerDialog.ammo_depleted_taking_damage
            });

            m_combat_menu.AddNode(new PlayerDialogNode
            {
                Label = "On Grabbed by Tank",
                DialogID = GD.PlayerDialog.on_grabbed_by_tank
            });

            m_combat_menu.AddNode(new PlayerDialogNode
            {
                Label = "Held by Tank",
                DialogID = GD.PlayerDialog.held_by_tank
            });

            m_combat_menu.AddNode(new PlayerDialogNode
            {
                Label = "Damage Grunt",
                DialogID = GD.PlayerDialog.after_damage_generic
            });

            m_combat_menu.AddNode(new PlayerDialogNode
            {
                Label = "Damage Grunt (Low Health)",
                DialogID = GD.PlayerDialog.low_health_limit
            });
        }

        private void SetupMasterMenu()
        {
            m_master_menu = new PlayerDialogNodeMenu();

            m_master_menu.AddNode(new PlayerDialogNode
            {
                Label = "Exploration",
                ChildMenu = this.m_exploration_menu
            });

            m_master_menu.AddNode(new PlayerDialogNode
            {
                Label = "Combat",
                ChildMenu = this.m_combat_menu
            });

            m_master_menu.AddNode(new PlayerDialogNode
            {
                Label = "Apex",
                ChildMenu = this.m_apex_combat
            });
            m_master_menu.AddNode(new PlayerDialogNode
            {
                Label = "Decon Unit",
                ChildMenu = this.m_decon_unit_menu
            });
            m_master_menu.AddNode(new PlayerDialogNode
            {
                Label = "Misc..",
                ChildMenu = this.m_misc_menu
            });
        }

        private void SetupExplorationMenu()
        {
            m_exploration_menu = new PlayerDialogNodeMenu();

            m_exploration_menu.AddNode(new PlayerDialogNode
            {
                Label = "Random Comment (Combat Potential)",
                DialogID = GD.PlayerDialog.random_comment_combat_potential
            });
            m_exploration_menu.AddNode(new PlayerDialogNode
            {
                Label = "Random Comment (Pure Stealth)",
                DialogID = GD.PlayerDialog.random_comment_pure_stealth
            });
            m_exploration_menu.AddNode(new PlayerDialogNode
            {
                Label = "Idle Low Health (Grunt)",
                DialogID = GD.PlayerDialog.idle_low_health
            });
            m_exploration_menu.AddNode(new PlayerDialogNode
            {
                Label = "Idle Low Health (Talk)",
                DialogID = GD.PlayerDialog.low_health_talk
            });

            m_exploration_menu.AddNode(new PlayerDialogNode
            {
                Label = "Sneeze",
                DialogID = GD.PlayerDialog.sneeze
            });
            m_exploration_menu.AddNode(new PlayerDialogNode
            {
                Label = "Cough (Soft)",
                DialogID = GD.PlayerDialog.cough_soft
            });
            m_exploration_menu.AddNode(new PlayerDialogNode
            {
                Label = "Cough (Hard)",
                DialogID = GD.PlayerDialog.cough_hard
            });
            m_exploration_menu.AddNode(new PlayerDialogNode
            {
                Label = "We are Lost",
                DialogID = GD.PlayerDialog.idle_no_progress
            });
        }

        

        


        private bool m_showMenu;

        private GUIStyle m_normalStyle;

        private GUIStyle m_selectedStyle;

        private void OnGUI()
        {
            if (!this.m_showMenu)
            {
                return;
            }

            this.m_current_menu.OnGUI(this.m_selectedStyle, this.m_normalStyle);          
        }

        private void Update()
        {
            UpdateToggleMenu();
            if (!this.m_showMenu)
            {
                return;
            }
            if (this.m_current_menu != null)
            {
                this.m_current_menu.Update();
                this.UpdateSelectNode();
            }

        }

       

        private void UpdateToggleMenu()
        {
            if (Input.GetKeyDown(KeyCode.Home))
            {
                m_showMenu = !m_showMenu;
                if (m_showMenu)
                {
                    this.m_current_menu = this.m_master_menu;
                }
            }
        }

        private void UpdateSelectNode()
        {
            if (!this.m_showMenu)
            {
                return;
            }
            if (!Input.GetKeyDown(KeyCode.End))
            {
                return;
            }


            if (this.m_current_menu == null || this.m_current_menu.CurrentNode == null)
            {
                return;
            }

            if (this.m_current_menu.CurrentNode.ChildMenu != null)
            {
                this.m_current_menu = this.m_current_menu.CurrentNode.ChildMenu;
                return;
            }
            

            if (PlayerManager.TryGetLocalPlayerAgent(out var playerAgent))
            {
                playerAgent.WantToStartDialog(this.m_current_menu.CurrentNode.DialogID, true);
            }

            this.m_showMenu = false;
        }



    }
}
