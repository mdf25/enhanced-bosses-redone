using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.ModerAttacks
{
    public class ModerSummon : SummonAttack
    {
        public ModerSummon()
        {
            name = "dragon_summon";
            baseName = "dragon_spit_shotgun";
            bossName = "Dragon";
            stopOriginalAttack = true;
        }

        public override void AssignParams(Character character, GameObject gameObject, out bool cancelSpawn)
        {
            base.AssignParams(character, gameObject, out cancelSpawn);
            if (ConfigManager.ModerSummonDieAfter!.Value > 0)
            {
                gameObject.AddComponent<ModerSummonScript>();
            }
        }
    }
}
