using EnhancedBossesRedone.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EnhancedBossesRedone.AttachmentScripts
{
    internal class Vortex : MonoBehaviour
    {
        public void Awake()
        {
            td = GetComponent<ExtendedTimedDestruction>();
            nview = GetComponent<ZNetView>();
            position = transform.position + Vector3.up;
        }

        public void Update()
        {
            if (character == null)
            {
                return;
            }

            if (!nview!.IsValid() || !nview!.IsOwner())
            {
                return;
            }

            foreach (Character character in GetEnemies())
            {
                if (character.IsBoss())
                {
                    continue;
                }

                float num = Vector3.Distance(character.transform.position, position);
                if (num < 2f)
                {
                    td!.StopAllAudioSources();
                    td!.StopAllParticleSystems();
                    Object.Destroy(this);
                    return;
                }


                if (!character.IsStaggering() && ConfigManager.EikthyrVortexStagger!.Value)
                {
                    Vector3 forceDirection = Vector3.Normalize(character.transform.position - position);
                    character.Stagger(forceDirection);
                }
                else
                {
                    Vector3 a = Vector3.Normalize(position - character.transform.position);
                    float d = 1.5f;
                    character.transform.position = character.transform.position + a * d * Time.deltaTime;
                }
            }
        }

        public void OnDestroy()
        {
            GameObject aoeShock = ZNetScene.instance.GetPrefab("fx_eikthyr_stomp");
            GameObject lightning = ZNetScene.instance.GetPrefab("eb_lightning");
            Instantiate(aoeShock, transform.position, Quaternion.identity);
            Instantiate(lightning, transform.position, Quaternion.identity);
            foreach (Character character in GetEnemies())
            {
                HitData hitData = new HitData();
                hitData.m_damage.m_lightning = 20f;
                hitData.SetAttacker(this.character);
                character.Damage(hitData);
            }
        }

        public IEnumerable<Character> GetEnemies()
        {
            List<Character> list = new List<Character>();
            Character.GetCharactersInRange(transform.position, range, list);
            return from ch in list
                   where BaseAI.IsEnemy(character, ch)
                   select ch;
        }

        private ZNetView? nview;
        public float range = 5f;
        public Vector3 position;
        public Character? character;
        public ExtendedTimedDestruction? td;
    }
}
