using System.Collections.Generic;
using UnityEngine;
using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Item;

namespace EnhancedBossesRedone.Entity
{
    public class Wyvern : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = zNetScene.GetPrefab("Dragon").Clone("eb_wyvern");
            entity.transform.SetParent(Main.Holder!.transform, false);
            entity.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            entity.ApplyTint(new Color(0.8f, 0.9f, 1.0f));

            BaseAI baseAI = entity.GetComponent<BaseAI>();
            baseAI.m_chanceToLand = 0;
            baseAI.m_deathMessage = "";

            MonsterAI monsterAI = entity.GetComponent<MonsterAI>();
            monsterAI.m_chanceToLand = 0;
            monsterAI.m_deathMessage = "";

            Character character = entity.GetComponent<Character>();
            character.m_health = ConfigManager.ModerWyvernHealth!.Value;
            character.m_name = "Wyvern";
            character.m_boss = false;
            character.m_bossEvent = "";
            character.m_defeatSetGlobalKey = "";
            character.m_flyFastSpeed = 6.0f;
            character.m_flySlowSpeed = 4.5f;

            CharacterDrop characterDrop = entity.GetComponent<CharacterDrop>();
            characterDrop.m_dropsEnabled = false;
            characterDrop.m_drops = new List<CharacterDrop.Drop>();

            Humanoid humanoid = entity.GetComponent<Humanoid>();
            humanoid.m_health = ConfigManager.ModerWyvernHealth.Value;
            humanoid.m_name = "Wyvern";
            humanoid.m_flyFastSpeed = 6.0f;
            humanoid.m_flySlowSpeed = 4.5f;

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
                ObjectDB.instance.GetItemPrefab(WyvernBreath.name)
            };
            humanoid.m_defaultItems = attack.ToArray();
        }
    }
}
