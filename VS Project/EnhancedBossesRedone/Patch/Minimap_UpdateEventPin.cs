using EnhancedBossesRedone.Data;
using HarmonyLib;

namespace EnhancedBossesRedone.Patch
{
    internal class Minimap_UpdateEventPin
    {
        [HarmonyPatch(typeof(Minimap), "UpdateEventPin")]
        public static class UpdateEventMobMinimapPinsPatch
        {
            public static void Postfix()
            {
                PinManager.CheckBossPins();
            }
        }
    }
}
