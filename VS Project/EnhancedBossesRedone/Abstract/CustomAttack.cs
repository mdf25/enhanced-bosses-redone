using BepInEx.Bootstrap;
using EnhancedBossesRedone.Data;
using System;
using UnityEngine;

namespace EnhancedBossesRedone.Abstract
{
    public abstract class CustomAttack
    {
        public string? bossName;
        public string? name;
        public string? baseName;
        public bool stopOriginalAttack;
        public float heal;
        public float speedModifier;
        public GameObject? attackPrefab;
        public Attack? attack;
        public ItemDrop? itemDrop;

        public virtual void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
        }

        public virtual void AdjustAttackParameters()
        {
        }

        public virtual void AdjustAttackParametersLate()
        {
        }

        public bool GetEnabled()
        {
            try
            {
                return ConfigManager.BossConfigs![bossName!][name!].Enabled;
            }
            catch (Exception e)
            {
                Main.Log!.LogWarning("No JSON entry found for " + name + ". Disabling.");
                Main.Log!.LogWarning(e.Message);
                return false;
            }
        }

        public int GetStars()
        {
            try
            {
                if (!Chainloader.PluginInfos.ContainsKey("org.bepinex.plugins.creaturelevelcontrol"))
                {
                    return 0;
                }
                return ConfigManager.BossConfigs![bossName!][name!].Stars;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public float GetHeal()
        {
            return ConfigManager.BossConfigs![bossName!][name!].Heal;
        }

        public float GetHpThreshold()
        {
            try
            {
                return ConfigManager.BossConfigs![bossName!][name!].HpThreshold == 0f ? 1f : ConfigManager.BossConfigs![bossName!][name!].HpThreshold;
            }
            catch (Exception)
            {
                return 1f;
            }
        }

        public float GetMinDistance()
        {
            int minDistance = 0;
            if (ConfigManager.BossConfigs![bossName!][name!].MinDistance != 0)
            {
                minDistance = Mathf.Clamp(ConfigManager.BossConfigs![bossName!][name!].MinDistance, 0, 1000);
            }
            return minDistance;
        }

        public float GetMaxDistance()
        {
            int maxDistance = 1000;
            if (ConfigManager.BossConfigs![bossName!][name!].MaxDistance != 0)
            {
                maxDistance = Mathf.Clamp(ConfigManager.BossConfigs![bossName!][name!].MaxDistance, 0, 1000);
            }
            return maxDistance;
        }

        public float GetShieldHp()
        {
            return ConfigManager.BossConfigs![bossName!][name!].ShieldHp;
        }

        public float GetShieldDuration()
        {
            return ConfigManager.BossConfigs![bossName!][name!].ShieldDuration;
        }

        public virtual bool CanUseAttack(Character character, MonsterAI monsterAI)
        {
            return character.GetHealthPercentage() <= GetHpThreshold() && character.GetLevel() > GetStars();
        }

        public virtual void Setup(ObjectDB objectDB)
        {
            GameObject prefab = objectDB.GetItemPrefab(baseName);
            attackPrefab = name == baseName ? prefab : prefab.Clone(name!);
            attackPrefab.transform.SetParent(Main.Holder!.transform, false);

            itemDrop = attackPrefab.GetComponent<ItemDrop>();
            attack = itemDrop.m_itemData.m_shared.m_attack;

            AdjustAttackParameters();
            if (name == baseName)
            {
                return;
            }

            itemDrop.m_itemData.m_shared.m_name = name;
            itemDrop.m_itemData.m_shared.m_setName = name;
        }

        public virtual void SetupLate()
        {
            AdjustAttackParametersLate();
        }

        public virtual void AddToObjectDB(ObjectDB objectDB)
        {
            ItemDrop component = attackPrefab!.GetComponent<ItemDrop>();
            component = itemDrop!;

            if (attackPrefab != null)
            {
                objectDB.AddCustomAttack(attackPrefab);
            }
        }
    }
}
