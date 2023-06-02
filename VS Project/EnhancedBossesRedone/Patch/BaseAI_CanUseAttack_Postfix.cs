using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;
using HarmonyLib;

namespace EnhancedBossesRedone.Patch
{
    internal class BaseAI_CanUseAttack
    {
        [HarmonyPatch(typeof(BaseAI), "CanUseAttack")]
        public static class BaseAI_CanUseAttack_Postfix
        {
            public static void Postfix(BaseAI __instance, ItemDrop.ItemData item, ref bool __result)
            {
                Boss? boss = __instance.GetBoss();
                if (boss != null && __result)
                {
                    __result = boss.CanUseAttack(item);
                }
            }
        }
    }
}
