using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.EikthyrAttacks
{
    public class EikthyrProjectile : CustomAttack
    {
        public EikthyrProjectile()
        {
            name = "Eikthyr_projectile";
            baseName = "Eikthyr_charge";
            bossName = "Eikthyr";
            stopOriginalAttack = false;
        }

        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }
            
            Character target = monsterAI.GetTargetCreature();
            Vector3 targetDirection = (target.transform.position - character.transform.position).normalized;

            Vector3[] directions = new Vector3[3];
            directions[0] = Quaternion.Euler(0, Random.Range(-5, -5), 0) * targetDirection;
            directions[1] = Quaternion.Euler(0, Random.Range(-10, -15), 0) * targetDirection;
            directions[2] = Quaternion.Euler(0, Random.Range(10, 15), 0) * targetDirection;

            Vector3 positionInFront = character.transform.position + character.transform.forward * 4f + Vector3.up * 2f;
            GameObject prefab = ZNetScene.instance.GetPrefab("eb_thunderprojectile");
            float healthPerc = character.GetHealthPercentage() / GetHpThreshold();

            for (int i = 0; i < 6; i += 1)
            {
                if (healthPerc > 0.8 && i > 0)
                {
                    break;
                }
                if (healthPerc > 0.65 && healthPerc <= 0.8 && (i == 0 || i > 2))
                {
                    continue;
                }
                if (healthPerc > 0.5 && i > 2)
                {
                    break;
                }
                if (healthPerc > 0.4 && i > 3)
                {
                    break;
                }

                GameObject thunderProjectile = Object.Instantiate(prefab, positionInFront, Quaternion.identity);
                ThunderProjectile projectile = thunderProjectile.GetComponent<ThunderProjectile>();
                TimedDestruction timedDestruction = thunderProjectile.AddComponent<TimedDestruction>();
                timedDestruction.m_timeout = Random.Range(4.0f, 7.5f);
                timedDestruction.Trigger();
                projectile.speed = Random.Range(9f, 15f);
                projectile.character = character;
                projectile.targetDirection = directions[i % 3];
            }
        }

        public override void AdjustAttackParameters()
        {
            itemDrop!.m_itemData.m_shared.m_aiPrioritized = true;
            itemDrop!.m_itemData.m_shared.m_aiAttackRange = 50f;
            itemDrop!.m_itemData.m_shared.m_aiAttackRangeMin = 15f;
        }
    }
}
