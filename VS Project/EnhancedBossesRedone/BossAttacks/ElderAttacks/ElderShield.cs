using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.StatusEffects;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.ElderAttacks
{
    internal class ElderShield : CustomAttack
    {
        public ElderShield()
        {
            name = "gd_king_shield";
            baseName = "gd_king_rootspawn";
            bossName = "gd_king";
            stopOriginalAttack = true;
        }

        public override bool CanUseAttack(Character character, MonsterAI monsterAI)
        {
            return !character.HaveShield() && base.CanUseAttack(character, monsterAI);
        }

        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            SetupShield(character);
        }

        public void SetupShield(Character character)
        {
            SE_ElderShield shield = ScriptableObject.CreateInstance<SE_ElderShield>();
            shield.m_character = character;
            shield.SetMaxHP(GetShieldHp());
            shield.SetDuration(GetShieldDuration());
            character.GetSEMan().AddStatusEffect(shield, false, 0, 0f);
        }
    }
}
