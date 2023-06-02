using System.Collections.Generic;
using UnityEngine;
using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.Item;
using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.Entity
{
    public class Helboar : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = zNetScene.GetPrefab("Boar").Clone("eb_helboar");
            entity.transform.SetParent(Main.Holder!.transform, false);
            entity.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            entity.ApplyTint(new Color(0.4f, 0.1f, 0.2f));

            Character character = entity.GetComponent<Character>();
            character.m_health = ConfigManager.EikthyrHelsvinHealth!.Value;
            character.m_name = "Helboar";

            CharacterDrop characterDrop = entity.GetComponent<CharacterDrop>();
            characterDrop.m_dropsEnabled = false;
            characterDrop.m_drops = new List<CharacterDrop.Drop>();

            Humanoid humanoid = entity.GetComponent<Humanoid>();
            humanoid.m_health = ConfigManager.EikthyrHelsvinHealth.Value;
            humanoid.m_name = "Helboar";

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
                ObjectDB.instance.GetItemPrefab(HelboarAttack.name),
            };
            humanoid.m_defaultItems = attack.ToArray();
        }
    }
}
