using System.Collections.Generic;
using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.Entity
{
    public class ModerHatchling : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = zNetScene.GetPrefab("Hatchling").Clone("eb_hatchling");
            entity.transform.SetParent(Main.Holder!.transform, false);

            Character character = entity.GetComponent<Character>();
            character.m_health = ConfigManager.ModerHatchlingHealth!.Value;

            CharacterDrop characterDrop = entity.GetComponent<CharacterDrop>();
            characterDrop.m_dropsEnabled = false;
            characterDrop.m_drops = new List<CharacterDrop.Drop>();

            Humanoid humanoid = entity.GetComponent<Humanoid>();
            humanoid.m_health = ConfigManager.ModerHatchlingHealth.Value;

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
        }
    }
}
