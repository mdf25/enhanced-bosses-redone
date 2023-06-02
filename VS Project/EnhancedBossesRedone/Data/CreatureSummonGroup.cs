using System.Collections.Generic;

namespace EnhancedBossesRedone.Data
{
    public class CreatureSummonGroup
    {
        public CreatureSummonGroup()
        {
            summons = new List<CreatureSummon>();
        }

        public List<string> GetSummonsForThreshold(float HpPercentage)
        {
            List<string> result = new List<string>();
            if (summons == null || summons!.Count == 0)
            {
                return result;
            }

            foreach (CreatureSummon creatureSummon in summons)
            {
                if (creatureSummon.CreatureName == null)
                {
                    continue;
                }

                if (creatureSummon.MinHpThreshold <= HpPercentage && HpPercentage <= creatureSummon.MaxHpThreshold)
                {
                    result.Add(creatureSummon.CreatureName);
                }
            }

            return result;
        }


        public List<CreatureSummon> summons;
    }
}
