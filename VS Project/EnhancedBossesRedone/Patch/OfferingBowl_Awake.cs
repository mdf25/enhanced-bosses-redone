using HarmonyLib;

namespace EnhancedBossesRedone.Patch
{
    internal class OfferingBowl_Awake
    {
        [HarmonyPatch(typeof(OfferingBowl), "Awake")]
        public static class OfferingBowl_Awake_Prefix
        {
            public static void Prefix(OfferingBowl __instance)
            {
                __instance.m_bossPrefab = ZNetScene.instance.GetPrefab(__instance.m_bossPrefab.name);
            }
        }
    }
}
