using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;
using System;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.EikthyrAttacks
{
    public class EikthyrClones : CustomAttack
    {
        public EikthyrClones()
        {
            name = "Eikthyr_clones";
            baseName = "Eikthyr_stomp";
            bossName = "Eikthyr";
            stopOriginalAttack = true;
        }

        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }

            Vector3 position = character.transform.position;
            for (int i = 0; i < Mathf.Clamp(ConfigManager.EikthyrCloneMax!.Value, 1, 5); i++)
            {
                Vector3 position2 = (position + Helpers.InsideAnnulusXZ(minRadius, radius)).ConvertToWorldHeight();
                SpawnClone(character, position2);
            }
            Eikthyr.Thunder(character, position);
            character.Heal(GetHeal(), true);
        }

        public void SpawnClone(Character character, Vector3 position)
        {
            GameObject prefab = ZNetScene.instance.GetPrefab("eb_heldyr");
            GameObject gameObject = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);
            Character component = gameObject.GetComponent<Character>();
            DateTime time = ZNet.instance.GetTime();
            component.m_nview.GetZDO().Set("spawn_time", time.Ticks);
            CharacterTimedDestruction characterTimedDestruction = gameObject.AddComponent<CharacterTimedDestruction>();
            if (characterTimedDestruction != null)
            {
                characterTimedDestruction.m_timeoutMin = lifetime;
                characterTimedDestruction.m_timeoutMax = lifetime;
                characterTimedDestruction.Trigger();
            }
            component.m_baseAI.SetHuntPlayer(true);
            Eikthyr.Thunder(character, position, false);
        }

        public float minRadius = 5f;
        public float radius = 10f;
        public float lifetime = 30f;
    }
}
