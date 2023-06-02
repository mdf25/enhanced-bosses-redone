using EnhancedBossesRedone.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EnhancedBossesRedone.AttachmentScripts
{
    internal class ThunderProjectile : MonoBehaviour
    {
        public void Awake()
        {
            speed = Random.Range(8f, 12f);
            targetDirection = Vector3.zero;
            nview = gameObject.GetComponentInChildren<ZNetView>();
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

            totalTime += Time.deltaTime;
            collideTime += Time.deltaTime;
            directionTime += Time.deltaTime;

            if (directionTime > directionChangeTime)
            {
                ChangeTargetDirection();
                directionTime -= directionChangeTime;
            }

            if (collideTime > collisionCheckTime)
            {
                CheckWorldCollisions();
                collideTime -= collisionCheckTime;
            }

            MoveProjectile();
            DestroyIfNearbyTargets();
        }

        public void MoveProjectile()
        {
            gameObject.transform.position += targetDirection * speed * Time.deltaTime;

            float height = gameObject.transform.position.y - gameObject.transform.position.ConvertToWorldHeight().y;
            if (height < 1.3f)
            {
                gameObject.transform.position += Vector3.up * 0.5f * speed * Time.deltaTime;
            }
            else if (height > 2.0f)
            {
                gameObject.transform.position -= Vector3.up * 0.5f * speed * Time.deltaTime;
            }
        }

        public void DestroyIfNearbyTargets()
        {
            foreach (Character character in GetEnemies())
            {
                if (character.IsBoss())
                {
                    continue;
                }

                float num = Vector3.Distance(character.transform.position, gameObject.transform.position + Vector3.up);
                if (num < 3f)
                {
                    TriggerDestroy();
                }
            }
        }


        public void TriggerDestroy()
        {
            td = GetComponent<TimedDestruction>();
            if (td != null)
            {
                td.DestroyNow();
                return;
            }
        }


        public void OnDestroy()
        {
            GameObject prefab = ZNetScene.instance.GetPrefab("fx_eikthyr_stomp");
            Instantiate(prefab, transform.position, Quaternion.identity);
            if (character == null)
            {
                return;
            }

            HitData hitData = new HitData();
            hitData.m_damage.m_lightning = 20.0f;
            hitData.m_damage.m_chop = 500.0f;
            hitData.m_damage.m_pickaxe = 500.0f;
            hitData.m_dodgeable = true;
            hitData.m_blockable = true;
            hitData.SetAttacker(character);

            foreach (Character character in GetEnemies())
            {
                character.Damage(hitData);
            }

            Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, range);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.name == "viewblocker" || collider.gameObject.name.Contains("eb_thundercloud"))
                {
                    continue;
                }

                Destructible destructible = collider.gameObject.GetComponent<Destructible>();
                if (destructible != null)
                {
                    destructible.Damage(hitData);
                }
            }

        }

        public void ChangeTargetDirection()
        {
            targetDirection = Quaternion.Euler(0, Random.Range(-5, -5), 0) * targetDirection;
        }

        public IEnumerable<Character> GetEnemies()
        {
            List<Character> list = new List<Character>();
            Character.GetCharactersInRange(transform.position, range, list);
            return from ch in list
                   where BaseAI.IsEnemy(character, ch)
                   select ch;
        }

        public void CheckWorldCollisions()
        {
            Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, 0.5f);
            if (colliders == null || colliders.Length == 0)
            {
                return;
            }

            foreach (Collider collider in colliders)
            {
                bool invalidCollider = false;
                foreach (string colliderName in excludedColliders)
                {
                    if (collider.gameObject.name == colliderName || gameObject.name.Contains(colliderName))
                    {
                        invalidCollider = true;
                        break;
                    }
                }

                if (invalidCollider)
                {
                    continue;
                }

                TriggerDestroy();
            }
        }

        public List<string> excludedColliders = new List<string>()
        {
            "Sphere",
            "viewblock",
            "Lod",
            "Lod0",
            "eb_thundercloud",
            "WaterVolume",
        };

        private ZNetView? nview;
        public float range = 5f;
        public Vector3 targetDirection;
        public Character? character;
        public TimedDestruction? td;
        public float speed;

        private float totalTime;
        private float directionTime;
        private float collideTime;
        private float directionChangeTime = 0.5f;
        private float collisionCheckTime = 0.1f;
    }
}
