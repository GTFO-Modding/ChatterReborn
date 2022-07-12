using ChatterReborn.ChatterEvent;
using ChatterReborn.Data;
using ChatterReborn.Utils;
using GameData;
using Player;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Managers
{
    public class PlayerCommunicationManager : ChatterManager<PlayerCommunicationManager>, IChatterEventListener<TextCommandEvent>
    {
        protected override void PostSetup()
        {
            ChatterEventListenerHandler<TextCommandEvent>.RegisterListener(this);
        }

        private static void GetCommnunicationData(SNet_Player src, uint textId, SNet_Player dst, out bool hasDst, out bool hasSrc)
        {
            string log = "\n[PUI_CommunicationMenu.OnCommunicationReceived] Results:";
            hasDst = false;
            hasSrc = false;
            if (src != null)
            {
                log += "\n\tSourceAgent Name : " + src.NickName;
                log += "\n\tSourceAgent IsLocal : " + src.IsLocal;
                hasSrc = true;
            }
            if (dst != null)
            {
                log += "\n\tDestinationAgent Name : " + dst.NickName;
                log += "\n\tDestinationAgent IsLocal : " + dst.IsLocal;
                hasDst = true;
            }
            log += "\n\tTextId : " + textId;
            log += "\n\tTextName : " + TextDataBlock.GetBlockName(textId);
            Current.DebugPrint(log);
        }

        private bool CheckPlacement(PlayerAgent source)
        {
            return true;
        }

        public void OnChatterEvent(TextCommandEvent chatterEvent)
        {
            GetCommnunicationData(chatterEvent.source, chatterEvent.textId, chatterEvent.destination, out bool hasDst, out bool hasSrc);
            if (hasDst && hasSrc)
            {
                if (Il2cppUtils.Convert(chatterEvent.destination.PlayerAgent, out PlayerAgent dstPlayerAgent) && Il2cppUtils.Convert(chatterEvent.source.PlayerAgent, out PlayerAgent sourceAgent))
                {
                    if (!dstPlayerAgent.IsLocallyOwned)
                    {
                        return;
                    }
                    WeightHandler<uint> affirmHandler = WeightHandler<uint>.CreateWeightHandler();
                    affirmHandler.AddValue(GD.PlayerDialog.CL_Yes, 1f);
                    if (dstPlayerAgent.PlayerCharacterFilter != DialogCharFilter.Char_F)
                    {
                        affirmHandler.AddValue(GD.PlayerDialog.CL_WillDo, 5f);
                    }
                    affirmHandler.AddValue(GD.PlayerDialog.CL_IWillDoIt, 2f);


                    var followYou = WeightHandler<uint>.CreateWeightHandler();
                    followYou.AddValue(GD.PlayerDialog.CL_IllFollowYourLead, 2f);
                    if ((sourceAgent.Position - dstPlayerAgent.Position).magnitude < 8f)
                    {
                        followYou.AddValue(GD.PlayerDialog.CL_IllStayCloseToYou, 2f);
                    }
                    ExtendedPlayerManager.GetToolAmmo(dstPlayerAgent, out float toolAmmoRel);

                    bool commandUsesTool = (eTextCommandID)chatterEvent.textId == eTextCommandID.DeploySentry || (eTextCommandID)chatterEvent.textId == eTextCommandID.DeployMines;
                    if (toolAmmoRel <= 0f && commandUsesTool)
                    {
                        affirmHandler.Clear();
                        affirmHandler.AddValue(GD.PlayerDialog.CL_ICantDoThat, 1f);
                    }
                    else if (!this.CheckPlacement(sourceAgent))
                    {
                        affirmHandler.Clear();
                        affirmHandler.AddValue(GD.PlayerDialog.CL_ICantPlaceItThere, 1f);
                    }
                    uint dialogID = 0U;
                    switch ((eTextCommandID)chatterEvent.textId)
                    {
                        case eTextCommandID.Follow:
                            dialogID = followYou.Best.Value;
                            break;
                        case eTextCommandID.Carry:
                            dialogID = affirmHandler.Best.Value;
                            break;
                        case eTextCommandID.DeploySentry:
                            dialogID = affirmHandler.Best.Value;
                            break;
                        case eTextCommandID.DeployMines:
                            dialogID = affirmHandler.Best.Value;
                            break;
                        case eTextCommandID.Scan:
                            dialogID = affirmHandler.Best.Value;
                            break;
                        case eTextCommandID.CfoamHere:
                            dialogID = affirmHandler.Best.Value;
                            break;
                        default:
                            break;
                    }
                    if (dialogID > 0U && ConfigurationManager.ComResponseEnabled)
                    {
                        PrisonerDialogManager.DelayLocalDialogForced(1.25f, dialogID);
                    }
                }

            }
        }
    }
}
