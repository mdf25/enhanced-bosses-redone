using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.Bosses
{
    public class Eikthyr : Boss
    {
        public Eikthyr()
        {
            bossName = "Eikthyr";
        }

        public override void Awake()
        {
            base.Awake();
            base.InvokeRepeating("AdjustMovementSpeed", 5.0f, 2.0f);
        }

        public override void PopulateDefaultValues()
        {
            HitData.DamageModifiers modifiers = new HitData.DamageModifiers();
            modifiers.m_blunt = HitData.DamageModifier.Normal;
            modifiers.m_slash = HitData.DamageModifier.Normal;
            modifiers.m_pierce = HitData.DamageModifier.Normal;
            modifiers.m_chop = HitData.DamageModifier.Ignore;
            modifiers.m_pickaxe = HitData.DamageModifier.Ignore;
            modifiers.m_fire = HitData.DamageModifier.Normal;
            modifiers.m_frost = HitData.DamageModifier.Normal;
            modifiers.m_lightning = HitData.DamageModifier.Normal;
            modifiers.m_poison = HitData.DamageModifier.Normal;
            modifiers.m_spirit = HitData.DamageModifier.Normal;

            defaultModifiers = modifiers;
            defaultWalkSpeed = 5;
            defaultRunSpeed = 8;
        }

        public override Boss AddBossComponent(GameObject gameObject)
        {
            return gameObject.AddComponent<Eikthyr>();
        }

        public static void Thunder(Character character, Vector3 position, bool processDamage = true)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }

            float num;
            Heightmap.GetHeight(position, out num);
            Vector3 position2 = new Vector3(position.x - 0.5f, num - 5f, position.z + 1f);
            Instantiate(ZNetScene.instance.GetPrefab("fx_eikthyr_forwardshockwave"), position2, Quaternion.Euler(-90f, 0f, 0f));
            if (!processDamage)
            {
                return;
            }

            HitData hitData = new HitData();
            hitData.m_damage.m_lightning = Random.Range(15f, 25f);
            hitData.SetAttacker(character);
            foreach (Character enemy in character.FindEnemies(2f))
            {
                enemy.Damage(hitData);
            }
        }

        public void AdjustMovementSpeed()
        {
            if (!zNetView!.IsValid() || !zNetView!.IsOwner())
            {
                return;
            }

            float t = 1 - character!.GetHealthPercentage();
            character.m_walkSpeed = defaultWalkSpeed * Mathf.Lerp(ConfigManager.EikthyrDefaultSpeed!.Value, ConfigManager.EikthyrMaxSpeed!.Value, t);
            character.m_runSpeed = defaultRunSpeed * Mathf.Lerp(ConfigManager.EikthyrDefaultSpeed!.Value, ConfigManager.EikthyrMaxSpeed!.Value, t);
        }
    }
}
