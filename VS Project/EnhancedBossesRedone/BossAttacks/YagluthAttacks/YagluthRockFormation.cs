using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.YagluthAttacks
{
    public class YagluthRockFormation : CustomAttack
    {
        /**
		 * Yagluth raise rocks.
		 */
        public YagluthRockFormation()
        {
            name = "GoblinKing_RockFormation";
            baseName = "GoblinKing_Nova";
            bossName = "GoblinKing";
            stopOriginalAttack = true;
        }

        /**
		 * Checks whether not in rock formation.
		 */
        public override bool CanUseAttack(Character character, MonsterAI monsterAI)
        {
            Yagluth? yagluth = character.GetBoss() as Yagluth;
            if (yagluth == null)
            {
                return false;
            }

            return !yagluth.IsInRockFormation() && base.CanUseAttack(character, monsterAI);
        }

        /**
		 * Adjust attack range.
		 */
        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_aiAttackRangeMin = 5f;
            itemDrop!.m_itemData.m_shared.m_aiAttackRange = 80f;
        }

        /**
		 * Add rocks when attack is triggered.
		 */
        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }

            Yagluth? yagluth;
            if ((yagluth = character.GetBoss() as Yagluth) == null)
            {
                return;
            }

            List<Character> enemies = character.FindEnemies(80f);
            if (enemies.Count == 0)
            {
                return;
            }

            Vector3 spawnUnderYag = character.transform.position - 2 * Vector3.up;

            float healthPerc = Mathf.Clamp(yagluth.character!.GetHealthPercentage() / GetHpThreshold(), 0, 1);
            int rocksPerTier = (int)Mathf.Max(3, this.rocksPerTier * (1 - healthPerc));
            float yagHeight = Mathf.Max(2, this.yagHeight * (1 - healthPerc));
            int tiers = Mathf.Max(2, (int)(this.tiers * (1 - healthPerc)));

            GameObject mainRock = Object.Instantiate(ZNetScene.instance.GetPrefab("eb_redrock"), spawnUnderYag, Quaternion.identity);
            mainRock.GetComponent<YagluthRockScript>().isYagluthRock = true;
            mainRock.GetComponent<YagluthRockScript>().rockHeight = yagHeight + 1.0f;

            yagluth.SetRockFormation(true);
            yagluth.StartCoroutine(yagluth.TriggerRockSpawns(rocksPerTier, radius, yagHeight, tiers));
        }

        public int rocksPerTier = 5;
        public float radius = 6.0f;
        public float yagHeight = 7.0f;
        public int tiers = 6;
    }
}
