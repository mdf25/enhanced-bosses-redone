using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.QueenAttacks
{
    public class QueenRush : CustomAttack
    {
        public QueenRush()
        {
            name = "SeekerQueen_Rush";
            baseName = "SeekerQueen_Rush";
            bossName = "SeekerQueen";
            stopOriginalAttack = false;
        }
    }
}
