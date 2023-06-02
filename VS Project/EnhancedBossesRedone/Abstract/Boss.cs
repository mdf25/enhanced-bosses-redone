using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using EnhancedBossesRedone.Data;

namespace EnhancedBossesRedone.Abstract
{
    public abstract class Boss : MonoBehaviour
    {
        public Boss()
        {
            string nsPath = "EnhancedBossesRedone.BossAttacks." + GetType().Name + "Attacks";
            List<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(t => (t.IsClass && t.ToString().StartsWith(nsPath) && !t.ToString().Contains("+<"))).ToList();
            customAttacks = new List<CustomAttack>();

            foreach (Type type in types)
            {
                CustomAttack? attack = Activator.CreateInstance(type) as CustomAttack;
                if (attack != null && attack.GetEnabled())
                {
                    customAttacks.Add(attack);
                }
            }
        }

        public virtual void Awake()
        {
            bossPrefab = gameObject;

            // Check for clone spawns and ignore extra components.
            if (bossName + "(Clone)" != name)
            {
                return;
            }

            pin = PinManager.AddBossPin(position);
            Main.pinsList.Add(this);

            attackStats = new Dictionary<string, CustomAttackData>();
            for (int i = 0; i < humanoid!.m_defaultItems.Length; i += 1)
            {
                ItemDrop component = humanoid.m_defaultItems[i].GetComponent<ItemDrop>();
                Main.ItemInfo itemInfo = ConfigManager.BossConfigs![bossName!][component.name];
                CustomAttackData data = new CustomAttackData();
                data.CooldownAdjust = itemInfo.AttackCoolDownMultiplier;
                data.DefaultCooldown = itemInfo.Cooldown;
                data.HPThreshold = (float)itemInfo.HpThreshold > 0 ? (float)itemInfo.HpThreshold : 1;
                attackStats.Add(component.name, data);
            }

            PopulateDefaultValues();

            InvokeRepeating("AdjustCooldowns", 5.0f, 5.0f);
        }

        public void AdjustCooldowns()
        {
            if (!zNetView!.IsValid() || !zNetView!.IsOwner())
            {
                return;
            }

            float currentHealth = Mathf.Round(character!.GetHealthPercentage() * 20f) / 20f;
            if (currentHealth == lastHealthValue)
            {
                return;
            }

            for (var i = 0; i < humanoid!.m_inventory.m_inventory.Count; i += 1)
            {
                ItemDrop.ItemData itemData = humanoid.m_inventory.m_inventory[i];

                string name = itemData.m_dropPrefab != null ? itemData.m_dropPrefab.name : itemData.m_shared.m_name;
                if (name == null)
                {
                    continue;
                }

                CustomAttackData data;
                if (!attackStats!.TryGetValue(name, out data))
                {
                    continue;
                }

                itemData.m_shared.m_aiAttackInterval = CalculateCooldownAdjustment(currentHealth, data);
            }
            humanoid.m_inventory.Changed();
            return;
        }

        public float CalculateCooldownAdjustment(float characterHP, CustomAttackData data)
        {
            if (data.CooldownAdjust == 0 || data.CooldownAdjust == 1)
            {
                return data.DefaultCooldown;
            }

            return data.DefaultCooldown * Mathf.Lerp(data.CooldownAdjust, 1, Mathf.Min(characterHP / data.HPThreshold, 1));
        }

        public virtual void PopulateDefaultValues()
        {

        }

        public virtual void DropItemsOnDespawn()
        {
            CharacterDrop characterDrop = gameObject.GetComponent<CharacterDrop>();
            characterDrop.m_dropsEnabled = false;
        }

        public void OnDeath()
        {
            if (pin != null)
            {
                Minimap.instance.RemovePin(pin);
            }
        }

        public void UpdatePosition()
        {
            position = character!.transform.position;
            if (pin != null)
            {
                pin.m_pos = position;
            }
        }

        public virtual bool Process_Attack(Attack attack)
        {
            ItemDrop.ItemData weapon = attack.m_weapon;
            foreach (CustomAttack customAttack in customAttacks!)
            {
                bool flag = weapon.m_dropPrefab != null ? weapon.m_dropPrefab.name == customAttack.name : weapon.m_shared.m_name == customAttack.name;
                if (!flag)
                {
                    continue;
                }
                
                customAttack.OnAttackTriggered(character!, monsterAI!);
                bool stopOriginalAttack = customAttack.stopOriginalAttack;
                if (stopOriginalAttack)
                {
                    return false;
                }
                return true;
            }
            return true;
        }

        public virtual bool CanUseAttack(ItemDrop.ItemData item)
        {
            foreach (CustomAttack customAttack in customAttacks!)
            {
                bool flag = item.m_dropPrefab != null ? item.m_dropPrefab.name == customAttack.name : item.m_shared.m_name == customAttack.name;
                if (flag)
                {
                    return customAttack.CanUseAttack(character!, monsterAI!);
                }
            }
            return true;
        }

        public virtual void SetupCharacter(ZNetScene zNetScene)
        {
            bossPrefab = GetPrefab(zNetScene).Clone(bossName!);
            bossPrefab.transform.SetParent(Main.Holder!.transform, false);

            Boss boss = AddBossComponent(bossPrefab);
            boss.character = bossPrefab.GetComponent<Character>();
            boss.humanoid = bossPrefab.GetComponent<Humanoid>();
            boss.monsterAI = bossPrefab.GetComponent<MonsterAI>();
            boss.baseAI = bossPrefab.GetComponent<BaseAI>();
            boss.zNetView = bossPrefab.GetComponent<ZNetView>();
        }

        public virtual Boss AddBossComponent(GameObject gameObject)
        {
            return gameObject.AddComponent<Boss>();
        }

        public void AddBossPrefab(ZNetScene zNetScene)
        {
            zNetScene.AddCustomPrefab(bossPrefab!);
        }

        public GameObject GetPrefab(ZNetScene zNetScene)
        {
            return zNetScene.GetPrefab(bossName!);
        }

        public void SetupCustomAttacks(ObjectDB objectDB)
        {
            foreach (CustomAttack customAttack in customAttacks!)
            {
                Main.ItemInfo itemInfo = ConfigManager.BossConfigs![bossName!][customAttack.name!];
                customAttack.Setup(objectDB);

                GameObject prefab = customAttack.attackPrefab!;
                ItemDrop component = prefab.GetComponent<ItemDrop>();
                component.m_itemData.m_shared.m_aiAttackInterval = itemInfo.Cooldown;
                component.m_itemData.m_shared.m_aiMaxHealthPercentage = customAttack.GetHpThreshold();
            }
        }

        public void SetupLateCustomAttacks()
        {
            foreach (CustomAttack customAttack in customAttacks!)
            {
                if (customAttack.attackPrefab == null)
                {
                    continue;
                }

                customAttack.SetupLate();
            }
        }

        public void AddCustomAttacksToPrefab()
        {
            List<GameObject> prefabs = new List<GameObject>();
            foreach (CustomAttack customAttack in customAttacks!)
            {
                prefabs.Add(customAttack.attackPrefab!);
            }

            bossPrefab!.GetComponent<Humanoid>().m_defaultItems = prefabs.ToArray();
        }

        public void AddToObjectDB(ObjectDB objectDB)
        {
            foreach (CustomAttack customAttack in customAttacks!)
            {
                if (customAttack.attackPrefab != null)
                {
                    customAttack.AddToObjectDB(objectDB);
                }
            }
        }

        public string? bossName;

        public Character? character;
        public Humanoid? humanoid;
        public MonsterAI? monsterAI;
        public BaseAI? baseAI;
        public ZNetView? zNetView;
        public Vector3 position;
        public Minimap.PinData? pin;
        public GameObject? bossPrefab;

        protected HitData.DamageModifiers defaultModifiers;
        protected float defaultWalkSpeed;
        protected float defaultRunSpeed;

        public float lastHealthValue;

        public List<CustomAttack>? customAttacks;
        public Dictionary<string, CustomAttackData>? attackStats;
    }
}
