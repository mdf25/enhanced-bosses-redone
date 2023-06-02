using UnityEngine;
using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.BossAttacks.BonemassAttacks
{
    public class BonemassAOE : CustomAttack
    {
        public BonemassAOE()
        {
            name = "bonemass_attack_aoe";
            baseName = "bonemass_attack_aoe";
            bossName = "Bonemass";
            stopOriginalAttack = false;
        }

        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (ConfigManager.BonemassAOEDurability!.Value)
            {
                AOEDebuffs(character);
            }
        }

        public void AOEDebuffs(Character character)
        {
            foreach (Player player in Helpers.FindPlayers(character.transform.position, 15f))
            {
                foreach (ItemDrop.ItemData itemData in new ItemDrop.ItemData[]
                {
                    player.m_helmetItem,
                    player.m_chestItem,
                    player.m_legItem,
                    player.m_shoulderItem,
                    player.m_utilityItem
                })
                {
                    if (itemData != null && itemData.m_shared.m_useDurability)
                    {
                        itemData.m_durability = Mathf.Max(0f, itemData.m_durability - Random.Range(0.5f * ConfigManager.BonemassAOEIntensity!.Value, ConfigManager.BonemassAOEIntensity.Value) * itemData.m_durability);
                    }
                }
            }
        }
    }
}
