using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.Entity
{
    public class ModerFireProjectile : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = zNetScene.GetPrefab("Imp_fireball_projectile").Clone("eb_dragon_fire_projectile");
            entity.transform.SetParent(Main.Holder!.transform, false);
            entity.transform.localScale *= 2f;
            entity.ApplyTint(Color.red);

            Light? light = entity.transform.Find("Point light").gameObject.GetComponent<Light>();
            light!.intensity = 5;
            light!.color = new Color(1.0f, 0.2f, 0.3f);

            ParticleSystem? particles = entity.transform.Find("flames_world").gameObject.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = particles.main;
            main.startLifetime = 5.0f;
            main.startSpeed = 0.3f;

            Projectile projectile = entity.GetComponent<Projectile>();
            projectile.m_ttl = 5;
            projectile.m_spawnOnHit = zNetScene.GetPrefab("eb_landfire");
        }
    }
}
