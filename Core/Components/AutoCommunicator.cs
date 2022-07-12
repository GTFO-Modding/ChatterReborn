using Agents;
using ChatterReborn.Attributes;
using ChatterReborn.Utils;
using Enemies;
using GameData;
using Il2CppInterop.Runtime.Attributes;
using Player;
using UnityEngine;

namespace ChatterReborn.Components
{
    [IL2CPPType]
    public class AutoCommunicator : MonoBehaviour
    {

        void Awake()
        {
            this.LocalPlayer = this.GetComponent<LocalPlayerAgent>();
            ChatterDebug.LogMessage("Setting up an AutoCommunicator For LocalPlayer");
        }

        void Update()
        {
            UpdateStealthInteraction();
            UpdateSteathInteractionPrompt();
            UpdateStealthInteractionInput();
        }

        [HideFromIl2Cpp]
        private void UpdateSteathInteractionPrompt()
        {
            if (this.stealthTargetAquired)
            {
                if (this.LocalPlayer.m_interactionCommunicator.m_lastReceiveTime == 0f)
                {
                    if (!PlayerInteraction.InteractionEnabled && !PlayerInteraction.LadderInteractionEnabled && !PlayerInteraction.CameraRayInteractionEnabled)
                    {
                        if (GuiManager.InteractionLayer.InteractPromptVisible != this.stealthTargetAquired)
                        {
                            EnableInteractionPrompt(true, true);
                        }                        
                    }
                }                              
            }            
        }

        [HideFromIl2Cpp]
        private void UpdateStealthInteraction()
        {
            if (this.TryToAquireTargetEnemy(out var newEnemyTarget))
            {
                m_targetAgent = newEnemyTarget;
                EnableInteractionPrompt(true);
                return;
            }

            EnableInteractionPrompt(false);
        }


        private void UpdateStealthInteractionInput()
        {
            if (!stealthTargetAquired)
            {
                return;
            }

            if (FocusStateManager.CurrentState != eFocusState.FPS)
            {
                return;
            }


            /*if (InputMapper.GetButtonDown.Invoke(InputAction.NavMarkerPing, eFocusState.FPS))
            {
                this.LocalPlayer.WantToStartDialog(GD.PlayerDialog.CL_AreYouReady, true);
            }*/

            if (InputMapper.GetButtonDown.Invoke(InputAction.Use, eFocusState.FPS))
            {
                StartCountDownCommand();
            }
        }

        private void StartCountDownCommand()
        {
            if (!m_countDownStarted || m_countDownTimer < Clock.Time)
            {
                m_countDownStarted = true;
                m_countDownTimer = Time.time + 3f;
                this.LocalPlayer.WantToStartDialog(GD.PlayerDialog.CL_ThreeTwoOneGo, true);
                return;
            }

            m_countDownStarted = false;
            this.LocalPlayer.WantToStartDialog(GD.PlayerDialog.CL_CancelThat, true);
        }

        private bool m_countDownStarted;

        private float m_countDownTimer;


        [HideFromIl2Cpp]
        private bool isEnemyAgentValidForAquiring(EnemyAgent enemyAgent)
        {
            return enemyAgent != null && enemyAgent.Alive && enemyAgent.AI != null && enemyAgent.AI.Mode != AgentMode.Agressive;
        }

        [HideFromIl2Cpp]
        private bool TryToAquireTargetEnemy(out EnemyAgent targetAgent)
        {
            targetAgent = null;
            if (!GameStateManager.IsInExpedition)
            {
                return false;
            }            

            if (this.LocalPlayer == null)
            {
                return false;
            }
            if (!this.LocalPlayer.Alive)
            {
                return false;
            }


            if (this.LocalPlayer.FPSCamera == null)
            {
                return false;
            }

            if (this.LocalPlayer.FPSCamera.CameraRayObject == null)
            {
                return false;
            }

            if (this.LocalPlayer.FPSCamera.CameraRayDist > 3f)
            {
                return false;
            }

            var enemytarget = this.LocalPlayer.FPSCamera.CameraRayObject.GetAbsoluteComponent<EnemyAgent>();
            if (isEnemyAgentValidForAquiring(enemytarget))
            {
                targetAgent = enemytarget;
            }

            return targetAgent != null;
        }


        [HideFromIl2Cpp]
        private void AquireTarget(EnemyAgent enemyAgent)
        {
            m_targetAgent = enemyAgent;
        }

        private string GetStealthMessage()
        {
            string message = string.Empty;
            var stealthMessage = new string[]
            {
                //"Press <color=yellow>$PING_BTN:</color> To Ask if Ready",
                "Press <color=yellow>$USE_BTN:</color> CountDown/Cancel"
            };
            

            for (int i = 0; i < stealthMessage.Length; i++)
            {
                message += stealthMessage[i];
                if (i < stealthMessage.Length - 1)
                {
                    message += "\n";
                }
            }

            message = message.Replace("$PING_BTN:", InputMapper.GetBindingName(InputAction.NavMarkerPing));
            message = message.Replace("$USE_BTN:", InputMapper.GetBindingName(InputAction.Use));

            return message;
        }


        [HideFromIl2Cpp]
        private void EnableInteractionPrompt(bool enabled = false, bool force = false)
        {
            if (!force && stealthTargetAquired == enabled)
            {
                return;
            }

            stealthTargetAquired = enabled;
            if (stealthTargetAquired)
            {
                GuiManager.InteractionLayer.SetInteractPrompt("Stealth Commands", GetStealthMessage(), ePUIMessageStyle.Default);
            }
            GuiManager.InteractionLayer.InteractPromptVisible = stealthTargetAquired;
        }

        [HideFromIl2Cpp]
        private LocalPlayerAgent LocalPlayer { get; set; }

        private EnemyAgent m_targetAgent;

        private bool stealthTargetAquired;

    }
}
