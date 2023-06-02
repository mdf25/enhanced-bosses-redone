using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.Entity
{
    public class EikthyrThunderVortex : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
           
            entity = Main.Bundle!.LoadAsset<GameObject>("tornado.prefab").Clone("eb_thundervortex");
            entity.transform.localScale = new Vector3(5, 3, 5);
            entity.AddComponent<ZNetView>();
            entity.AddComponent<ZSyncTransform>();
            entity.AddComponent<Vortex>();

            ExtendedTimedDestruction timedDestruction = entity.AddComponent<ExtendedTimedDestruction>();
            timedDestruction.m_triggerOnAwake = true;
        }
    }
}
