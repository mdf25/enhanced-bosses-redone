using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.AttachmentScripts;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.Entity
{
    public class EikthyrThundercloud : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = zNetScene.GetPrefab("vfx_mistlands_mist").Clone("eb_thundercloud");
            entity.transform.SetParent(Main.Holder!.transform, false);
            entity.transform.localScale = new Vector3(0.3f, 0.2f, 0.3f);
            entity.ApplyTint(new Color(0.4f, 0.1f, 0.4f));

            TimedDestruction timedDestruction = entity.AddComponent<TimedDestruction>();
            timedDestruction.m_timeout = 60.0f;
            timedDestruction.m_triggerOnAwake = true;

            ParticleSystem particle = entity.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = particle.main;
            main.startLifetime = 3.0f;
            main.startColor = Color.grey;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            main.startSize = 25.0f;
            main.maxParticles = 25;

            entity.AddComponent<CloudScript>();
        }
    }
}
