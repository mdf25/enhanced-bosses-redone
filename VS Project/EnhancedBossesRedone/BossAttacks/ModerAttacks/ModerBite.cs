using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.ModerAttacks
{
    public class ModerBite : CustomAttack
    {
        public ModerBite()
        {
            name = "dragon_bite";
            baseName = "dragon_bite";
            bossName = "Dragon";
            stopOriginalAttack = false;
        }
    }
}
