using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;
using System.Collections;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.BonemassAttacks
{
    public class BonemassRockSlam : CustomAttack
    {
        /**
		 * Setup attack
		 */
        public BonemassRockSlam()
        {
            name = "bonemass_rock_slam";
            baseName = "bonemass_attack_punch";
            bossName = "Bonemass";
            stopOriginalAttack = true;
        }

        /**
		 * Summon rocks if enemy is found.
		 */
        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }

            Bonemass? bonemass = character.GetBoss() as Bonemass;
            Character target;
            if (bonemass == null || (target = bonemass.monsterAI!.GetTargetCreature()) == null)
            {
                return;
            }
            if (!BaseAI.IsEnemy(character, target))
            {
                return;
            }

            bonemass.StartCoroutine(SpawnRocks(character, target));
        }

        /**
		 * Set attack priorities and range.
		 */
        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_aiAttackRangeMin = 6f;
            itemDrop!.m_itemData.m_shared.m_aiAttackRange = 50f;
            itemDrop!.m_itemData.m_shared.m_aiPrioritized = true;
        }

        /**
		 * Spawn rock coroutine.
		 */
        private IEnumerator SpawnRocks(Character character, Character target)
        {
            Vector3 direction = target.transform.position - character.transform.position;
            int maxMagnitude = (int)direction.magnitude + 10;
            Vector3 normDirection = direction.normalized;

            for (int i = 4; i < maxMagnitude; i += 3)
            {
                Vector3[] directions = new Vector3[3];
                directions[0] = Quaternion.Euler(0, Random.Range(-5, -5), 0) * normDirection;
                directions[1] = Quaternion.Euler(0, Random.Range(-20, -35), 0) * normDirection;
                directions[2] = Quaternion.Euler(0, Random.Range(20, 35), 0) * normDirection;

                if (character == null || target == null)
                {
                    yield break;
                }

                for (int j = 0; j < directions.Length; j += 1)
                {
                    if (j > 0 && character.GetHealthPercentage() > 0.5 * GetHpThreshold())
                    {
                        break;
                    }

                    Vector3 roughPosition = character.transform.position + i * directions[j];
                    Vector3 spawnPosition = roughPosition.ConvertToWorldHeight() - 4f * Vector3.up;
                    GameObject rock = Object.Instantiate(ZNetScene.instance.GetPrefab("eb_greenrock"), spawnPosition, Quaternion.Euler(Random.Range(0, 4), Random.Range(0, 360), Random.Range(0, 4)));
                    rock.transform.localScale *= Random.Range(0.6f, 1.4f);
                }
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
