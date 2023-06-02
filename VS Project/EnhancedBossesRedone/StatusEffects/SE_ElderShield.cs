using EnhancedBossesRedone.Abstract;
using UnityEngine;

namespace EnhancedBossesRedone.StatusEffects
{
    public class SE_ElderShield : SE_CustomShield
    {
        public SE_ElderShield()
        {
            name = "SE_ElderShield";
            shieldPrefabName = "vfx_GoblinShield";
            hitEffectPrefabName = "fx_GoblinShieldHit";
            destroyEffectPrefabName = "fx_GoblinShieldBreak";
            shieldTint = new Color(0.1f, 0.2f, 0.1f, 0.04f);
        }

        public override void Setup(Character character)
        {
            shieldScale = 5f * character.transform.localScale.magnitude;
            shieldSpawnOffset = Vector3.up * character.transform.localScale.magnitude;
            base.Setup(character);

        }
    }
}
