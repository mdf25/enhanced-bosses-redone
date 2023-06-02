using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedBossesRedone.AttachmentScripts
{
    public class YagluthRockScript : MonoBehaviour
    {
        /**
         * Setup yagluth rock.
         */
        public void Awake()
        {
            Vector3 startPos = gameObject.transform.position;
            initialPosition = new Vector3(startPos.x, startPos.y, startPos.z);
            groundPosition = initialPosition.ConvertToWorldHeight();
            currentMoveDir = Vector3.zero;

            moveUp = true;
            moveSpeed = 3.5f;
            rockHeight = 12f;

            rigidbody = gameObject.GetComponent<Rigidbody>();
            nview = gameObject.GetComponent<ZNetView>();

            if (nview!.IsValid() && nview!.IsOwner())
            {
                Instantiate(ZNetScene.instance.GetPrefab("vfx_gdking_stomp"), groundPosition, Quaternion.identity);
                Instantiate(ZNetScene.instance.GetPrefab("sfx_stonegolem_attack_hit"), groundPosition, Quaternion.identity);
            }
        }

        /**
         * Control movement on update.
         */
        public void FixedUpdate()
        {
            if (!nview!.IsValid() || !nview!.IsOwner())
            {
                return;
            }

            time += Time.deltaTime;
            if (time > moveDownTime)
            {
                moveUp = false;
                raised = false;
            }

            MoveRock();
        }

        /**
         * Move the rock when needed.
         */
        public void MoveRock()
        {
            if (moveUp && !raised)
            {
                if (gameObject.transform.position.y <= groundPosition.y + rockHeight)
                {
                    currentMoveDir = 0.4f * moveSpeed * Vector3.up * Time.fixedDeltaTime;
                    rigidbody!.MovePosition(rigidbody.transform.position + currentMoveDir);
                    return;
                }
                raised = true;
                return;
            }

            if (raised)
            {
                return;
            }

            currentMoveDir = 0.4f * moveSpeed * Vector3.down * Time.fixedDeltaTime;
            gameObject.transform.position += currentMoveDir;
        }

        /**
         * When collision is entered
         */
        public void OnCollisionEnter(Collision collision)
        {
            GameObject? collided = collision.rigidbody.gameObject;
            if (collided == null && collided?.GetComponent<Character>() == null)
            {
                return;
            }
            collided.transform.parent = IsMoving() ? gameObject.transform : null;
        }

        public void OnCollisionExit(Collision collision)
        {
            GameObject? collided = collision.rigidbody.gameObject;
            if (collided == null && collided?.GetComponent<Character>() == null)
            {
                return;
            }
            collided.transform.parent = null;
        }

        public void OnDestroy()
        {
            int attachedObjects = gameObject.transform.childCount;
            for (int i = 0; i < attachedObjects; i += 1)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                child.transform.parent = null;
            }
        }

        public bool IsMoving()
        {
            return moveUp && !raised || !moveUp && raised;
        }

        private ZNetView? nview;

        public Rigidbody? rigidbody;
        public Vector3 initialPosition;
        public Vector3 groundPosition;
        public Vector3 currentMoveDir;
        public bool isYagluthRock;
        public bool moveUp;
        public bool raised;
        public HitData? hitData;
        public float moveSpeed;
        public float rockHeight;
        public int direction;

        private float time;
        private float moveDownTime = 45.0f;
    }
}
