using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.ElderAttacks
{
    public class ElderPoisonBreath : CustomAttack
    {
        public ElderPoisonBreath()
        {
            name = "gd_king_poisonbreath";
            baseName = "gd_king_shoot";
            bossName = "gd_king";
            stopOriginalAttack = false;
        }

        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_aiAttackRange = 60f;
            itemDrop!.m_itemData.m_shared.m_aiAttackRangeMin = 10f;

            HitData.DamageTypes damages = new HitData.DamageTypes();
            damages.m_poison = 30.0f;
            itemDrop.m_itemData.m_shared.m_damages = damages;
        }

        public override void AdjustAttackParametersLate()
        {
            attack!.m_attackAnimation = "scream";
            attack!.m_attackProjectile = ZNetScene.instance.GetPrefab("eb_poisoncloud");

            

            //attack!.m_projectileAccuracy = 0.6f;
            //attack!.m_projectileAccuracyMin = 0.5f;
            //attack!.m_projectiles = 20;
            //attack!.m_projectileVel = 8.0f;
            //attack!.m_projectileFireTimer = 0.02f;
        }
    }
}
