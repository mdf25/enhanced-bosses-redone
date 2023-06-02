using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.AttachmentScripts
{
    public class ModerSummonScript : MonoBehaviour
    {
        /**
         * Need to get a connection to Bonemass. If none found, slime will just skip and stay still.
         */
        public void Awake()
        {
            nview = gameObject.GetComponent<ZNetView>();
            character = gameObject.GetComponent<Character>();
            deathTime = ConfigManager.ModerSummonDieAfter!.Value;
        }

        /**
         * Kill the creature after a specified amount of time.
         */
        public void Update()
        {
            if (!nview!.IsValid() || !nview!.IsOwner())
            {
                return;
            }

            if (character == null)
            {
                return;
            }

            time += Time.deltaTime;
            if (time > deathTime)
            {
                character.SetHealth(0.0f);
            }
        }

        private Character? character;
        private ZNetView? nview;
        private float deathTime;
        private float time;
    }
}
