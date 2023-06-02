using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.Entity
{
    public class BonemassRock : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            //entity = zNetScene.GetPrefab("Rock_3").Clone("eb_greenrock");
            entity = Main.Bundle!.LoadAsset<GameObject>("bonemassrock.prefab").Clone("eb_greenrock");
            entity.transform.SetParent(Main.Holder!.transform, false);
            //entity.ApplyTint(new Color(0.4f, 0.9f, 0.5f));
            //entity.transform.localScale = new Vector3(3.2f, 5.0f, 3.2f);
            entity.transform.localScale *= 0.65f;

            TimedDestruction timedDestruction = entity.AddComponent<TimedDestruction>();
            timedDestruction.m_timeout = 20.0f;
            timedDestruction.m_triggerOnAwake = true;

            //Destructible destructibe = entity.GetComponent<Destructible>();
            //destructibe.m_health = 1000;
            //destructibe.m_spawnWhenDestroyed = null;

            //Rigidbody rigidbody = entity.AddComponent<Rigidbody>();
            //rigidbody.isKinematic = true;

            ZNetView zNetView = entity.AddComponent<ZNetView>();
            zNetView.m_type = ZDO.ObjectType.Solid;
            zNetView.m_ghost = false;
            zNetView.m_body = entity.GetComponent<Rigidbody>();
            zNetView.m_distant = false;
            zNetView.m_persistent = false;
            zNetView.m_syncInitialScale = true;
            

            ZSyncTransform zst = entity.AddComponent<ZSyncTransform>();
            zst.m_isKinematicBody = true;
            zst.m_useGravity = false;

            entity.AddComponent<BonemassRockScript>();
        }
    }
}
