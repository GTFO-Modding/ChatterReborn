using Player;

namespace ChatterReborn.Extra
{
    public class ItemPingDescriptor
    {

        public uint m_dialogID;

        public eNavMarkerStyle m_style;


        public virtual void TryToApplyAmmo(Item item)
        {
        }


        public virtual void PlayPingDialog(PlayerAgent playerAgent)
        {
            PlayerDialogManager.WantToStartDialogForced(this.m_dialogID, playerAgent);
        }



    }
}