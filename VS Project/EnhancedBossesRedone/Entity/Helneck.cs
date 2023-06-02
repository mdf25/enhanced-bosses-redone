using System.Collections.Generic;
using UnityEngine;
using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Item;

namespace EnhancedBossesRedone.Entity
{
    public class Helneck : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = zNetScene.GetPrefab("Neck").Clone("eb_helneck");
            entity.transform.SetParent(Main.Holder!.transform, false);
            entity.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            entity.ApplyTint(new Color(0.4f, 0.1f, 0.2f));

            Character character = entity.GetComponent<Character>();
            character.m_health = ConfigManager.EikthyrHelneckHealth!.Value;
            character.m_name = "Helneck";

            CharacterDrop characterDrop = entity.GetComponent<CharacterDrop>();
            characterDrop.m_dropsEnabled = false;
            characterDrop.m_drops = new List<CharacterDrop.Drop>();

            Humanoid humanoid = entity.GetComponent<Humanoid>();
            humanoid.m_health = ConfigManager.EikthyrHelneckHealth.Value;
            humanoid.m_name = "Helneck";

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

            List<GameObject> attacks = new List<GameObject>
            {
                ObjectDB.instance.GetItemPrefab(HelneckAttack.name)
            };

            humanoid.m_defaultItems = attacks.ToArray();
        }
    }


}
