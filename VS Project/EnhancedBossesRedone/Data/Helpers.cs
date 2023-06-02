using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ServerSync;
using UnityEngine;
using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.Data
{
    public static class Helpers
    {
        public static ConfigEntry<T> BindServer<T>(this ConfigFile config, string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = config.Bind(group, name, value, description);
            SyncedConfigEntry<T> syncedConfigEntry = ConfigManager.configSync!.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;
            return configEntry;
        }


        public static AssetBundle? LoadAssetBundleFromResources(string bundleName, Assembly resourceAssembly)
        {
            if (resourceAssembly == null)
            {
                throw new ArgumentNullException("Parameter resourceAssembly can not be null.");
            }
            string? text = null;
            try
            {
                string[] manifestNames = resourceAssembly.GetManifestResourceNames();
                text = resourceAssembly.GetManifestResourceNames().Single((str) => str.EndsWith(bundleName));
            }
            catch (Exception)
            {
            }
            if (text == null)
            {
                Main.Log!.LogFatal("AssetBundle " + bundleName + " not found in assembly manifest");
                return null;
            }
            AssetBundle result;
            using (Stream manifestResourceStream = resourceAssembly.GetManifestResourceStream(text))
            {
                result = AssetBundle.LoadFromStream(manifestResourceStream);
            }
            return result;
        }

        public static AssetBundle? LoadAssetBundleFromResources(string bundleName)
        {
            return LoadAssetBundleFromResources(bundleName, Assembly.GetExecutingAssembly());
        }


        public static AssetBundle? LoadAssetBundle(string bundlePath)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string filePath = Path.Combine(Path.GetDirectoryName(path), bundlePath);
            if (!File.Exists(filePath))
            {
                Main.Log!.LogFatal("No asset bundle '" + bundlePath + "' found at path '" + path + "'.");
                return null;
            }

            return AssetBundle.LoadFromFile(filePath);
        }


        public static Assembly? GetCallingAssembly()
        {
            Type reflectedType = (new StackTrace().GetFrames() ?? Array.Empty<StackFrame>()).First(delegate (StackFrame x)
            {
                Type reflectedType2 = x.GetMethod().ReflectedType;
                return (reflectedType2 != null ? reflectedType2.Assembly : null) != typeof(Main).Assembly;
            }).GetMethod().ReflectedType;
            if (reflectedType == null)
            {
                return null;
            }
            return reflectedType.Assembly;
        }

        public static Attack Copy(this Attack source)
        {
            // Iterate the Properties of the destination instance and  
            // populate them from their source counterparts  
            Attack destination = new Attack();
            PropertyInfo[] destinationProperties = destination.GetType().GetProperties();
            foreach (PropertyInfo destinationPi in destinationProperties)
            {
                PropertyInfo sourcePi = source.GetType().GetProperty(destinationPi.Name);
                destinationPi.SetValue(destination, sourcePi.GetValue(source, null), null);
            }
            return destination;
        }

        public static void PrintData(this ItemDrop.ItemData itemData)
        {
            if (itemData.m_lastProjectile != null)
            {
                Main.Log!.LogInfo("Last Projectile: " + itemData.m_lastProjectile.name);
            }
            else
            {
                Main.Log!.LogWarning("No last projectile...");
            }

            if (itemData.m_shared.m_hitEffect != null && itemData.m_shared.m_hitEffect.m_effectPrefabs.Length > 0)
            {
                Main.Log!.LogInfo("Hit Effect prefabs...");
                foreach (EffectList.EffectData effect in itemData.m_shared.m_hitEffect.m_effectPrefabs)
                {
                    if (effect.m_prefab != null)
                    {
                        Main.Log!.LogInfo(effect.m_prefab.name);
                    }
                }
            }
            else
            {
                Main.Log!.LogWarning("No Hit Effects");
            }

            if (itemData.m_shared.m_spawnOnHit != null)
            {
                Main.Log.LogInfo("Spawn on hit: " + itemData.m_shared.m_spawnOnHit.name);
            }
            else
            {
                Main.Log.LogWarning("No spawn on hit.");
            }

            if (itemData.m_shared.m_startEffect != null && itemData.m_shared.m_startEffect.m_effectPrefabs.Length > 0)
            {
                Main.Log!.LogInfo("Start Effect prefabs...");
                foreach (EffectList.EffectData effect in itemData.m_shared.m_startEffect.m_effectPrefabs)
                {
                    if (effect.m_prefab != null)
                    {
                        Main.Log!.LogInfo(effect.m_prefab.name);
                    }
                }
            }
            else
            {
                Main.Log.LogWarning("No start effect.");
            }

            if (itemData.m_shared.m_trailStartEffect != null && itemData.m_shared.m_trailStartEffect.m_effectPrefabs.Length > 0)
            {
                Main.Log!.LogInfo("Trail Start Effect prefabs...");
                foreach (EffectList.EffectData effect in itemData.m_shared.m_trailStartEffect.m_effectPrefabs)
                {
                    if (effect.m_prefab != null)
                    {
                        Main.Log!.LogInfo(effect.m_prefab.name);
                    }
                }
            }
            else
            {
                Main.Log.LogWarning("No trail start effect.");
            }

            if (itemData.m_shared.m_triggerEffect != null && itemData.m_shared.m_triggerEffect.m_effectPrefabs.Length > 0)
            {
                Main.Log!.LogInfo("Trigger Effect prefabs...");
                foreach (EffectList.EffectData effect in itemData.m_shared.m_triggerEffect.m_effectPrefabs)
                {
                    if (effect.m_prefab != null)
                    {
                        Main.Log!.LogInfo(effect.m_prefab.name);
                    }
                }
            }
            else
            {
                Main.Log.LogWarning("No trigger effect.");

            }

            itemData.m_shared.m_attack.PrintData();
        }

        public static void PrintData(this Attack attack)
        {
            if (attack.m_spawnOnTrigger != null)
            {
                Main.Log!.LogInfo("Spawn On trigger: " + attack.m_spawnOnTrigger.name);
            }
            else
            {
                Main.Log!.LogWarning("No spawn on trigger.");
            }

            if (attack.m_attackProjectile != null)
            {
                Main.Log!.LogInfo("Attack Projectile: " + attack.m_attackProjectile.name);
            }
            else
            {
                Main.Log!.LogWarning("No projectile...");
            }

            if (attack.m_burstEffect != null && attack.m_burstEffect.m_effectPrefabs.Length > 0)
            {
                Main.Log.LogInfo("Burst Effect prefabs...");
                foreach (EffectList.EffectData effect in attack.m_burstEffect.m_effectPrefabs)
                {
                    if (effect.m_prefab != null)
                    {
                        Main.Log!.LogInfo(effect.m_prefab.name);
                    }
                }
            }
            else
            {
                Main.Log.LogWarning("No burst effect prefabs...");
            }

            if (attack.m_hitEffect != null && attack.m_hitEffect.m_effectPrefabs.Length > 0)
            {
                Main.Log!.LogInfo("Hit Effect prefabs...");
                foreach (EffectList.EffectData effect in attack.m_hitEffect.m_effectPrefabs)
                {
                    if (effect.m_prefab != null)
                    {
                        Main.Log!.LogInfo(effect.m_prefab.name);
                    }
                }
            }
            else
            {
                Main.Log!.LogWarning("No hit effect prefabs...");
            }

            if (attack.m_startEffect != null && attack.m_startEffect.m_effectPrefabs.Length > 0)
            {
                Main.Log.LogInfo("Start Effect prefabs...");
                foreach (EffectList.EffectData effect in attack.m_startEffect.m_effectPrefabs)
                {
                    if (effect.m_prefab != null)
                    {
                        Main.Log!.LogInfo(effect.m_prefab.name);
                    }
                }
            }
            else
            {
                Main.Log!.LogWarning("No start effect prefabs...");
            }

            if (attack.m_triggerEffect != null && attack.m_triggerEffect.m_effectPrefabs.Length > 0)
            {
                Main.Log.LogInfo("Trigger Effect prefabs...");
                foreach (EffectList.EffectData effect in attack.m_triggerEffect.m_effectPrefabs)
                {
                    if (effect.m_prefab != null)
                    {
                        Main.Log!.LogInfo(effect.m_prefab.name);
                    }
                }
            }
            else
            {
                Main.Log!.LogWarning("No trigger effect prefabs...");
            }
        }

        public static Vector3 GetLookDirXZ(this Character character)
        {
            return character.GetLookDir();
        }

        public static void ApplyTint(this GameObject gameObject, Color color)
        {
            if (gameObject == null)
            {
                return;
            }

            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            if (renderers == null || renderers.Length == 0)
            {
                return;
            }

            for (int i = 0; i < renderers.Length; i += 1)
            {
                Material mat = renderers[i].material;
                if (mat == null)
                {
                    continue;
                }

                mat.color = color;
            }
        }

        public static List<Player> FindPlayers(Vector3 point, float radius)
        {
            List<Player> list = new List<Player>();
            Player.GetPlayersInRange(point, radius, list);
            return list;
        }

        public static List<Character> FindEnemies(this Character character, float range)
        {
            return character.FindEnemies(character.transform.position, range);
        }

        public static List<Character> FindEnemies(this Character character, Vector3 point, float range)
        {
            List<Character> list = new List<Character>();
            List<Character> output = new List<Character>();
            Character.GetCharactersInRange(point, range, list);
            foreach (Character character2 in list)
            {
                if (BaseAI.IsEnemy(character, character2))
                {
                    output.Add(character2);
                }
            }
            return output;
        }

        public static Vector3 AlongUnitSphereXZ()
        {
            Vector2 circle = UnityEngine.Random.insideUnitCircle.normalized;
            return new Vector3(circle.x, 0, circle.y);
        }

        public static Vector3 InsideAnnulusXZ(float min, float max)
        {
            return AlongUnitSphereXZ() * UnityEngine.Random.Range(min, max);
        }

        public static Vector3 ConvertToWorldHeight(this Vector3 vector)
        {
            float height;
            if (!Heightmap.GetHeight(vector, out height))
            {
                return vector;
            }
            return new Vector3(vector.x, height, vector.z);
        }

        public static float GetHeightFromGround(this Vector3 vector)
        {
            float height;
            if (!Heightmap.GetHeight(vector, out height))
            {
                return 0;
            }
            return Mathf.Abs(height - vector.y);
        }

        public static float Lerp(ref float value, float target, float delta)
        {
            if (value != target)
            {
                value = value < target ? Mathf.Min(target, value + delta) : Mathf.Max(target, value - delta);
            }
            return value / target;
        }

        public static string ReadJsonToText(string filename)
        {
            string path = Path.Combine(Main.ModPath, filename);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("No file '" + filename + "' found at location " + Main.ModPath + ".");
            }

            return File.ReadAllText(path);
        }

        public static Dictionary<string, Dictionary<string, Main.ItemInfo>>? DeserializeJson(string JSON)
        {  
            return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Main.ItemInfo>>>(JSON);
        }

        public static Boss? GetBoss(this Character character)
        {
            string text = character.name.Replace("(Clone)", string.Empty);
            switch (text)
            {
                case "Eikthyr":
                    return character.gameObject.GetComponent<Eikthyr>();
                case "gd_king":
                    return character.gameObject.GetComponent<Elder>();
                case "Bonemass":
                    return character.gameObject.GetComponent<Bonemass>();
                case "Dragon":
                    return character.gameObject.GetComponent<Moder>();
                case "GoblinKing":
                    return character.gameObject.GetComponent<Yagluth>();
                case "SeekerQueen":
                    return character.gameObject.GetComponent<Queen>();
                default:
                    return null;
            }
        }

        public static Boss? GetBoss(this BaseAI baseAI)
        {
            return baseAI.m_character.GetBoss();
        }

        public static GameObject Clone(this GameObject prefab, string name)
        {
            GameObject newPrefab = UnityEngine.Object.Instantiate(prefab, Main.Holder!.transform, false);
            newPrefab.name = name;
            newPrefab.transform.SetParent(Main.Holder.transform, false);
            return newPrefab;
        }

        public static void AddCustomPrefab(this ZNetScene zNetScene, GameObject prefab, bool overwrite = true)
        {
            GameObject gameObject;
            if ((gameObject = zNetScene.GetPrefab(prefab.name)) != null)
            {
                if (!overwrite)
                {
                    Main.Log!.LogWarning("Trying to set prefab " + gameObject.name + " but one exists.");
                    return;
                }
                zNetScene.m_prefabs.Remove(gameObject);
                zNetScene.m_namedPrefabs.Remove(prefab.name.GetStableHashCode());
            }
            zNetScene.m_prefabs.Add(prefab);
            zNetScene.m_namedPrefabs.Add(prefab.name.GetStableHashCode(), prefab);
        }

        public static void AddCustomAttack(this ObjectDB objectDB, GameObject attack, bool overwrite = true)
        {
            GameObject prefab;
            if ((prefab = objectDB.GetItemPrefab(attack.name)) != null)
            {
                if (!overwrite)
                {
                    Main.Log!.LogWarning("Trying to set prefab " + prefab.name + " but one exists.");
                    return;
                }
                objectDB.m_items.Remove(prefab);
                objectDB.m_itemByHash.Remove(attack.name.GetStableHashCode());
            }
            objectDB.m_items.Add(attack);
            objectDB.m_itemByHash.Add(attack.name.GetStableHashCode(), attack);
        }

        public static void AddStatusEffect(this ObjectDB objectDB, StatusEffect effect)
        {
            objectDB.m_StatusEffects.Add(effect);
        }


        public static bool HaveShield(this Character character)
        {
            List<StatusEffect> characterEffects = character.GetSEMan().GetStatusEffects();
            foreach (StatusEffect effect in characterEffects)
            {
                if (effect.name.ToLower().Contains("shield"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
