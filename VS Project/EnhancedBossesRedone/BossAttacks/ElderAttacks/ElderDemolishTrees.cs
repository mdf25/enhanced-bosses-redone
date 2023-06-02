using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Bosses;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.ElderAttacks
{
    public class ElderDemolishTrees : CustomAttack
    {
        public ElderDemolishTrees()
        {
            name = "gd_king_demolish";
            baseName = "gd_king_rootspawn";
            bossName = "gd_king";
            stopOriginalAttack = true;
        }

        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_name = name;
            itemDrop!.m_itemData.m_shared.m_aiAttackRange = 50f;
            itemDrop!.m_itemData.m_shared.m_aiAttackRangeMin = 10f;
        }

        public override bool CanUseAttack(Character character, MonsterAI monsterAI)
        {
            treesToDestroy = new List<TreeBase>();
            if (monsterAI == null || !monsterAI.HaveTarget() || monsterAI.GetTargetCreature() == null)
            {
                return false;
            }

            treesToDestroy = Elder.FindNearTrees(monsterAI.GetTargetCreature().transform.position, searchRadius);
            return (treesToDestroy.Count > 0 && base.CanUseAttack(character, monsterAI));
        }

        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }
            DemolishTrees(monsterAI);
        }

        public void DemolishTrees(MonsterAI monsterAI)
        {
            foreach (TreeBase tree in treesToDestroy!)
            {
                DemolishTree(monsterAI, tree);
            }
        }

        public void DemolishTree(MonsterAI monsterAI, TreeBase tree)
        {
            tree.m_destroyedEffect.Create(tree.transform.position, tree.transform.rotation, tree.transform, 1f, -1);
            Vector3 hitDir = monsterAI.GetTargetCreature().transform.position - tree.transform.position;
            tree.SpawnLog(hitDir);
            List<GameObject> dropList = tree.m_dropWhenDestroyed.GetDropList();
            for (int i = 0; i < dropList.Count; i++)
            {
                Vector2 vector = Random.insideUnitCircle * 0.5f;
                Vector3 position = tree.transform.position + Vector3.up * tree.m_spawnYOffset + new Vector3(vector.x, tree.m_spawnYStep * i, vector.y);
                Quaternion rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
                Object.Instantiate(dropList[i], position, rotation);
            }
            tree.gameObject.SetActive(false);
            tree.m_nview.Destroy();
        }

        public float searchRadius = 10f;

        public List<TreeBase>? treesToDestroy = new List<TreeBase>();
    }
}
