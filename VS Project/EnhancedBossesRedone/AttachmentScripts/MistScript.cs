using UnityEngine;

namespace EnhancedBossesRedone.AttachmentScripts
{
    public class MistScript : MonoBehaviour
    {
        /**
         * When instantiating we need to set up the particle system and start some coroutines.
         */
        public void Awake()
        {
            particle = gameObject.GetComponent<ParticleSystem>();
            nview = gameObject.GetComponent<ZNetView>();
        }

        /**
         * Destroy particle effect after a time.
         */
        public void Update()
        {
            if (!nview!.IsValid() || !nview!.IsOwner())
            {
                return;
            }

            if (totalTime > destroyTime)
            {
                particle!.Stop();
            }

            totalTime += Time.deltaTime;
        }

        private ParticleSystem? particle;
        private ZNetView? nview;

        private float totalTime;
        private float destroyTime = 30;
    }
}
