using EnhancedBossesRedone.AttachmentScripts;
using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.EikthyrAttacks
{
    public class EikthyrCharge : CustomAttack
    {
        public EikthyrCharge()
        {
            name = "Eikthyr_charge";
            baseName = "Eikthyr_charge";
            bossName = "Eikthyr";
            stopOriginalAttack = false;
        }

        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!ConfigManager.EikthyrTeleport!.Value)
            {
                return;
            }

            Character targetCreature = monsterAI.m_targetCreature;
            if (targetCreature == null)
            {
                return;
            }

            Vector3 eyePoint = character.GetEyePoint();
            float y;
            Heightmap.GetHeight(character.transform.position, out y);
            RaycastHit raycastHit;
            Vector3 target = !Physics.Raycast(character.GetEyePoint(), character.GetLookDir(), out raycastHit, float.PositiveInfinity, Warp_Layermask) || !raycastHit.collider ? eyePoint + character.GetLookDir() * 1000f : raycastHit.point;
            target.y = y;
            float magnitude = (teleportDistance * character.GetLookDir()).magnitude;
            Vector3 position = Vector3.MoveTowards(eyePoint, target, magnitude);
            character.transform.position = position;
            Vector3 worldPosition = new Vector3(targetCreature.transform.position.x, character.transform.position.y, targetCreature.transform.position.z);
            character.transform.LookAt(worldPosition);
            Eikthyr.Thunder(character, character.transform.position);
        }

        public void Execute_Lightning(Character character, MonsterAI monsterAI)
        {
            Character targetCreature = monsterAI.m_targetCreature;
            if (targetCreature == null)
            {
                return;
            }

            Vector3 localPosition = character.transform.localPosition;
            Vector3 localPosition2 = targetCreature.transform.localPosition;
            localPosition.y = 0f;
            localPosition2.y = 0f;

            if (Vector3.Dot(localPosition - localPosition2, targetCreature.transform.forward) >= 0f)
            {
                character.transform.localPosition = targetCreature.transform.localPosition - targetCreature.transform.forward * 15f;
                character.transform.localRotation = targetCreature.transform.localRotation;
            }
            else
            {
                character.transform.localPosition = targetCreature.transform.localPosition + targetCreature.transform.forward * 15f;
                character.transform.localRotation = targetCreature.transform.localRotation;
                character.transform.RotateAround(character.transform.localPosition, character.transform.up, 180f);
            }

            Vector3 worldPosition = new Vector3(targetCreature.transform.position.x, character.transform.position.y, targetCreature.transform.position.z);
            character.transform.LookAt(worldPosition);
            GameObject prefab = ZNetScene.instance.GetPrefab("fx_eikthyr_forwardshockwave");
            Object.Instantiate(prefab, character.transform.position, Quaternion.LookRotation(character.GetLookDir()));
            HitData hitData = new HitData();
            hitData.m_damage.m_lightning = 20f;
            hitData.SetAttacker(character);
            foreach (RaycastHit raycastHit in Physics.SphereCastAll(character.GetEyePoint(), 5f, character.GetLookDir(), 15f, ScriptChar_Layermask))
            {
                Collider collider = raycastHit.collider;
                Character? character2;
                if (collider == null)
                {
                    character2 = null;
                }
                else
                {
                    GameObject gameObject = collider.gameObject;
                    character2 = gameObject != null ? gameObject.GetComponent<Character>() : null;
                }

                Character character3 = character2!;
                if (character3 != null && BaseAI.IsEnemy(character, character3))
                {
                    character3.Damage(hitData);
                }
            }
        }

        public float teleportDistance = 30f;

        public int Warp_Layermask = LayerMask.GetMask(new string[]
        {
            "Default",
            "static_solid",
            "Default_small",
            "piece_nonsolid",
            "terrain",
            "vehicle",
            "piece",
            "viewblock",
            "Water",
            "character",
            "character_net",
            "character_ghost"
        });

        public int ScriptChar_Layermask = LayerMask.GetMask(new string[]
        {
            "Default",
            "static_solid",
            "Default_small",
            "piece_nonsolid",
            "terrain",
            "vehicle",
            "piece",
            "viewblock",
            "character",
            "character_net",
            "character_ghost"
        });
    }
}
