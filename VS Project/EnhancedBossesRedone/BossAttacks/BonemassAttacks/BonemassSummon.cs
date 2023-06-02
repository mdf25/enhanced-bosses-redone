using EnhancedBossesRedone.Abstract;
using System.Collections.Generic;

namespace EnhancedBossesRedone.BossAttacks.BonemassAttacks
{
    public class BonemassSummon : SummonAttack
    {
        public BonemassSummon()
        {
            name = "bonemass_summon";
            baseName = "bonemass_attack_aoe";
            bossName = "Bonemass";
            stopOriginalAttack = false;
            prefabs = new List<string>() { "vfx_ghost_death" };
            minRadius = 5f;
            radius = 7f;
        }

        public override void AdjustAttackParametersLate()
        {
            attack!.m_attackProjectile = ZNetScene.instance.GetPrefab("eb_bonemass_aoe_yellow");
        }
    }
}
