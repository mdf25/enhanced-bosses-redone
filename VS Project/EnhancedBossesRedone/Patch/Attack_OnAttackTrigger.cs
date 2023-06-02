using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;
using HarmonyLib;

namespace EnhancedBossesRedone.Patch
{
    internal class Attack_OnAttackTrigger
    {
        [HarmonyPatch(typeof(Attack), "OnAttackTrigger")]
        public static class Attack_OnAttackTrigger_Prefix
        {
            public static bool Prefix(Attack __instance)
            {
                Character character = __instance.m_character;
                Boss? boss = character.GetBoss();
                bool flag = boss != null;
                return !flag || boss!.Process_Attack(__instance);
            }
        }
    }
}
