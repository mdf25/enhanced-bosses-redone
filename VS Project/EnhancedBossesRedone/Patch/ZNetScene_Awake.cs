using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;
using HarmonyLib;

namespace EnhancedBossesRedone.Patch
{
    internal class ZNetScene_Awake
    {
        [HarmonyPatch(typeof(ZNetScene), "Awake")]
        [HarmonyPriority(Priority.Last)]
        public static class ZNetScene_Awake_Postfix
        {
            public static void Postfix(ZNetScene __instance)
            {
                if (ObjectDB_Awake.ObjectDB_Awake_Postfix.FejdObjectDB == null)
                {
                    Main.Log!.LogError("No fejd");
                    return;
                }

                ConfigManager.ParseJson();
                ObjectDB objectDB = ObjectDB_Awake.ObjectDB_Awake_Postfix.FejdObjectDB.GetComponent<ObjectDB>();

                LoadModdedAssets(__instance);
                
                SetupBosses(__instance, objectDB);
                Main.ReloadPrefabHolder();
            }


            private static void SetupBosses(ZNetScene instance, ObjectDB objectDB)
            {
                foreach (Boss boss in Main.bossList!)
                {
                    boss.SetupCustomAttacks(objectDB);
                    boss.SetupCharacter(instance);
                    boss.SetupLateCustomAttacks();
                    boss.AddToObjectDB(objectDB);
                    boss.AddCustomAttacksToPrefab();
                    boss.AddBossPrefab(instance);
                    Main.Log!.LogMessage("Added " + boss.customAttacks!.Count.ToString() + " custom attacks to " + boss.bossName + ".");
                }
            }

            private static void LoadModdedAssets(ZNetScene instance)
            {
                foreach (ExternalEntity entity in Main.externalEntities!)
                {
                    entity.Setup(instance);
                    entity.AddToPrefabs(instance);
                }
            }
        }
    }
}