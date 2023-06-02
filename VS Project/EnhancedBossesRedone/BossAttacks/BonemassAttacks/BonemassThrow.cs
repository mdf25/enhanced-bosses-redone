using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.BonemassAttacks
{
    public class BonemassThrow : CustomAttack
    {
        public BonemassThrow()
        {
            name = "bonemass_attack_throw";
            baseName = "bonemass_attack_throw";
            bossName = "Bonemass";
            stopOriginalAttack = false;
        }
    }
}
