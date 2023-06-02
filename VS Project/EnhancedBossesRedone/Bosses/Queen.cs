using EnhancedBossesRedone.Abstract;
using UnityEngine;

namespace EnhancedBossesRedone.Bosses
{
    public class Queen : Boss
    {
        public Queen()
        {
            bossName = "SeekerQueen";
        }

        public override void PopulateDefaultValues()
        {
            HitData.DamageModifiers modifiers = new HitData.DamageModifiers();
            modifiers.m_blunt = HitData.DamageModifier.Normal;
            modifiers.m_slash = HitData.DamageModifier.Normal;
            modifiers.m_pierce = HitData.DamageModifier.Resistant;
            modifiers.m_chop = HitData.DamageModifier.Ignore;
            modifiers.m_pickaxe = HitData.DamageModifier.Ignore;
            modifiers.m_fire = HitData.DamageModifier.Normal;
            modifiers.m_frost = HitData.DamageModifier.Normal;
            modifiers.m_lightning = HitData.DamageModifier.Normal;
            modifiers.m_poison = HitData.DamageModifier.Normal;
            modifiers.m_spirit = HitData.DamageModifier.Immune;

            defaultModifiers = modifiers;
            defaultWalkSpeed = 2;
            defaultRunSpeed = 8;
        }

        public override Boss AddBossComponent(GameObject gameObject)
        {
            return gameObject.AddComponent<Queen>();
        }
    }
}
