using EnhancedBossesRedone.Abstract;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.ElderAttacks
{
    public class ElderShoot : CustomAttack
    {
        public ElderShoot()
        {
            name = "gd_king_shoot";
            baseName = "gd_king_shoot";
            bossName = "gd_king";
            stopOriginalAttack = false;
        }

        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_aiPrioritized = false;
        }
    }
}
