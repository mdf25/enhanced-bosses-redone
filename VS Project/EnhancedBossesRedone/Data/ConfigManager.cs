using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using ServerSync;
using System;
using System.Collections.Generic;

namespace EnhancedBossesRedone.Data
{
    internal class ConfigManager
    {
        public static bool CreateConfigValues(BaseUnityPlugin plugin)
        {
            configSync = new ConfigSync(Main.PluginGUID)
            {
                DisplayName = "Enhanced Bosses Redone",
                CurrentVersion = Main.PluginVersion,
                MinimumRequiredVersion = Main.PluginVersion,
                ModRequired = true,
            };

            plugin.Config.SaveOnConfigSet = true;
            // GENERAL
            ModEnabled = plugin.Config.BindServer<bool>("1. General", "Enabled Mod", true, new ConfigDescription("Whether this mod is enabled or not."));
            JsonSettings = plugin.Config.BindServer<string>("1. General", "Boss Config JSON", defaultJsonFile, new ConfigDescription("Name of the Enhanced Bosses JSON configuration file.\nIf you change this, COPY this file into the same directory as the DLL file and rename the copied JSON file name to match what you put in the config. This will avoid new versions overwriting the JSON config when updating the mod and will just replace the original one, so you can check for new updates manually.\nOnly change this if you know what you're doing."));
            RawJson = plugin.Config.BindServer<string>("5. Raw Data", "Boss JSON Raw", "", new ConfigDescription("Holds the boss JSON settings for ServerSync. This loads your JSON file here for reading and parsing.\nDo not adjust this value, as it will be overwritten on next game load. To make changes to boss attack behaviour, adjust the JSON in the file directly to make changes and it will update here."));

            bool JsonLoaded = LoadJsonConfig(plugin, JsonSettings.Value);
            if (!JsonLoaded)
            {
                Main.Log!.LogInfo("Attempting to load the default JSON config.");
                JsonLoaded = LoadJsonConfig(plugin, defaultJsonFile);
            }
            if (!JsonLoaded)
            {
                Main.Log!.LogError("Failed to load necessary configuration files.");
                return false;
            }

            // EIKTHYR
            EikthyrDefaultSpeed = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Default Speed", 1.1f, new ConfigDescription("Eikthyr's default movement speed setting. Change this to make him run faster or slower.", new AcceptableValueRange<float>(0.5f, 2.0f)));
            EikthyrMaxSpeed = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Max Speed", 1.7f, new ConfigDescription("At low HP, Eikthyr will reach this speed when moving.", new AcceptableValueRange<float>(0.5f, 2.0f)));
            EikthyrTeleport = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Teleport", false, new ConfigDescription("Whether Eikthyr can teleport behind you when using his frontal electric attack."));
            EikthyrCloneMax = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Clones Maximum", 3, new ConfigDescription("How many clones Eikthyr can spawn in. Can disable clonses in the JSON config.", new AcceptableValueRange<int>(1, 5)));
            EikthyrStormClouds = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Storm Clouds", 10, new ConfigDescription("How many storm clouds will be called when Eikthyr summons storms.", new AcceptableValueRange<int>(1, 10)));
            EikthyrStormLightningDamage = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Storm Lightning Damage", 25.0f, new ConfigDescription("Damage done by lightning strikes from storm clouds.", new AcceptableValueRange<float>(5.0f, 50.0f)));
            EikthyrVortexStagger = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Vortex Stagger", true, new ConfigDescription("Whether being too close to the vortex attack causes you to stagger and have no escape."));
            EikthyrHeldyrHealth = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Heldyr Summon HP", 50f, new ConfigDescription("Heldyr maximum HP.", new AcceptableValueRange<float>(5.0f, 100.0f)));
            EikthyrHeldyrDamage = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Heldyr Summon Damage", 25f, new ConfigDescription("Damage done by Heldyr attacks.", new AcceptableValueRange<float>(5.0f, 50.0f)));
            EikthyrHelneckHealth = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Helneck Summon HP", 30f, new ConfigDescription("Helneck maximum HP.", new AcceptableValueRange<float>(10.0f, 100.0f)));
            EikthyrHelneckDamage = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Helneck Summon Damage", 18f, new ConfigDescription("Damage done by Helneck attacks.", new AcceptableValueRange<float>(5.0f, 30.0f)));
            EikthyrHelsvinHealth = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Helsvin Summon HP", 40f, new ConfigDescription("Helsvin maximum HP.", new AcceptableValueRange<float>(10.0f, 100.0f)));
            EikthyrHelsvinDamage = plugin.Config.BindServer("2. Eikthyr", "Eikthyr Helsvin Summon Damage", 22f, new ConfigDescription("Damage done by Helsvin attacks.", new AcceptableValueRange<float>(5.0f, 30.0f)));

            // BONEMASS
            BonemassTripEffect = plugin.Config.Bind("3. Bonemass", "Bonemass Hallucinations", true, new ConfigDescription("Getting near bonemass will cause the screen to blur and your character to be slowed. Disabled if FPSBoost is also loaded."));
            BonemassTripIntensity = plugin.Config.Bind("3. Bonemass", "Bonemass Hallucination Intensity", 4.6f, new ConfigDescription("How intense the hallucinations are.", new AcceptableValueRange<float>(0.1f, 10.0f)));
            BonemassTripSlowdown = plugin.Config.BindServer("3. Bonemass", "Bonemass Hallucination Slowdown", 0.8f, new ConfigDescription("Governs how much the hallucinations will also slow you down. Example: 0.3 will mean you will move at 30% of your usual speed when under a hallucination.", new AcceptableValueRange<float>(0.0f, 1.0f)));
            BonemassAOEDurability = plugin.Config.BindServer("3. Bonemass", "Bonemass Poison Cloud Damages Durability", true, new ConfigDescription("Whether Bonemass AoE poison cloud will also do damage to your equipped armor when walked into."));
            BonemassAOEIntensity = plugin.Config.BindServer("3. Bonemass", "Bonemass Poison Cloud Durability Loss Factor", 0.07f, new ConfigDescription("The percentage of durability loss on equipped armor when stepping into a poison cloud.", new AcceptableValueRange<float>(0.0f, 1.0f)));
            BonemassAncientSlimeLifetime = plugin.Config.BindServer("3. Bonemass", "Bonemass Ancient Oozer Lifetime", 60.0f, new ConfigDescription("The time that Ancient Oozers will stay alive for before disappearing and healing Bonemass.", new AcceptableValueRange<float>(30.0f, 120.0f)));
            if (Chainloader.PluginInfos.ContainsKey("castix_ValheimFPSBoost"))
            {
                BonemassTripEffect.Value = false;
            }

            // MODER
            ModerHealthThreshold = plugin.Config.BindServer("4. Moder", "Moder HP threshold", 0.75f, new ConfigDescription("Moder will change takeoff and landing AI after her HP reaches this threshold.", new AcceptableValueRange<float>(0.0f, 1.0f)));
            ModerLandChanceBefore = plugin.Config.BindServer("4. Moder", "Moder Land Chance Before Threshold", 0.6f, new ConfigDescription("How likely Moder will land before her HP reaches the threshold.", new AcceptableValueRange<float>(0.0f, 1.0f)));
            ModerLandChanceAfter = plugin.Config.BindServer("4. Moder", "Moder Land Chance After Threshold", 0.8f, new ConfigDescription("How likely Moder will land after her HP reaches the threshold.", new AcceptableValueRange<float>(0.0f, 1.0f)));
            ModerTakeOffChanceBefore = plugin.Config.BindServer("4. Moder", "Moder Takeoff Chance Before Threshold", 1.0f, new ConfigDescription("How likely Moder will resume flight before her HP reaches the threshold.", new AcceptableValueRange<float>(0.0f, 1.0f)));
            ModerTakeOffChanceAfter = plugin.Config.BindServer("4. Moder", "Moder Takeoff Chance After Threshold", 0.4f, new ConfigDescription("How likely Moder will resume flight after her HP reaches the threshold.", new AcceptableValueRange<float>(0.0f, 1.0f)));
            ModerAncientAwakeningDuration = plugin.Config.BindServer("4. Moder", "Moder Ancient Awakening Duration", 120.0f, new ConfigDescription("How long Moder stays in Ancient Awakened form. 0 will make her always retain this form once activated.", new AcceptableValueRange<float>(0.0f, 300.0f)));
            ModerSummonDieAfter = plugin.Config.BindServer("4. Moder", "Moder Summons Lifetime", 60.0f, new ConfigDescription("Determines how long Moder's summons live for. Will prevent impossible boss fights for the times your bow breaks. Set to 0 to make summons never die.", new AcceptableValueRange<float>(0.0f, 120.0f)));
            ModerHatchlingHealth = plugin.Config.BindServer("4. Moder", "Moder Summon Drake HP", 50.0f, new ConfigDescription("Health of the Drakes summoned by Moder. Note that this will ONLY work if summon is set to eb_hatchling in the JSON config.", new AcceptableValueRange<float>(25.0f, 200.0f)));
            ModerWyvernHealth = plugin.Config.BindServer("4. Moder", "Moder Summon Wyvern HP", 150.0f, new ConfigDescription("Health of the Wyverns summoned by Moder. Note that this will ONLY work if summon is set to eb_wyvern in the JSON config.", new AcceptableValueRange<float>(50.0f, 300.0f)));
            ModerWyvernProjectilesLaunched = plugin.Config.BindServer("4. Moder", "Wyvern Projectile Amount", 5, new ConfigDescription("Number of projectiles launched by the Wyvern when attacking.", new AcceptableValueRange<int>(1, 10)));
            ModerWyvernProjectileFrostDamage = plugin.Config.BindServer("4. Moder", "Wyvern Projectile Frost Damage", 45.0f, new ConfigDescription("Frost damage dealt by Wyvern projectile.", new AcceptableValueRange<float>(5.0f, 80.0f)));
            ModerWyvernProjectileBluntDamage = plugin.Config.BindServer("4. Moder", "Wyvern Projectile Blunt Damage", 25.0f, new ConfigDescription("Blunt damage dealt by Wyvern projectile.", new AcceptableValueRange<float>(5.0f, 80.0f)));

            // Deserialize other config values and exit out.
            return ParseJson();
        }

        private static bool LoadJsonConfig(BaseUnityPlugin plugin, string fileName)
        {
            try
            {
                string JSON = Helpers.ReadJsonToText(fileName);
                RawJson!.Value = JSON;
                return true;
            }
            catch (Exception e)
            {
                if (fileName == defaultJsonFile)
                {
                    Main.Log!.LogError("The default JSON file '" + defaultJsonFile + "' is missing.");
                }
                else
                {
                    Main.Log!.LogWarning(e.Message);
                }
                return false;
            }
        }

        public static bool ParseJson()
        {
            try
            {
                BossConfigs = Helpers.DeserializeJson(RawJson!.Value);
                return true;
            }
            catch (Exception e)
            {
                Main.Log!.LogError(e.Message);
                return false;
            }
        }


        public static readonly string defaultJsonFile = "eb_settings.json";

        public static ConfigSync? configSync;

        public static Dictionary<string, Dictionary<string, Main.ItemInfo>>? BossConfigs;
        public static ConfigEntry<bool>? ModEnabled;
        public static ConfigEntry<string>? JsonSettings;
        public static ConfigEntry<string>? RawJson;
        public static ConfigEntry<float>? EikthyrDefaultSpeed;
        public static ConfigEntry<float>? EikthyrMaxSpeed;
        public static ConfigEntry<bool>? EikthyrTeleport;
        public static ConfigEntry<int>? EikthyrCloneMax;
        public static ConfigEntry<int>? EikthyrStormClouds;
        public static ConfigEntry<float>? EikthyrStormLightningDamage;
        public static ConfigEntry<bool>? EikthyrVortexStagger;
        public static ConfigEntry<float>? EikthyrHeldyrHealth;
        public static ConfigEntry<float>? EikthyrHeldyrDamage;
        public static ConfigEntry<float>? EikthyrHelneckHealth;
        public static ConfigEntry<float>? EikthyrHelneckDamage;
        public static ConfigEntry<float>? EikthyrHelsvinHealth;
        public static ConfigEntry<float>? EikthyrHelsvinDamage;
        public static ConfigEntry<bool>? BonemassTripEffect;
        public static ConfigEntry<float>? BonemassTripIntensity;
        public static ConfigEntry<float>? BonemassTripSlowdown;
        public static ConfigEntry<bool>? BonemassAOEDurability;
        public static ConfigEntry<float>? BonemassAOEIntensity;
        public static ConfigEntry<float>? BonemassAncientSlimeLifetime;
        public static ConfigEntry<float>? ModerHealthThreshold;
        public static ConfigEntry<float>? ModerLandChanceBefore;
        public static ConfigEntry<float>? ModerLandChanceAfter;
        public static ConfigEntry<float>? ModerTakeOffChanceBefore;
        public static ConfigEntry<float>? ModerTakeOffChanceAfter;
        public static ConfigEntry<float>? ModerAncientAwakeningDuration;
        public static ConfigEntry<float>? ModerSummonDieAfter;
        public static ConfigEntry<float>? ModerHatchlingHealth;
        public static ConfigEntry<float>? ModerWyvernHealth;
        public static ConfigEntry<int>? ModerWyvernProjectilesLaunched;
        public static ConfigEntry<float>? ModerWyvernProjectileFrostDamage;
        public static ConfigEntry<float>? ModerWyvernProjectileBluntDamage;
    }
}
