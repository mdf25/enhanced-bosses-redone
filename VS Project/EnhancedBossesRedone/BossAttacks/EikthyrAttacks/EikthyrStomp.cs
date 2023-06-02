using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.EikthyrAttacks
{
    public class EikthyrStomp : CustomAttack
    {
        public EikthyrStomp()
        {
            name = "Eikthyr_stomp";
            baseName = "Eikthyr_stomp";
            bossName = "Eikthyr";
            stopOriginalAttack = false;
        }

        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_attackForce = 40f;
        }
    }
}
