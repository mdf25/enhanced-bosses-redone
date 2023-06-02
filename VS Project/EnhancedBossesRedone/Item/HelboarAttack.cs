using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;

namespace EnhancedBossesRedone.Item
{
    public class HelboarAttack : ExternalItem
    {
        public override void Setup(ObjectDB objectDB)
        {
            base.Setup(objectDB);
            item = objectDB.GetItemPrefab("boar_base_attack").Clone(name);
            ItemDrop component = item.GetComponent<ItemDrop>();
            HitData.DamageTypes damages = new HitData.DamageTypes()
            {
                m_blunt = ConfigManager.EikthyrHelsvinDamage!.Value,
            };
            component.m_itemData.m_shared.m_damages = damages;
        }

        public static new string name = "eb_helboar_attack";
    }
}
