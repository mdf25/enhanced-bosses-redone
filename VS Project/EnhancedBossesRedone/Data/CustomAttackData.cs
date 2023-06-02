namespace EnhancedBossesRedone.Data
{
    public class CustomAttackData
    {
        public float HPThreshold;
        public float DefaultCooldown;
        public float CooldownAdjust;

        public override string ToString()
        {
            return "[Threshold:" + HPThreshold.ToString() + ", Default:" + DefaultCooldown.ToString() + ", Adjust:" + CooldownAdjust + "]";
        }
    }
}
