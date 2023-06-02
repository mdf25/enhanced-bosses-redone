using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;

namespace EnhancedBossesRedone.BossAttacks.ModerAttacks
{
    public class ModerSpitShotgunFire : CustomAttack
    {
        public ModerSpitShotgunFire()
        {
            name = "dragon_spit_shotgun_fire";
            baseName = "dragon_spit_shotgun";
            bossName = "Dragon";
            stopOriginalAttack = false;
        }

        public override bool CanUseAttack(Character character, MonsterAI monsterAI)
        {
            Moder? moder = character.GetBoss() as Moder;
            if (moder == null)
            {
                return false;
            }

            return moder.IsInAncientState() && base.CanUseAttack(character, monsterAI);
        }

        public override void AdjustAttackParameters()
        {
            HitData.DamageTypes damages = new HitData.DamageTypes();
            damages.m_blunt = 50;
            damages.m_fire = 80;

            itemDrop!.m_itemData.m_shared.m_damages = damages;
        }

        public override void AdjustAttackParametersLate()
        {
            attack!.m_attackProjectile = ZNetScene.instance.GetPrefab("eb_dragon_fire_projectile");
        }
    }
}
