using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.Entity
{
    public class ElderLeafTornado : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
           
            entity = Main.Bundle!.LoadAsset<GameObject>("leaftornado.prefab").Clone("eb_leaftornado");
            entity.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
            entity.AddComponent<ZNetView>();
            entity.AddComponent<ZSyncTransform>();

            TimedDestruction timedDestruction = entity.AddComponent<TimedDestruction>();
            timedDestruction.m_triggerOnAwake = true;
            timedDestruction.m_timeout = 20.0f;
        }
    }
}
