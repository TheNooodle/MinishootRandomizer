using UnityEngine;

namespace MinishootRandomizer
{
    public interface ILocationVisitor
    {
        IPatchAction VisitPickup(PickupLocation location, Item item);
        IPatchAction VisitShard(ShardLocation location, Item item);
        IPatchAction VisitCrystalNpc(CrystalNpcLocation location, Item item);
        IPatchAction VisitLinearShop(LinearShopLocation location, Item item);
        IPatchAction VisitParallelShop(ChoiceShopLocation location, Item item);
        IPatchAction VisitDestroyable(DestroyableLocation location, Item item);
        IPatchAction VisitDungeonReward(DungeonRewardLocation location, Item item);
    }
}
