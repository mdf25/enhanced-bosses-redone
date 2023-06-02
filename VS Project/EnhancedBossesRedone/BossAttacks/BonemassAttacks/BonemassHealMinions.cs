using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;
using System.Collections.Generic;

namespace EnhancedBossesRedone.BossAttacks.BonemassAttacks
{
    public class BonemassHealMinions : SummonAttack
    {
        public BonemassHealMinions()
        {
            name = "bonemass_heal_minions";
            baseName = "bonemass_attack_aoe";
            bossName = "Bonemass";
            stopOriginalAttack = false;
            spawnsHuntPlayer = false;
            prefabs = new List<string>() { "vfx_ghost_death" };
            minRadius = 8f;
            radius = 13f;
        }

        public override bool CanUseAttack(Character character, MonsterAI monsterAI)
        {
            Bonemass bonemass;
            if ((bonemass = (character.GetBoss() as Bonemass)!) == null)
            {
                return false;
            }

            return !bonemass.IsShielded() && base.CanUseAttack(character, monsterAI);
        }

        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_aiAttackRange = 50f;
            itemDrop!.m_itemData.m_shared.m_aiPrioritized = true;
            itemDrop!.m_itemData.m_shared.m_name = name;
        }

        public override void AdjustAttackParametersLate()
        {
            attack!.m_attackProjectile = ZNetScene.instance.GetPrefab("eb_bonemass_aoe_blue");
        }

        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }

            Bonemass? boss = character.GetBoss() as Bonemass;
            if (boss == null)
            {
                return;
            }

            base.OnAttackTriggered(character, monsterAI);
        }
    }
}
