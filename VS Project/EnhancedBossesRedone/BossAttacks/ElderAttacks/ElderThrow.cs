using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;

namespace EnhancedBossesRedone.BossAttacks.ElderAttacks
{
    internal class ElderThrow : CustomAttack
    {
        /**
		 * Allows Elder to heal when consuming nearby tree logs.
		 */
        public ElderThrow()
        {
            name = "gd_king_throw";
            baseName = "gd_king_shoot";
            bossName = "gd_king";
            stopOriginalAttack = true;
        }

        /**
		 * Checks whether Elder can find trees around the player or himself in his general look direction.
		 */
        public override bool CanUseAttack(Character character, MonsterAI monsterAI)
        {
            logsToThrow = new List<TreeLog>();
            if (monsterAI == null || !monsterAI.HaveTarget() || monsterAI.GetTargetCreature() == null)
            {
                return false;
            }

            logsToThrow = Elder.FindNearLogs(character.transform.position + searchDistance * character.GetLookDirXZ(), searchDistance);
            if (logsToThrow.Count > 0)
            {
                return base.CanUseAttack(character, monsterAI);
            }

            logsToThrow = Elder.FindNearLogs(monsterAI.GetTargetCreature().transform.position, targetSearchDistance);
            return (logsToThrow.Count > 0 && base.CanUseAttack(character, monsterAI));
        }

        /**
		 * Start the eat tree process on when attack concludes.
		 */
        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }

            int treeCount = (int)Mathf.Lerp(1, 5, 1 - Mathf.Min(character.GetHealthPercentage() / GetHpThreshold(), 1));
            EatTrees(character, treeCount);
        }

        /**
		 * Eat group of trees.
		 */
        public void EatTrees(Character character, int count = 1)
        {
            if (logsToThrow.Count == 0)
            {
                return;
            }

            int thrown = 0;
            foreach (TreeLog treeLog in logsToThrow)
            {
                if (thrown >= count)
                {
                    break;
                }

                if (treeLog == null)
                {
                    continue;
                }

                treeLog.gameObject.AddComponent<MoveForThrow>();
                thrown += 1;
            }
        }

        /**
		 * Set min and max parameters.
		 */
        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_aiAttackRange = 60f;
            itemDrop!.m_itemData.m_shared.m_aiAttackRangeMin = 25f;
            itemDrop!.m_itemData.m_shared.m_aiPrioritized = true;
        }

        private List<TreeLog> logsToThrow = new List<TreeLog>();
        private float searchDistance = 40f;
        private float targetSearchDistance = 10f;
    }
}
