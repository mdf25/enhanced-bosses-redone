using BepInEx.Bootstrap;
using EnhancedBossesRedone.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EnhancedBossesRedone.Abstract
{
    public abstract class SummonAttack : CustomAttack
    {

        public void SetupCreatureGroup()
        {
            if (summonGroup != null)
            {
                return;
            }

            summonGroup = new CreatureSummonGroup();
            List<string> creatures = GetCreatures();
            foreach (string creature in creatures)
            {
                summonGroup.summons.Add(new CreatureSummon(creature));
            }
        }

        public override bool CanUseAttack(Character character, MonsterAI monsterAI)
        {
            return base.CanUseAttack(character, monsterAI) && GetSpawnedCount(character) < CalculateMaxCount();
        }

        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            SetupCreatureGroup();
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }

            int spawnCount = CalculateSpawnCount(character);
            List<Character> spawned = new List<Character>();
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 effectPos;
                Character? characterSpawned;
                SpawnOne(character, monsterAI, out effectPos, out characterSpawned);
                if (characterSpawned != null)
                {
                    spawned.Add(characterSpawned);
                }
            }
            lastSpawned = spawned;
        }

        public virtual void SpawnOne(Character character, MonsterAI monsterAI, out Vector3 effectPos, out Character? spawned)
        {
            Vector3 roughSpawnPos = character.transform.position + Helpers.InsideAnnulusXZ(minRadius, radius);
            Vector3 spawnPointPos = roughSpawnPos.ConvertToWorldHeight();

            effectPos = spawnPointPos;
            GameObject? randomCreature = GetRandomCreature(character);
            if (randomCreature == null)
            {
                spawned = null;
                return;
            }

            GameObject gameObject = Object.Instantiate(randomCreature, spawnPointPos, Quaternion.Euler(0f, Random.Range(0.0f, 360f), 0f));
            gameObject.SetActive(false);
            bool cancelSpawn;
            AssignParams(character, gameObject, out cancelSpawn);
            if (cancelSpawn)
            {
                Object.Destroy(gameObject);
                spawned = null;
                return;
            }

            gameObject.SetActive(true);
            spawned = gameObject.GetComponent<Character>();
            if (monsterAI.m_targetCreature != null)
            {
                spawned.m_baseAI.SetTargetInfo(monsterAI.m_targetCreature.GetZDOID());
            }
            spawned.m_baseAI.SetHuntPlayer(spawnsHuntPlayer);

            bool matchBossLevel;
            int maxLevel = GetSummonMaxStars(out matchBossLevel) + 1;
            int minLevel = character.GetLevel();
            
            if (matchBossLevel)
            {
                spawned.SetLevel(character.GetLevel());
            }

            if (!matchBossLevel && maxLevel > 1)
            {
                spawned.SetLevel(GetSummonLevel(minLevel, maxLevel));
            }

            CharacterDrop drops;
            if ((drops = gameObject.GetComponent<CharacterDrop>()) != null)
            {
                if (disableDrops)
                {
                    drops.m_dropsEnabled = false;
                    drops.m_drops = new List<CharacterDrop.Drop>(); ;
                }
            }

            if (prefabs == null)
            {
                Object.Instantiate(ZNetScene.instance.GetPrefab("vfx_Potion_stamina_medium"), spawnPointPos, Quaternion.identity);
                Object.Instantiate(ZNetScene.instance.GetPrefab("vfx_WishbonePing"), spawnPointPos, Quaternion.identity);
                return;
            }

            foreach (string prefabName in prefabs)
            {
                Object.Instantiate(ZNetScene.instance.GetPrefab(prefabName), spawnPointPos, Quaternion.identity);
            }
        }

        public virtual void AssignParams(Character character, GameObject gameObject, out bool cancelSpawn)
        {
            cancelSpawn = false;
        }

        public List<string> GetCreatures()
        {
            return ConfigManager.BossConfigs![bossName!][name!].Creatures!;
        }

        public int GetMaxMinionsCount()
        {
            return ConfigManager.BossConfigs![bossName!][name!].maxMinionsCount;
        }

        public int GetmaxMinionsCountPerPlayer()
        {
            return ConfigManager.BossConfigs![bossName!][name!].extraMaxMinionsCountPerPlayer;
        }

        public int GetSpawnMinionsCount()
        {
            return ConfigManager.BossConfigs![bossName!][name!].spawnMinionsCount;
        }

        public int GetSpawnMinionsCountPerPlayer()
        {
            return ConfigManager.BossConfigs![bossName!][name!].extraSpawnMinionsCountPerPlayer;
        }

        public int GetSummonMaxStars(out bool matchBossLevel)
        {
            int defaultMaxLevel = Chainloader.PluginInfos.ContainsKey("org.bepinex.plugins.creaturelevelcontrol") ? 5 : 2;
            matchBossLevel = false;

            try
            {
                if (ConfigManager.BossConfigs![bossName!][name!].SummonMaxStars == -1)
                {
                    matchBossLevel = true;
                    return defaultMaxLevel;
                }
                return Mathf.Clamp(ConfigManager.BossConfigs![bossName!][name!].SummonMaxStars, 0, defaultMaxLevel);
            }
            catch
            {
                return defaultMaxLevel;
            }
        }

        public GameObject? GetRandomCreature(Character character)
        {
            if (summonGroup == null)
            {
                Main.Log!.LogError("Creature group is null.");
            }

            List<string> creatures = summonGroup!.GetSummonsForThreshold(character.GetHealthPercentage() / GetHpThreshold());
            if (creatures.Count == 0)
            {
                Main.Log!.LogWarning("No creatures exist in current HP threshold.");
                return null;
            }

            int index = Random.Range(0, creatures.Count);
            return ZNetScene.instance.GetPrefab(creatures[index]);
        }

        public int CalculateMaxCount()
        {
            int nrOfPlayers = ZNet.instance.GetNrOfPlayers();
            return GetMaxMinionsCount() + (nrOfPlayers - 1) * GetmaxMinionsCountPerPlayer();
        }

        public int CalculateSpawnCount(Character character)
        {
            int nrOfPlayers = ZNet.instance.GetNrOfPlayers();
            int num = GetSpawnMinionsCount() + (nrOfPlayers - 1) * GetSpawnMinionsCountPerPlayer();
            return (int)Mathf.Lerp(0f, num, CalculateMaxCount() - GetSpawnedCount(character));
        }

        public int GetSpawnedCount(Character character)
        {
            int num = 0;
            using (List<BaseAI>.Enumerator enumerator = BaseAI.GetAllInstances().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    BaseAI baseAI = enumerator.Current;
                    if (!GetCreatures().Any((e) => baseAI.name.Contains(e)))
                    {
                        continue;
                    }

                    float num2 = Utils.DistanceXZ(baseAI.transform.position, character.transform.position);
                    if (num2 < searchMinionsRadius)
                    {
                        num += 1;
                    }
                }
            }
            return num;
        }

        private int GetSummonLevel(int minLevel, int maxLevel)
        {
            return (minLevel < maxLevel ? UnityEngine.Random.Range(minLevel, maxLevel) : maxLevel);
        }

        public CreatureSummonGroup? summonGroup;
        public bool spawnsHuntPlayer = true;
        public bool disableDrops = true;
        public float minRadius = 5.0f;
        public float radius = 10.0f;
        public float searchMinionsRadius = 40f;
        public List<string>? prefabs;
        public List<string>? creatures;
        public Vector3 effectPos;
        protected List<Character>? lastSpawned;
    }
}
