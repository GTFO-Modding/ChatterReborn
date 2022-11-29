using GameData;

namespace ChatterReborn.Data
{
    public enum eTextCommandID : uint
    {
        Follow = GD.Text.PlayerDialogData_CommunicationList_OnMe,
        Carry = GD.Text.PlayerDialogData_CommunicationList_GrabTheItem,
        DeploySentry = GD.Text.PlayerDialogData_CommunicationList_DeploySentryGun,
        DeployMines = GD.Text.PlayerDialogData_CommunicationList_DeployTripMine,
        Scan = GD.Text.PlayerDialogData_CommunicationList_Scan,
        CfoamHere = GD.Text.PlayerDialogData_CommunicationList_UseGlue
    }
}
