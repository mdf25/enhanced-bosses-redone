using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;
using EnhancedBossesRedone.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedBossesRedone.Bosses
{
    public class Elder : Boss
    {
        public Elder()
        {
            bossName = "gd_king";
        }

        public override void Awake()
        {
            base.Awake();
            InvokeRepeating("ProcessHealing", 1.0f, 1.0f);
        }

        public override void PopulateDefaultValues()
        {
            HitData.DamageModifiers modifiers = new HitData.DamageModifiers();
            modifiers.m_blunt = HitData.DamageModifier.Normal;
            modifiers.m_slash = HitData.DamageModifier.Normal;
            modifiers.m_pierce = HitData.DamageModifier.Normal;
            modifiers.m_chop = HitData.DamageModifier.Ignore;
            modifiers.m_pickaxe = HitData.DamageModifier.Ignore;
            modifiers.m_fire = HitData.DamageModifier.VeryWeak;
            modifiers.m_frost = HitData.DamageModifier.Normal;
            modifiers.m_lightning = HitData.DamageModifier.Normal;
            modifiers.m_poison = HitData.DamageModifier.Immune;
            modifiers.m_spirit = HitData.DamageModifier.Immune;

            defaultModifiers = modifiers;
            defaultWalkSpeed = 5;
            defaultRunSpeed = 6;
        }

        public void ProcessHealing()
        {
            if (amountToHeal == 0)
            {
                return;
            }

            float heal = Mathf.Max(amountToHeal / 20.0f, 10.0f);

            character!.Heal(heal);
            amountToHeal = Mathf.Max(amountToHeal - heal, 0);
        }


        public void AddToHealth()
        {
            amountToHeal += ConfigManager.BossConfigs![bossName!]["gd_king_heal"].Heal;
        }


        /**
		 * Add Elder component.
		 */
        public override Boss AddBossComponent(GameObject gameObject)
        {
            return gameObject.AddComponent<Elder>();
        }

        /**
		 * Find collection of trees.
		 */
        public static List<TreeBase> FindNearTrees(Vector3 position, float radius)
        {
            List<TreeBase> list = new List<TreeBase>();
            Collider[] array = Physics.OverlapSphere(position, radius);
            foreach (Collider collider in array)
            {
                TreeBase? treeBase;
                if (collider == null)
                {
                    continue;
                }

                GameObject gameObject = collider.gameObject;
                treeBase = gameObject != null ? gameObject.GetComponentInParent<TreeBase>() : null;
                if (treeBase != null && !list.Contains(treeBase))
                {
                    list.Add(treeBase);
                }
            }
            return list;
        }

        /**
		 * Find near logs.
		 */
        public static List<TreeLog> FindNearLogs(Vector3 position, float radius)
        {
            List<TreeLog> list = new List<TreeLog>();
            Collider[] array = Physics.OverlapSphere(position, radius);
            foreach (Collider collider in array)
            {
                TreeLog? treeLog;
                if (collider == null)
                {
                    continue;
                }

                GameObject gameObject = collider.gameObject;
                if (gameObject == null)
                {
                    continue;
                }

                treeLog = gameObject.GetComponentInParent<TreeLog>();
                if (treeLog == null || list.Contains(treeLog))
                {
                    continue;
                }

                if (treeLog.gameObject.GetComponent<MoveTowardsElder>() != null || treeLog.gameObject.GetComponent<MoveForThrow>())
                {
                    continue;
                }

                list.Add(treeLog);
            }
            return list;
        }

        /**
		 * Destroy a tree but don't influence fall direction.
		 */
        public static void DestroyTree(TreeBase tree, out TreeLog? log)
        {
            tree.m_destroyedEffect.Create(tree.transform.position, tree.transform.rotation, tree.transform, 1f, -1);
            tree.SpawnLog(Vector3.zero);
            List<GameObject> dropList = tree.m_dropWhenDestroyed.GetDropList();
            log = null; //log = Elder.FindNearLog(tree.transform.position, 6f);
            for (int i = 0; i < dropList.Count; i++)
            {
                Vector2 vector = Random.insideUnitCircle * 0.5f;
                Vector3 position = tree.transform.position + Vector3.up * tree.m_spawnYOffset + new Vector3(vector.x, tree.m_spawnYStep * i, vector.y);
                Quaternion rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
                GameObject treeDrop = Instantiate(dropList[i], position, rotation);
                if (log != null)
                {
                    log = treeDrop.GetComponentInParent<TreeLog>();
                }
            }

            tree.gameObject.SetActive(false);
            tree.m_nview.Destroy();
        }

        private float amountToHeal;
    }
}
