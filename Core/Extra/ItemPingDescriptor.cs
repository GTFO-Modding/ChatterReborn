using ChatterReborn.Utils;
using Player;

namespace ChatterReborn.Extra
{
    public class ItemPingDescriptor
    {

        public WeightHandler<uint> DialogIDs; 

        public eNavMarkerStyle PingStyle;


        public virtual void TryToApplyAmmo(Item item)
        {
        }


        public virtual void PlayPingDialog(PlayerAgent playerAgent)
        {
            if (this.DialogIDs.TryToGetBestValue(out WeightValue<uint> best))
            {
                playerAgent.WantToStartDialog(best.Value, true);
            }
            
        }





    }
}