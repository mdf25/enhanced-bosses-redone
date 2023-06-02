using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.QueenAttacks
{
    public class QueenCall : CustomAttack
    {
        public QueenCall()
        {
            name = "SeekerQueen_Call";
            baseName = "SeekerQueen_Call";
            bossName = "SeekerQueen";
            stopOriginalAttack = false;
        }
    }
}
