using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;

namespace EnhancedBossesRedone.Item
{
    public class HelneckAttack : ExternalItem
    {
        public override void Setup(ObjectDB objectDB)
        {
            base.Setup(objectDB);
            item = objectDB.GetItemPrefab("Neck_BiteAttack").Clone(name);
            ItemDrop component = item.GetComponent<ItemDrop>();
            HitData.DamageTypes damages = new HitData.DamageTypes()
            {
                m_slash = ConfigManager.EikthyrHelneckDamage!.Value,
            };
            component.m_itemData.m_shared.m_damages = damages;
        }

        public static new string name = "eb_helneck_attack";
    }
}
