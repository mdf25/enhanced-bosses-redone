using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedBossesRedone.AttachmentScripts
{
    public class BonemassRockScript : MonoBehaviour
    {
        /**
         * Setup bonemass rock.
         */
        public void Awake()
        {
            nview = gameObject.GetComponentInChildren<ZNetView>();
            if (!nview.IsValid() || !nview.IsOwner())
            {
                return;
            }

            bonemass = TryGetBonemass();
            if (bonemass == null)
            {
                nview.Destroy();
                return;
            }

            Vector3 startPos = gameObject.transform.position;
            initialPosition = new Vector3(startPos.x, startPos.y, startPos.z);
            groundPosition = initialPosition.ConvertToWorldHeight();

            moveUp = true;
            moveSpeed = 3.5f;

            hitDataMelee = new HitData();
            hitDataMelee.m_damage.m_pierce = 75.0f;
            hitDataMelee.m_dodgeable = true;
            hitDataMelee.SetAttacker(bonemass.character);

            hitDataPoison = new HitData();
            hitDataPoison.m_damage.m_poison = 85.0f;
            hitDataPoison.SetAttacker(bonemass.character);

            Instantiate(ZNetScene.instance.GetPrefab("vfx_gdking_stomp"), groundPosition, Quaternion.identity);
            Instantiate(ZNetScene.instance.GetPrefab("sfx_stonegolem_attack_hit"), groundPosition, Quaternion.identity);
        }

        /**
         * Control movement on update.
         */
        public void Update()
        {
            if (!nview!.IsValid() || !nview!.IsOwner())
            {
                return;
            }

            totalTime += Time.deltaTime;
            if (bonemass == null || totalTime > moveDownAfter)
            {
                moveUp = false;
            }

            MoveRock();
        }

        public void MoveRock()
        {
            if (moveUp)
            {
                if (gameObject.transform.position.y <= groundPosition.y - 1)
                {
                    gameObject.transform.position += 4 * moveSpeed * Vector3.up * Time.deltaTime;
                }
                else
                {
                    notRising = true;
                }
                return;
            }

            gameObject.transform.position += moveSpeed * Vector3.down * Time.deltaTime;
        }


        /**
         * Try to get Bonemass to apply damage calculations.
         */
        public Bonemass? TryGetBonemass()
        {
            if (bonemass != null)
            {
                return bonemass;
            }

            List<Character> characters = Character.GetAllCharacters();
            foreach (Character character in characters)
            {
                Bonemass boss;
                if ((boss = character.GetComponent<Bonemass>()) != null)
                {
                    return boss;
                }
            }
            return null;
        }

        /**
         * If player collides, damage them.
         */
        public void OnCollisionEnter(Collision other)
        {
            if (bonemass == null)
            {
                return;
            }

            Main.Log!.LogInfo(other.rigidbody.gameObject.name);

            Player player;
            if ((player = other.rigidbody.gameObject.GetComponent<Player>()) != null)
            {
                if (notRising)
                {
                    player.Damage(hitDataPoison);
                    return;
                }

                if (other.relativeVelocity.magnitude > 4)
                {
                    player.Damage(hitDataMelee);
                }
            }
        }

        private Bonemass? bonemass;
        private ZNetView? nview;
        public Vector3 initialPosition;
        public Vector3 groundPosition;
        public bool moveUp;
        public HitData? hitDataMelee;
        public HitData? hitDataPoison;
        public float moveSpeed;

        private float totalTime;
        private float moveDownAfter = 12.0f;

        private bool notRising = false;
    }
}
