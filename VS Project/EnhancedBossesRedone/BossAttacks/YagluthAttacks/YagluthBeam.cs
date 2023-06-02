using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.YagluthAttacks
{
    public class YagluthBeam : CustomAttack
    {
        public YagluthBeam()
        {
            name = "GoblinKing_Beam";
            baseName = "GoblinKing_Beam";
            bossName = "GoblinKing";
            stopOriginalAttack = false;
        }
    }
}
