using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;
using EnhancedBossesRedone.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedBossesRedone.Bosses
{
    public class Yagluth : Boss
    {
        public Yagluth()
        {
            bossName = "GoblinKing";
        }

        public override void PopulateDefaultValues()
        {
            HitData.DamageModifiers modifiers = new HitData.DamageModifiers();
            modifiers.m_blunt = HitData.DamageModifier.Normal;
            modifiers.m_slash = HitData.DamageModifier.Normal;
            modifiers.m_pierce = HitData.DamageModifier.VeryResistant;
            modifiers.m_chop = HitData.DamageModifier.Ignore;
            modifiers.m_pickaxe = HitData.DamageModifier.Ignore;
            modifiers.m_fire = HitData.DamageModifier.Resistant;
            modifiers.m_frost = HitData.DamageModifier.Normal;
            modifiers.m_lightning = HitData.DamageModifier.Normal;
            modifiers.m_poison = HitData.DamageModifier.Immune;
            modifiers.m_spirit = HitData.DamageModifier.Normal;

            defaultModifiers = modifiers;
            defaultWalkSpeed = 5;
            defaultRunSpeed = 4;
        }

        /**
		 * Add component to Yagluth.
		 */
        public override Boss AddBossComponent(GameObject gameObject)
        {
            return gameObject.AddComponent<Yagluth>();
        }

        /**
         * Update Yagluth's rock formation.
         */
        public void Update()
        {
            if (!zNetView!.IsValid() || !zNetView!.IsOwner())
            {
                return;
            }

            AdjustRockFormationMovement();
        }

        /**
		 * Checks whether Yagluth is currently in the rock formation.
		 */
        public bool IsInRockFormation()
        {
            isInRockFormation = zNetView!.GetZDO().GetBool("rockformation");
            return isInRockFormation;
        }

        /**
		 * Assigns rock formation
		 */
        public void SetRockFormation(bool rockFormation)
        {
            isInRockFormation = rockFormation;
            zNetView!.GetZDO().Set("rockformation", rockFormation);
        }

        public void AdjustRockFormationMovement()
        {
            if (IsInRockFormation())
            {
                rockFormationTime += Time.deltaTime;
                if (rockFormationTime > rockFormationDuration)
                {
                    SetRockFormation(false);
                    rockFormationTime = 0;
                    return;
                }

                character!.m_walkSpeed = 0;
                character!.m_runSpeed = 0;
                return;
            }
            character!.m_walkSpeed = defaultWalkSpeed;
            character!.m_runSpeed = defaultRunSpeed;
        }

        /**
		 * Yagluth's thunder strike.
		 */
        public static void Thunder(Character character, Vector3 position, bool processDamage = true)
        {
            float num;
            Heightmap.GetHeight(position, out num);
            Vector3 position2 = new Vector3(position.x - 0.5f, num - 5f, position.z + 1f);

            Yagluth? boss = character.GetBoss() as Yagluth;
            if (boss == null)
            {
                return;
            }

            coroutine = ThunderInRange(character, position2, processDamage);
            boss.StartCoroutine(coroutine);
        }

        /**
		 * Trigger rock spawns over a time frame
		 */
        public IEnumerator TriggerRockSpawns(int rocksPerTier, float radius, float yagHeight, int tiers)
        {
            for (int i = 2; i <= tiers + 1; i += 1)
            {
                yield return new WaitForSeconds(0.5f);
                float height = (tiers + 2 - i) * yagHeight / tiers;
                float rad = i * radius;
                float angleDivide = rocksPerTier * i;
                float angle = 360 / angleDivide;
                for (int j = 0; j < angleDivide; j += 1)
                {
                    yield return new WaitForSeconds(0.05f);
                    float x = Mathf.Cos(j * angle) * rad;
                    float y = Mathf.Sin(j * angle) * rad;
                    Vector3 circlePosition = new Vector3(x, 0, y);
                    Vector3 roughWorldPos = character!.transform.position + circlePosition;
                    Vector3 rockPos = roughWorldPos.ConvertToWorldHeight() - 3 * Vector3.up;
                    GameObject rock = Instantiate(ZNetScene.instance.GetPrefab("eb_redrock"), rockPos, Quaternion.Euler(Random.Range(-15, 15), Random.Range(0, 360), Random.Range(-15, 15)));
                    rock.transform.Find("Parent").localScale *= Random.Range(0.3f, 0.5f);
                    rock.GetComponent<YagluthRockScript>().rockHeight = height;
                }
            }
        }

        /**
		 * Strike lightning in a range.
		 */
        public static IEnumerator ThunderInRange(Character character, Vector3 position, bool processDamage = true)
        {
            for (int i = 0; i < 3; i += 1)
            {
                float radius = 10f;
                Vector3 roughposition = position + radius * (Vector3)Random.insideUnitCircle;
                Vector3 worldPosition = roughposition.ConvertToWorldHeight();
                float num;
                Heightmap.GetHeight(roughposition, out num);
                Vector3 strike = worldPosition - 5 * Vector3.up;
                Instantiate(ZNetScene.instance.GetPrefab("vfx_RockDestroyed_large"), worldPosition, Quaternion.identity);
                Instantiate(ZNetScene.instance.GetPrefab("sfx_gdking_rock_destroyed"), worldPosition, Quaternion.identity);
                GameObject plume = Instantiate(ZNetScene.instance.GetPrefab("vfx_dragon_coldbreath"), strike, Quaternion.Euler(-90f, 0f, 0f));
                plume.ApplyTint(Color.red);

                yield return new WaitForSeconds(1.3f);
                Instantiate(ZNetScene.instance.GetPrefab("vfx_RockDestroyed_large"), worldPosition, Quaternion.identity);
                Instantiate(ZNetScene.instance.GetPrefab("sfx_stonegolem_attack_hit"), worldPosition, Quaternion.identity);
                GameObject lightning = Instantiate(ZNetScene.instance.GetPrefab("fx_eikthyr_forwardshockwave"), strike, Quaternion.Euler(-90f, 0f, 0f));
                lightning.transform.localScale = new Vector3(1f, 0.2f, 0.2f);
                lightning.ApplyTint(Color.red);
                if (!processDamage)
                {
                    yield return new WaitForSeconds(0.2f);
                }

                HitData hitData = new HitData();
                hitData.m_damage.m_lightning = Random.Range(55f, 85f);
                hitData.SetAttacker(character);

                List<Character> enemies = character.FindEnemies(worldPosition, 4f);
                if (enemies.Count == 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                foreach (Character enemy in enemies)
                {
                    if (enemy == null)
                    {
                        continue;
                    }

                    enemy.Damage(hitData);
                }
            }
        }

        private static IEnumerator? coroutine;
        private bool isInRockFormation;

        private float rockFormationTime;
        private float rockFormationDuration = 50f;
    }
}
