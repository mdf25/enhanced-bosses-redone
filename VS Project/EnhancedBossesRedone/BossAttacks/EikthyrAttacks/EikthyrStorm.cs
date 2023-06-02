using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;
using System.Collections;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.EikthyrAttacks
{
    public class EikthyrStorm : CustomAttack
    {
        /**
		 * Generates storm clouds around Eikthyr that randomly move about.
		 */
        public EikthyrStorm()
        {
            name = "Eikthyr_storm";
            baseName = "Eikthyr_stomp";
            bossName = "Eikthyr";
            stopOriginalAttack = true;
        }

        /**
		 * Sets this attack to priority so Eikthyr tries to trigger more often.
		 */
        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_aiPrioritized = true;
            itemDrop!.m_itemData.m_shared.m_aiAttackRange = 25f;
        }

        /**
		 * Generate storm clouds over time.
		 */
        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }

            Eikthyr? boss = character.GetBoss() as Eikthyr;
            if (boss == null)
            {
                return;
            }
            boss.StartCoroutine(SpawnThunderclouds(character));

            Object.Instantiate(ZNetScene.instance.GetPrefab("vfx_RockDestroyed_large"), character.transform.position, Quaternion.identity);
            Object.Instantiate(ZNetScene.instance.GetPrefab("sfx_gdking_rock_destroyed"), character.transform.position, Quaternion.identity);

            for (int i = -1; i <= 1; i += 1)
            {
                for (int j = -1; j <= 1; j += 1)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    Vector3 position = character.transform.position + 4.5f * new Vector3(i, 0, j).normalized;
                    GameObject plume = Object.Instantiate(ZNetScene.instance.GetPrefab("vfx_dragon_coldbreath"), position.ConvertToWorldHeight(), Quaternion.Euler(-90f, 0f, 0f));
                    plume.ApplyTint(new Color(0.2f, 0.2f, 0.6f, 0.2f));
                }
            }
        }

        /**
		 * Coroutine to spawn clouds.
		 */
        public IEnumerator SpawnThunderclouds(Character character)
        {
            for (int i = 0; i < ConfigManager.EikthyrStormClouds!.Value; i += 1)
            {
                if (character == null)
                {
                    yield break;
                }

                Vector3 roughSpawnPos = character.GetCenterPoint() + Helpers.InsideAnnulusXZ(5f, 15f);
                Vector3 cloudSpawnPos = roughSpawnPos.ConvertToWorldHeight() + 10f * Vector3.up;
                Object.Instantiate(ZNetScene.instance.GetPrefab("eb_thundercloud"), cloudSpawnPos, Quaternion.identity);
                yield return new WaitForSeconds(2.0f);
            }
        }
    }
}
