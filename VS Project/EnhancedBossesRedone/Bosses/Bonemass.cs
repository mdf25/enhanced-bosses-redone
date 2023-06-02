using System.Collections.Generic;
using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;
using EnhancedBossesRedone.Data;
using EnhancedBossesRedone.StatusEffects;
using UnityEngine;

namespace EnhancedBossesRedone.Bosses
{
    public class Bonemass : Boss
    {
        public Bonemass()
        {
            bossName = "Bonemass";
        }

        public override void Awake()
        {
            base.Awake();

            SetupModifiers();
            trip = ScriptableObject.CreateInstance<SE_Trip>();
            slow = ScriptableObject.CreateInstance<SE_Slow>();
            oozersInRange = new List<Character>();

            SkinnedMeshRenderer[] renderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < renderers.Length; i += 1)
            {
                Material[] materials = renderers[i].materials;
                for (int j = 0; j < materials.Length; j += 1)
                {
                    defaultColors.Add(materials[j].GetColor("_EmissiveColor"));
                }
            }

            InvokeRepeating("UpdateAura", 0f, 0.5f);
            InvokeRepeating("UpdateShieldStatus", 0f, 0.1f);
            InvokeRepeating("UpdateHealthGain", 0f, 1.0f);
        }

        public override void PopulateDefaultValues()
        {
            HitData.DamageModifiers modifiers = new HitData.DamageModifiers();
            modifiers.m_blunt = HitData.DamageModifier.Weak;
            modifiers.m_slash = HitData.DamageModifier.Resistant;
            modifiers.m_pierce = HitData.DamageModifier.VeryResistant;
            modifiers.m_chop = HitData.DamageModifier.Ignore;
            modifiers.m_pickaxe = HitData.DamageModifier.Ignore;
            modifiers.m_fire = HitData.DamageModifier.VeryResistant;
            modifiers.m_frost = HitData.DamageModifier.Weak;
            modifiers.m_lightning = HitData.DamageModifier.Normal;
            modifiers.m_poison = HitData.DamageModifier.Immune;
            modifiers.m_spirit = HitData.DamageModifier.Normal;

            defaultModifiers = modifiers;
            defaultWalkSpeed = 5;
            defaultRunSpeed = 4;
        }

        public override Boss AddBossComponent(GameObject gameObject)
        {
            return gameObject.AddComponent<Bonemass>();
        }

        public void SetupModifiers()
        {
            newModifiers = new HitData.DamageModifiers();
            newModifiers.m_blunt = HitData.DamageModifier.Immune;
            newModifiers.m_chop = HitData.DamageModifier.Immune;
            newModifiers.m_fire = HitData.DamageModifier.Immune;
            newModifiers.m_frost = HitData.DamageModifier.Immune;
            newModifiers.m_lightning = HitData.DamageModifier.Immune;
            newModifiers.m_pickaxe = HitData.DamageModifier.Immune;
            newModifiers.m_pierce = HitData.DamageModifier.Immune;
            newModifiers.m_poison = HitData.DamageModifier.Immune;
            newModifiers.m_slash = HitData.DamageModifier.Immune;
            newModifiers.m_spirit = HitData.DamageModifier.Immune;
        }

        public void SetShieldState(bool state)
        {
            isShielded = state;
            zNetView!.GetZDO().Set("shielded", state);
            
        }

        public bool IsShielded()
        {
            isShielded = zNetView!.GetZDO().GetBool("shielded");
            return isShielded;
        }

        public void UpdateShieldStatus()
        {
            if (!zNetView!.IsValid() || !zNetView!.IsOwner())
            {
                return;
            }

            FindAncientOozers();
            SetShieldState(oozersInRange!.Count > 0);
            
            humanoid!.m_damageModifiers = IsShielded() ? newModifiers : defaultModifiers;

            t = Mathf.Clamp(isShielded ? t + 0.05f : t - 0.05f, 0.0f, 1.0f);
            if (!isShielded && t == 0 || isShielded && t == 1)
            {
                return;
            }

            int k = 0;
            SkinnedMeshRenderer[] renderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < renderers.Length; i += 1)
            {
                Material[] materials = renderers[i].materials;
                for (int j = 0; j < materials.Length; j += 1)
                {
                    materials[j].SetColor("_EmissiveColor", Color.Lerp(defaultColors[k], new Color(0, 0, 0.8f), t));
                    k += 1;
                }
            }
        }

        public void UpdateAura()
        {
            if (!zNetView!.IsValid() || !zNetView!.IsOwner())
            {
                return;
            }

            foreach (Player player in Helpers.FindPlayers(character!.transform.position, 10f))
            {
                if (trip != null)
                {
                    player.GetSEMan().AddStatusEffect(trip, true, 0, 0f);
                }

                if (slow != null)
                {
                    player.GetSEMan().AddStatusEffect(slow, true, 0, 0f);
                }
            }
        }

        public void UpdateHealthGain()
        {
            if (healthAmount > 0)
            {
                float amountToHeal = Mathf.Max(healthAmount / 20f, 10f);
                character!.Heal(amountToHeal);
                healthAmount = Mathf.Max(healthAmount - amountToHeal, 0);
            }
        }

        public void FindAncientOozers()
        {
            if (character == null)
            {
                return;
            }

            if (oozersInRange!.Count > 0)
            {
                UpdateAncientOozers();
                return;
            }

            List<Character> characters = new List<Character>();
            Character.GetCharactersInRange(character!.transform.position, 60f, characters);

            if (characters == null || characters.Count == 0)
            {
                return;
            }

            foreach (Character characterInRange in characters)
            {
                if (characterInRange.name.Replace("(Clone)", "") == "eb_blobEliteAncient")
                {
                    AncientSlimeScript script;
                    if ((script = characterInRange.GetComponent<AncientSlimeScript>()) == null)
                    {
                        script = characterInRange.gameObject.AddComponent<AncientSlimeScript>();
                    }
                    script.bonemass = this;
                    oozersInRange.Add(characterInRange);
                }
            }
        }

        public void UpdateAncientOozers()
        {
            ancientOozerLifetime += 0.1f;
            for (int i = oozersInRange!.Count - 1; i >= 0; i -= 1)
            {
                if (oozersInRange[i] == null)
                {
                    oozersInRange.RemoveAt(i);
                }
            }

            Main.Log!.LogInfo("Ancient Oozers Alive: " + oozersInRange.Count.ToString());
            if (ancientOozerLifetime > ConfigManager.BonemassAncientSlimeLifetime!.Value)
            {
                Main.Log!.LogInfo("Killing oozers.");
                foreach (Character oozer in oozersInRange!)
                {
                    oozer!.SetHealth(0.0f);
                    healthAmount += ConfigManager.BossConfigs![bossName!]["bonemass_heal_minions"].Heal / ConfigManager.BossConfigs![bossName!]["bonemass_heal_minions"].spawnMinionsCount;
                }
                oozersInRange.Clear();

                GameObject healPulse = Object.Instantiate(ZNetScene.instance.GetPrefab("vfx_Potion_stamina_medium"), character!.transform.position, Quaternion.identity);
                healPulse.transform.localScale *= 8;
                ancientOozerLifetime = 0;
            }
        }

        private float t;
        private List<Color> defaultColors = new List<Color>();
        private bool isShielded;
        private HitData.DamageModifiers newModifiers;
        private SE_Trip? trip;
        private SE_Slow? slow;

        private List<Character>? oozersInRange;
        private float healthAmount;
        private float ancientOozerLifetime;
    }

}
