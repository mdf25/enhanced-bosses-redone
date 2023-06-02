using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.ElderAttacks
{
    public class ElderSummon : SummonAttack
    {
        public ElderSummon()
        {
            name = "gd_king_summon";
            baseName = "gd_king_rootspawn";
            bossName = "gd_king";
            stopOriginalAttack = true;
        }
    }
}
