using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedBossesRedone.Entity
{
    public class AncientOozer : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = zNetScene.GetPrefab("BlobTar").Clone("eb_blobEliteAncient");
            entity.transform.SetParent(Main.Holder!.transform, false);
            entity.transform.localScale = new Vector3(2.6f, 2.6f, 2.6f);
            entity.ApplyTint(new Color(0.1f, 0.1f, 1.0f));

            Rigidbody rb = entity.GetComponentInChildren<Rigidbody>();
            rb.isKinematic = true;

            ZSyncTransform zst = entity.GetComponentInChildren<ZSyncTransform>();
            zst.m_isKinematicBody = false;

            BaseAI baseAI = entity.GetComponent<BaseAI>();
            baseAI.m_aggravatable = false;
            baseAI.m_aggravated = false;
            baseAI.m_alerted = false;
            baseAI.m_hearRange = 0f;
            baseAI.m_huntPlayer = false;
            baseAI.m_viewAngle = 0f;
            baseAI.m_viewRange = 0f;

            MonsterAI monsterAI = entity.GetComponent<MonsterAI>();
            monsterAI.m_aggravatable = false;
            monsterAI.m_aggravated = false;
            monsterAI.m_alerted = false;
            monsterAI.m_alertRange = 0f;
            monsterAI.m_hearRange = 0f;
            monsterAI.m_huntPlayer = false;
            monsterAI.m_viewAngle = 0f;
            monsterAI.m_viewRange = 0f;

            Character character = entity.GetComponent<Character>();
            character.m_health = 5.0f;
            character.m_name = "Ancient Oozer";
            character.m_runSpeed = 0.0f;
            character.m_walkSpeed = 0.0f;

            CharacterDrop characterDrop = entity.GetComponent<CharacterDrop>();
            characterDrop.m_dropsEnabled = false;
            characterDrop.m_drops = new List<CharacterDrop.Drop>();

            Humanoid humanoid = entity.GetComponent<Humanoid>();
            humanoid.m_defaultItems = null;
            humanoid.m_health = 5.0f;
            humanoid.m_name = "Ancient Oozer";
            humanoid.m_runSpeed = 0.0f;
            humanoid.m_walkSpeed = 0.0f;
        }
    }
}
