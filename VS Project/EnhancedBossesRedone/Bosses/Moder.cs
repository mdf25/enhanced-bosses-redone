using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.Abstract;

namespace EnhancedBossesRedone.Bosses
{
    public class Moder : Boss
    {
        public Moder()
        {
            bossName = "Dragon";
        }

        public override void PopulateDefaultValues()
        {
            HitData.DamageModifiers modifiers = new HitData.DamageModifiers();
            modifiers.m_blunt = HitData.DamageModifier.Normal;
            modifiers.m_slash = HitData.DamageModifier.Normal;
            modifiers.m_pierce = HitData.DamageModifier.Normal;
            modifiers.m_chop = HitData.DamageModifier.Ignore;
            modifiers.m_pickaxe = HitData.DamageModifier.Ignore;
            modifiers.m_fire = HitData.DamageModifier.Weak;
            modifiers.m_frost = HitData.DamageModifier.Immune;
            modifiers.m_lightning = HitData.DamageModifier.Normal;
            modifiers.m_poison = HitData.DamageModifier.Normal;
            modifiers.m_spirit = HitData.DamageModifier.Immune;

            defaultModifiers = modifiers;
            defaultWalkSpeed = 5;
            defaultRunSpeed = 6;
        }

        public override Boss AddBossComponent(GameObject gameObject)
        {
            return gameObject.AddComponent<Moder>();
        }

        public void SetupModifiers()
        {
            //defaultModifiers = humanoid!.GetDamageModifiers();
            newModifiers = new HitData.DamageModifiers();
            newModifiers.m_blunt = HitData.DamageModifier.Normal;
            newModifiers.m_chop = HitData.DamageModifier.Immune;
            newModifiers.m_fire = HitData.DamageModifier.Immune;
            newModifiers.m_frost = HitData.DamageModifier.Normal;
            newModifiers.m_lightning = HitData.DamageModifier.Normal;
            newModifiers.m_pickaxe = HitData.DamageModifier.Immune;
            newModifiers.m_pierce = HitData.DamageModifier.Weak;
            newModifiers.m_poison = HitData.DamageModifier.Resistant;
            newModifiers.m_slash = HitData.DamageModifier.Normal;
            newModifiers.m_spirit = HitData.DamageModifier.Immune;
        }


        public override void Awake()
        {
            base.Awake();
            SetupModifiers();

            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i += 1)
            {
                Material material = renderers[i].material;
                {
                    defaultColors.Add(material.color);
                }
            }

            InvokeRepeating("UpdateAncientStatus", 0f, 0.1f);
        }

        public void SetAncientState(bool state)
        {
            isInAncientState = state;
            zNetView!.GetZDO().Set("ancientstate", state);

            //character!.m_damageModifiers = isInAncientState ? newModifiers : defaultModifiers;
            
        }

        public bool IsInAncientState()
        {
            isInAncientState = zNetView!.GetZDO().GetBool("ancientstate");
            return isInAncientState;
        }

        public void UpdateAncientStatus()
        {
            if (!zNetView!.IsValid() || !zNetView!.IsOwner())
            {
                return;
            }

            t = Mathf.Clamp(isInAncientState ? t + 0.05f : t - 0.05f, 0.0f, 1.0f);
            if (!isInAncientState && t == 0 || isInAncientState && t == 1)
            {
                return;
            }

            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i += 1)
            {
                if (i > defaultColors.Count - 1)
                {
                    break;
                }

                Material material = renderers[i].material;
                material.color = Color.Lerp(defaultColors[i], new Color(0.95f, 0.2f, 0.3f), t);
            }
        }

        public void Update()
        {
            if (character == null || baseAI == null)
            {
                return;
            }

            humanoid!.m_damageModifiers = IsInAncientState() ? newModifiers : defaultModifiers;

            if (character.GetHealthPercentage() > ConfigManager.ModerHealthThreshold!.Value)
            {
                baseAI.m_chanceToLand = ConfigManager.ModerLandChanceBefore!.Value;
                baseAI.m_chanceToTakeoff = ConfigManager.ModerTakeOffChanceBefore!.Value;
            }
            else
            {
                baseAI.m_chanceToLand = ConfigManager.ModerLandChanceAfter!.Value;
                baseAI.m_chanceToTakeoff = ConfigManager.ModerTakeOffChanceAfter!.Value;
            }
        }


        public IEnumerator RemoveAncientStateAfter(float time)
        {
            yield return new WaitForSeconds(time);
            SetAncientState(false);
        }

        public IEnumerator GenerateLandFireEffect()
        {
            int spots = 5;
            List<KeyValuePair<float, Vector3>> spawnSpots = new List<KeyValuePair<float, Vector3>>();
            float angle = 360 / spots;
            for (int i = 0; i < spots; i += 1)
            {
                spawnSpots.Add(new KeyValuePair<float, Vector3>(Random.Range(0, 1), Quaternion.Euler(0, i * angle, 0) * Vector3.forward));
            }

            var sorted = from item in spawnSpots
                         orderby item.Key
                         select item;

            foreach (KeyValuePair<float, Vector3> spawnSpot in spawnSpots)
            {
                Vector3 position = character!.transform.position + (8.5f * spawnSpot.Value).ConvertToWorldHeight();
                Instantiate(ZNetScene.instance.GetPrefab("eb_landfire"), position, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
                Instantiate(ZNetScene.instance.GetPrefab("vfx_RockDestroyed_large"), position, Quaternion.identity);
                Instantiate(ZNetScene.instance.GetPrefab("sfx_gdking_rock_destroyed"), position, Quaternion.identity);
                GameObject plume = Instantiate(ZNetScene.instance.GetPrefab("vfx_dragon_coldbreath"), position, Quaternion.Euler(-90f, 0f, 0f));
                plume.ApplyTint(new Color(0.9f, 0.1f, 0.2f, 0.2f));
                yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
            }
        }

        public IEnumerator GenerateLandFire()
        {
            GameObject hotBreath = ZNetScene.instance.GetPrefab("eb_landfire");
            Vector3 breathOrigin = character!.GetEyePoint().ConvertToWorldHeight();
            Vector3 lookDir = (character.GetHeadPoint() - character.GetCenterPoint()).normalized;
            yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < 8; i += 1)
            {
                yield return new WaitForSeconds(0.1f);
                if (character == null)
                {
                    yield break;
                }

                Vector3 spawnPos = (breathOrigin + 3.5f * (i + 1) * lookDir).ConvertToWorldHeight();
                GameObject fire = Instantiate(hotBreath, spawnPos, Quaternion.identity);
                Aoe aoe = fire.GetComponent<Aoe>();
                aoe.m_owner = character;
            }
        }


        private float t;
        private List<Color> defaultColors = new List<Color>();
        private bool isInAncientState;
        private HitData.DamageModifiers newModifiers;
    }
}
