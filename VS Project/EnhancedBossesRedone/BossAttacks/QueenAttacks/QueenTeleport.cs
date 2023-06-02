using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.QueenAttacks
{
    public class QueenTeleport : CustomAttack
    {
        public QueenTeleport()
        {
            name = "SeekerQueen_Teleport";
            baseName = "SeekerQueen_Teleport";
            bossName = "SeekerQueen";
            stopOriginalAttack = false;
        }
    }
}
