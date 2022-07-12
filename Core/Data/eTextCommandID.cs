using GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatterReborn.Data
{
    public enum eTextCommandID : uint
    {
        Follow = GD.Text.InGame.CommunicationList.Dialog.On_me,
        Carry = GD.Text.InGame.CommunicationList.Dialog.Grab_the_item,
        DeploySentry = GD.Text.InGame.CommunicationList.Dialog.PlaceSentry,
        DeployMines = GD.Text.InGame.CommunicationList.Dialog.PlaceTripMine,
        Scan = GD.Text.InGame.CommunicationList.Dialog.UseEnemyScanner,
        CfoamHere = GD.Text.InGame.CommunicationList.Dialog.UseGlue
    }
}
