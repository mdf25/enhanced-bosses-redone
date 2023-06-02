using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.QueenAttacks
{
    public class QueenSpit : CustomAttack
    {
        public QueenSpit()
        {
            name = "SeekerQueen_Spit";
            baseName = "SeekerQueen_Spit";
            bossName = "SeekerQueen";
            stopOriginalAttack = false;
        }
    }
}
