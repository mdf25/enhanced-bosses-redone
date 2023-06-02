using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.Entity
{
    public class YagluthRock : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = Main.Bundle!.LoadAsset<GameObject>("yagluthbigrock.prefab").Clone("eb_redrock");
            entity.transform.SetParent(Main.Holder!.transform, false);
            entity.transform.Find("Parent").gameObject.ApplyTint(new Color(0.9f, 0.4f, 0.2f));

            TimedDestruction timedDestruction = entity.AddComponent<TimedDestruction>();
            timedDestruction.m_timeout = 60.0f;
            timedDestruction.m_triggerOnAwake = true;

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

            entity.AddComponent<YagluthRockScript>();
        }
    }
}
