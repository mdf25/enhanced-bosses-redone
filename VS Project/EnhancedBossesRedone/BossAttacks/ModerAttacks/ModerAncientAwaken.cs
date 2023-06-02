using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Bosses;

namespace EnhancedBossesRedone.BossAttacks.ModerAttacks
{
    public class ModerAncientAwaken : CustomAttack
    {
        public ModerAncientAwaken()
        {
            name = "dragon_awaken";
            baseName = "dragon_taunt";
            bossName = "Dragon";
            stopOriginalAttack = false;
        }

        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_aiPrioritized = true;
            itemDrop!.m_itemData.m_shared.m_aiAttackRangeMin = 0;
        }

        public override bool CanUseAttack(Character character, MonsterAI monsterAI)
        {
            Moder? moder = character.GetBoss() as Moder;
            if (moder == null)
            {
                return false;
            }

            return !moder.IsInAncientState() && base.CanUseAttack(character, monsterAI);
        }

        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            Moder? moder = character.GetBoss() as Moder;
            if (moder == null)
            {
                return;
            }

            moder.StartCoroutine(moder.GenerateLandFireEffect());
            moder.SetAncientState(true);

            if (ConfigManager.ModerAncientAwakeningDuration!.Value > 0)
            {
                moder.StartCoroutine(moder.RemoveAncientStateAfter(ConfigManager.ModerAncientAwakeningDuration!.Value));
            }
        }
    }
}
