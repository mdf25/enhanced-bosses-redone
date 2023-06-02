using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.StatusEffects
{
    internal class SE_Slow : SE_Stats
    {
        public SE_Slow()
        {
            name = "EB_Slow";
            m_ttl = m_baseTTL;
        }

        public override void ModifySpeed(float baseSpeed, ref float speed)
        {
            speed *= speedAmount;
            base.ModifySpeed(baseSpeed, ref speed);
        }

        public static float m_baseTTL = 1f;
        public float speedAmount = Mathf.Min(Mathf.Max(0.2f, ConfigManager.BonemassTripSlowdown!.Value), 1.0f);
    }
}
