using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.YagluthAttacks
{
    public class YagluthTaunt : CustomAttack
    {
        public YagluthTaunt()
        {
            name = "GoblinKing_Taunt";
            baseName = "GoblinKing_Taunt";
            bossName = "GoblinKing";
            stopOriginalAttack = false;
        }
    }
}
