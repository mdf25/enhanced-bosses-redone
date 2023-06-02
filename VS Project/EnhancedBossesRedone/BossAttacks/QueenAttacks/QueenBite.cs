using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.QueenAttacks
{
    public class QueenBite : CustomAttack
    {
        public QueenBite()
        {
            name = "SeekerQueen_Bite";
            baseName = "SeekerQueen_Bite";
            bossName = "SeekerQueen";
            stopOriginalAttack = false;
        }
    }
}
