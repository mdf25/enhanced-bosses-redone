using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.QueenAttacks
{
    public class QueenSlap : CustomAttack
    {
        public QueenSlap()
        {
            name = "SeekerQueen_Slap";
            baseName = "SeekerQueen_Slap";
            bossName = "SeekerQueen";
            stopOriginalAttack = false;
        }
    }
}
