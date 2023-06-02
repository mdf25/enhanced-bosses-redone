using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.BonemassAttacks
{
    public class BonemassPunch : CustomAttack
    {
        public BonemassPunch()
        {
            name = "bonemass_attack_punch";
            baseName = "bonemass_attack_punch";
            bossName = "Bonemass";
            stopOriginalAttack = false;
        }
    }
}
