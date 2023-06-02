using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.EikthyrAttacks
{
    public class EikthyrVortex : CustomAttack
    {
        public EikthyrVortex()
        {
            name = "Eikthyr_vortex";
            baseName = "Eikthyr_charge";
            bossName = "Eikthyr";
            stopOriginalAttack = true;
        }

        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }

            Vector3 position = character.transform.position + character.transform.forward * 4f + Vector3.up;
            Vector3 windPos = position - Vector3.up;
            GameObject prefab = ZNetScene.instance.GetPrefab("eb_thundervortex");
            ExtendedTimedDestruction component = prefab.GetComponent<ExtendedTimedDestruction>();
            component.m_timeout = 4f;
            component.m_audiotimeout = 3f;
            component.m_pstimeout = 2.5f;
            GameObject gameObject = Object.Instantiate(prefab, position, Quaternion.identity);
            Vortex component2 = gameObject.GetComponent<Vortex>();
            component2.character = character;

            Object.Instantiate(ZNetScene.instance.GetPrefab("vfx_RockDestroyed_large"), windPos, Quaternion.identity);
            Object.Instantiate(ZNetScene.instance.GetPrefab("sfx_gdking_rock_destroyed"), windPos, Quaternion.identity);
            GameObject plume = Object.Instantiate(ZNetScene.instance.GetPrefab("vfx_dragon_coldbreath"), windPos, Quaternion.Euler(-90f, 0f, 0f));
            plume.ApplyTint(new Color(0.1f, 0.1f, 0.4f, 0.2f));
        }

        public override bool CanUseAttack(Character character, MonsterAI monsterAI)
        {
            return base.CanUseAttack(character, monsterAI) ? Player.IsPlayerInRange(character.transform.position, 10f) : false;
        }
    }
}
