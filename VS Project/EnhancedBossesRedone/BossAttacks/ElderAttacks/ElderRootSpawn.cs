using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.ElderAttacks
{
    public class ElderRootSpawn : CustomAttack
    {
        public ElderRootSpawn()
        {
            name = "gd_king_rootspawn";
            baseName = "gd_king_rootspawn";
            bossName = "gd_king";
            stopOriginalAttack = false;
        }
    }
}
