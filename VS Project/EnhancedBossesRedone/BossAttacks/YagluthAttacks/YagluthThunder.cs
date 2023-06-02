using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.YagluthAttacks
{
    public class YagluthThunder : CustomAttack
    {
        /**
		 * Yagluth thunder strike.
		 */
        public YagluthThunder()
        {
            name = "GoblinKing_Thunder";
            baseName = "GoblinKing_Taunt";
            bossName = "GoblinKing";
            stopOriginalAttack = false;
        }

        /**
		 * Thunder strike can't be used if Yag is in rock formation
		 */
        public override bool CanUseAttack(Character character, MonsterAI monsterAI)
        {
            Yagluth? yagluth = character.GetBoss() as Yagluth;
            if (yagluth == null)
            {
                return false;
            }

            if (!yagluth.IsInRockFormation())
            {
                return base.CanUseAttack(character, monsterAI);
            }

            Character target = monsterAI.GetTargetCreature();
            if (target == null)
            {
                return false;
            }

            Vector3 position = target.transform.position;
            return position.GetHeightFromGround() < 3f && base.CanUseAttack(character, monsterAI);
        }

        /**
		 * Adjust attack range.
		 */
        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_aiAttackRangeMin = 5f;
        }

        /**
		 * Use thunder on triggered attack.
		 */
        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }

            List<Character> enemies = character.FindEnemies(50f);
            if (enemies.Count == 0)
            {
                return;
            }

            Character enemy = enemies[Random.Range(0, enemies.Count)];
            Vector3 position = enemy.transform.position.ConvertToWorldHeight();
            Yagluth.Thunder(character, position);
        }

        public float radius = 6f;
    }
}
