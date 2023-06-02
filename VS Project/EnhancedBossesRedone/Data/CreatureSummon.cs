using System.Collections.Generic;

namespace EnhancedBossesRedone.Data
{
    public class CreatureSummon
    {
        public CreatureSummon(string data)
        {
            string[] entries = new string[3] { "", "0.0", "1.0" };
            if (data == null || data == "")
            {
                Main.Log!.LogFatal("Data for creature summon must not be blank.");
                return;
            }

            if (!data.Contains(":"))
            {
                entries[0] = data.Trim();
            }
            else
            {
                string[] split = data.Trim().Split(':');
                for (int i = 0; i < split!.Length; i += 1)
                {
                    if (split != null)
                    {
                        entries[i] = split[i].Trim();
                    }
                }
            }

            for (int i = 0; i < entries.Length; i += 1)
            {
                switch (i)
                {
                    case 0:
                        CreatureName = entries[i];
                        break;
                    case 1:
                        if (!float.TryParse(entries[i], out MinHpThreshold))
                        {
                            MinHpThreshold = 0.0f;
                            Main.Log!.LogWarning("Could not parse value " + entries[i] + " as float for creature " + CreatureName);
                        }
                        break;
                    case 2:
                        if (!float.TryParse(entries[i], out MaxHpThreshold))
                        {
                            MaxHpThreshold = 1.0f;
                            Main.Log!.LogWarning("Could not parse value " + entries[i] + " as float for creature " + CreatureName);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public string? CreatureName;
        public float MinHpThreshold;
        public float MaxHpThreshold;
    }
}
