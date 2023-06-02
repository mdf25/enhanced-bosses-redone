using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.ElderAttacks
{
    public class ElderTeleport : CustomAttack
    {
        public ElderTeleport()
        {
            name = "gd_king_teleport";
            baseName = "gd_king_scream";
            bossName = "gd_king";
            stopOriginalAttack = false;
        }

        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }

            Elder? elder = character.GetBoss() as Elder;
            if (elder == null)
            {
                return;
            }

            Object.Instantiate(ZNetScene.instance.GetPrefab("sfx_ghost_death"), character.transform.position, Quaternion.identity);

            coroutine = Teleport(character, monsterAI);
            elder.StartCoroutine(coroutine);
        }

        public IEnumerator Teleport(Character character, MonsterAI monsterAI)
        {
            Vector3 wavePosition = character.transform.position + character.transform.localScale.magnitude * new Vector3(0.0f, 3.0f, 0.0f);
            for (int i = 0; i < 4; i += 1)
            {
                yield return new WaitForSeconds(waveDelay);
                PlayTeleportWave(character, wavePosition);
            }

            float teleportDistance = Random.Range(minTeleportDistance, maxTeleportDistance);
            Vector3 roughPos = character.transform.position + teleportDistance * new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y).normalized;
            Vector3 teleportPos = roughPos.ConvertToWorldHeight();
            List<TreeBase> trees = Elder.FindNearTrees(teleportPos, 10f);
            foreach (TreeBase tree in trees)
            {
                TreeLog? log;
                Elder.DestroyTree(tree, out log);
            }

            character.transform.position = teleportPos;
            Object.Instantiate(ZNetScene.instance.GetPrefab("sfx_ghost_death"), teleportPos, Quaternion.identity);
            Vector3 newWavePosition = teleportPos + character.transform.localScale.magnitude * new Vector3(0.0f, 5.0f, 0.0f);
            for (int i = 0; i < 4; i += 1)
            {
                PlayTeleportWave(character, newWavePosition);
                yield return new WaitForSeconds(waveDelay);
            }
        }

        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_aiPrioritized = true;
            itemDrop!.m_itemData.m_shared.m_aiAttackRange = 10f;
            itemDrop!.m_itemData.m_shared.m_aiAttackRangeMin = 0f;
        }

        private void PlayTeleportWave(Character character, Vector3 position)
        {
            GameObject teleportWave = Object.Instantiate(ZNetScene.instance.GetPrefab("fx_sledge_demolisher_hit"), position, Quaternion.identity);
            float scaleValue = 8 * character.transform.localScale.magnitude;
            teleportWave.transform.localScale *= scaleValue; 
        }


        private IEnumerator? coroutine;

        private float waveDelay = 0.2f;
        public float minTeleportDistance = 20f;
        public float maxTeleportDistance = 30f;
    }
}
