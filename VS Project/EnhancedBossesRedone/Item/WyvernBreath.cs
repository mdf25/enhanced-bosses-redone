using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;

namespace EnhancedBossesRedone.Item
{
    public class WyvernBreath : ExternalItem
    {
        public override void Setup(ObjectDB objectDB)
        {
            base.Setup(objectDB);
            item = objectDB.GetItemPrefab("dragon_spit_shotgun").Clone(name);
            ItemDrop component = item.GetComponent<ItemDrop>();
            HitData.DamageTypes damages = new HitData.DamageTypes()
            {
                m_frost = ConfigManager.ModerWyvernProjectileFrostDamage!.Value,
                m_blunt = ConfigManager.ModerWyvernProjectileBluntDamage!.Value,
            };
            component.m_itemData.m_shared.m_damages = damages;
            component.m_itemData.m_shared.m_attack.m_projectileBursts = ConfigManager.ModerWyvernProjectilesLaunched!.Value;
            component.m_itemData.m_shared.m_attack.m_projectileFireTimer = 0.3f;
            component.m_itemData.m_shared.m_attack.m_projectileAccuracy = 0.8f;
            component.m_itemData.m_shared.m_attack.m_projectileAccuracyMin = 0.7f;
        }

        public static new string name = "eb_wyvern_breath";
    }
}
