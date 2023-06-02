using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Bosses;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.EikthyrAttacks
{
    public class EikthyrSummon : SummonAttack
    {
        public EikthyrSummon()
        {
            name = "Eikthyr_summon";
            baseName = "Eikthyr_charge";
            bossName = "Eikthyr";
            stopOriginalAttack = true;
        }

        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_aiAttackRangeMin = 0f;
            itemDrop!.m_itemData.m_shared.m_aiAttackRange = 30f;
            itemDrop!.m_itemData.m_shared.m_aiPrioritized = true;
        }

        public override void SpawnOne(Character character, MonsterAI monsterAI, out Vector3 effectPos, out Character? spawned)
        {
            base.SpawnOne(character, monsterAI, out effectPos, out spawned);
            Eikthyr.Thunder(character, effectPos);
        }
    }
}
