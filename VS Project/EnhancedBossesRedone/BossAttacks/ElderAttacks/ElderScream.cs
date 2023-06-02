using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.ElderAttacks
{
    public class ElderScream : CustomAttack
    {
        public ElderScream()
        {
            name = "gd_king_scream";
            baseName = "gd_king_scream";
            bossName = "gd_king";
            stopOriginalAttack = false;
        }
    }
}
