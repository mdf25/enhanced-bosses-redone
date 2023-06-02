using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedBossesRedone.AttachmentScripts
{
    public class CloudScript : MonoBehaviour
    {
        /**
         * When instantiating we need to set up the particle system and start some coroutines.
         */
        public void Awake()
        {
            particle = gameObject.GetComponent<ParticleSystem>();
            nview = gameObject.GetComponent<ZNetView>();
            td = gameObject.GetComponent<TimedDestruction>();
            canMove = true;

            if (!nview.IsValid() || !nview.IsOwner())
            {
                return;
            }

            eikthyr = TryGetEikthyr();
            moveSpeed = Random.Range(2f, 3f);
            vertSpeed = Random.Range(1f, 2f);
            root = gameObject.transform.position + Helpers.InsideAnnulusXZ(3f, 6f);
            nextStrikeTime = GetNextStrikeTime();
        }

        /**
         * Handle moving the cloud around during each update frame.
         */
        public void Update()
        {
            if (!nview!.IsValid() || !nview!.IsOwner())
            {
                return;
            }

            if (eikthyr == null)
            {
                StopCloudAndDestroy();
                return;
            }

            totalTime += Time.deltaTime;
            time += Time.deltaTime;
            timeSinceStrike += Time.deltaTime;
            
            if (totalTime > stopTime)
            {
                StopCloudAndDestroy();
                return;
            }

            if (time > moveTime)
            {
                GetNextMoveArea();
                time -= moveTime;
            }

            StrikeIfReady();
            MoveCloud();
        }


        public void StrikeIfReady()
        {
            if (timeSinceStrike <= nextStrikeTime)
            {
                return;
            }

            canMove = false;
            if (timeSinceStrike > nextStrikeTime + 1 && !struckRecently)
            {
                Strike();
                return;
            }

            if (timeSinceStrike > nextStrikeTime + 2)
            {
                timeSinceStrike = nextStrikeTime - 2;
                nextStrikeTime = GetNextStrikeTime();
                struckRecently = false;
                canMove = true;
            }
        }

        public void MoveCloud()
        {
            if (!canMove || nextMove == null || heightAdjust == null)
            {
                return;
            }
            gameObject.transform.position += (moveSpeed * nextMove + vertSpeed * heightAdjust) * Time.deltaTime;
        }

        public void StopCloudAndDestroy()
        {
            if (particle!.isPlaying)
            {
                particle!.Stop();
            }

            if (!particle!.IsAlive())
            {
                td!.DestroyNow();
            }
        }

        public float GetNextStrikeTime()
        {
            return UnityEngine.Random.Range(3f, 5f);
        }

        /**
         * Returns a movement vector that is 90 degrees around the root point.
         */
        public Vector3 CalcMoveDirection()
        {
            return Vector3.Cross(root - gameObject.transform.position, Vector3.up).normalized;
        }

        /**
         * Returns the position of the ground directly below the cloud.
         */
        public Vector3 GetGroundPos()
        {
           return gameObject.transform.position.ConvertToWorldHeight();
        }

        /**
         * Gets the distance from cloud to ground.
         */
        public float GetHeightDistance()
        {
            return Mathf.Abs(gameObject.transform.position.y - GetGroundPos().y);
        }

        /**
         * Find Eikthyr to apply damage sources.
         */
        public Eikthyr? TryGetEikthyr()
        {
            if (eikthyr != null)
            {
                return eikthyr;
            }

            List<Character> characters = Character.GetAllCharacters();
            foreach (Character character in characters)
            {
                Eikthyr boss;
                if ((boss = character.GetComponent<Eikthyr>()) != null)
                {
                    return boss;
                }
            }
            return null;
        }


        /**
         * Calculates the movement vector for the cloud once per second, rather than every frame in Update
         */
        private void GetNextMoveArea()
        {
            nextMove = CalcMoveDirection();
            heightAdjust = GetHeightDistance() < 10f ? Vector3.up : Vector3.down;
        }

        /**
         * Generate the lightning strike and controls cloud movement.
         */
        private void Strike()
        {
            struckRecently = true;
            GameObject lightning = Instantiate(ZNetScene.instance.GetPrefab("fx_eikthyr_forwardshockwave"), gameObject.transform.position, Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
            lightning.transform.localScale = new Vector3(GetHeightDistance(), 0.1f, 0.1f);
            Vector3 groundPos = GetGroundPos();

            Instantiate(ZNetScene.instance.GetPrefab("vfx_gdking_stomp"), groundPos, Quaternion.identity);
            Instantiate(ZNetScene.instance.GetPrefab("sfx_gdking_rock_destroyed"), groundPos, Quaternion.identity);

            Collider[] colliders = Physics.OverlapSphere(GetGroundPos(), 6.0f);
            foreach (Collider collider in colliders)
            {
                if (collider == null)
                {
                    continue;
                }

                Player player;
                if ((player = collider.gameObject.GetComponent<Player>()) == null)
                {
                    continue;
                }

                float lightningDamage = ConfigManager.EikthyrStormLightningDamage!.Value;

                HitData hitData = new HitData();
                hitData.m_damage.m_lightning = Random.Range(0.8f, 1.2f) * lightningDamage;
                hitData.SetAttacker(eikthyr != null ? eikthyr.character : null);

                player.Damage(hitData);
            }
        }

        private Eikthyr? eikthyr;
        private ZNetView? nview;
        private TimedDestruction? td;
        public ParticleSystem? particle;

        public bool canMove;
        public float moveSpeed;
        public float vertSpeed;
        
        public Vector3 root;
        public Vector3 nextMove;
        public Vector3 heightAdjust;

        private float time;
        private float totalTime;
        private float timeSinceStrike;
        private float nextStrikeTime;
        private float moveTime = 1;
        private float stopTime = 30;

        private bool struckRecently;
    }
}
