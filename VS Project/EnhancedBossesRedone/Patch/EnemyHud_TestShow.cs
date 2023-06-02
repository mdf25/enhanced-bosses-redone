using HarmonyLib;

namespace EnhancedBossesRedone.Patch
{
    internal class EnemyHud_TestShow
    {
        [HarmonyPatch(typeof(EnemyHud), "TestShow")]
        public static class EnemyHud_TestShow_Postfix
        {
            public static void Postfix(ref Character c, ref bool __result)
            {
                bool flag = c.name.Contains("RRRM_EikthyrClone");
                if (flag)
                {
                    __result = false;
                }
            }
        }
    }
}
