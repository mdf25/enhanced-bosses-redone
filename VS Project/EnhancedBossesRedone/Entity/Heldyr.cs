using System.Collections.Generic;
using UnityEngine;
using EnhancedBossesRedone.Item;
using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.Entity
{
    public class Heldyr : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = zNetScene.GetPrefab("Eikthyr").Clone("eb_heldyr");
            entity.transform.SetParent(Main.Holder!.transform, false);
            entity.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            entity.ApplyTint(new Color(0.4f, 0.1f, 0.2f));

            BaseAI baseAI = entity.GetComponent<BaseAI>();
            baseAI.m_deathMessage = "";

            MonsterAI monsterAI = entity.GetComponent<MonsterAI>();
            monsterAI.m_deathMessage = "";

            Character character = entity.GetComponent<Character>();
            character.m_health = ConfigManager.EikthyrHeldyrHealth!.Value;
            character.m_name = "Heldyr";
            character.m_boss = false;
            character.m_bossEvent = "";
            character.m_defeatSetGlobalKey = "";

            CharacterDrop characterDrop = entity.GetComponent<CharacterDrop>();
            characterDrop.m_dropsEnabled = false;
            characterDrop.m_drops = new List<CharacterDrop.Drop>();

            Humanoid humanoid = entity.GetComponent<Humanoid>();
            humanoid.m_health = ConfigManager.EikthyrHeldyrHealth.Value;
            humanoid.m_name = "Heldyr";

            EffectList effects = new EffectList();
            EffectList.EffectData deathvfx = new EffectList.EffectData();
            deathvfx.m_prefab = zNetScene.GetPrefab("vfx_ghost_death");
            deathvfx.m_enabled = true;
            EffectList.EffectData deathsfx = new EffectList.EffectData();
            deathsfx.m_prefab = zNetScene.GetPrefab("sfx_ghost_death");
            deathsfx.m_enabled = true;
            List<EffectList.EffectData> prefabs = new List<EffectList.EffectData>
            {
                deathvfx,
                deathsfx,
            };
            effects.m_effectPrefabs = prefabs.ToArray();
            humanoid.m_deathEffects = effects;

            List<GameObject> attack = new List<GameObject>
            {
                ObjectDB.instance.GetItemPrefab(HeldyrAttack.name)
            };
            humanoid.m_defaultItems = attack.ToArray();
        }
    }
}
