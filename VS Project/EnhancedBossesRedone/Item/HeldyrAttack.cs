using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;

namespace EnhancedBossesRedone.Item
{
    public class HeldyrAttack : ExternalItem
    {
        public override void Setup(ObjectDB objectDB)
        {
            base.Setup(objectDB);
            item = objectDB.GetItemPrefab("Eikthyr_antler").Clone(name);
            ItemDrop component = item.GetComponent<ItemDrop>();
            HitData.DamageTypes damages = new HitData.DamageTypes()
            {
                m_blunt = ConfigManager.EikthyrHeldyrDamage!.Value,
            };
            component.m_itemData.m_shared.m_damages = damages;
        }

        public static new string name = "eb_heldyr_attack";
    }
}
