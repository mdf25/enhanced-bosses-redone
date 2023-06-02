using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;
using System.Collections.Generic;
using UnityEngine;

namespace EnhancedBossesRedone.Entity
{
    public class LandFire : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = zNetScene.GetPrefab("DvergerStaffFire_clusterbomb_aoe").Clone("eb_landfire");
            entity.transform.SetParent(Main.Holder!.transform, false);
            entity.transform.localScale *= 0.7f;

            RemoveUnwantedGameobjects(entity);

            SphereCollider? collider = entity.GetComponent<SphereCollider>();
            Object.Destroy(collider);//.enabled = false;

            Aoe aoe = entity.GetComponent<Aoe>();
            aoe.m_damage.m_fire = 65;
            aoe.m_dodgeable = true;
            aoe.m_radius = 1.6f;
            aoe.m_hitInterval = 1.4f;
            aoe.m_ttl = 12.0f;
        }

        public void RemoveUnwantedGameobjects(GameObject entity)
        {
            GameObject? particles = GetParticleSystem(entity);
            if (particles == null)
            {
                return;
            }

            ParticleSystem? ps = particles.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Object.Destroy(ps);
            }
            ParticleSystemRenderer? psr = particles.GetComponent<ParticleSystemRenderer>();
            if (psr != null)
            {
                Object.Destroy(psr);
            }

            for (int i = 0; i < particles.transform.childCount; i += 1)
            {
                Transform child = particles.transform.GetChild(i);
                if (objectNamesToDelete.Contains(child.gameObject.name.Trim()))
                {
                    child.gameObject.SetActive(false);
                    //child.parent = null;
                }
            }
        }


        public GameObject? GetParticleSystem(GameObject entity)
        {
            Transform? particles = null;
            Transform? shockwave = null;

            for (int i = 0; i < entity.transform.childCount; i += 1)
            {
                Transform child = entity.transform.GetChild(i);
                if (child.gameObject.name == "particles")
                {
                    particles = child;
                }
            }

            if (particles == null)
            {
                return null;
            }

            for (int i = 0; i < particles.childCount; i += 1)
            {
                Transform child = particles.GetChild(i);
                if (child.gameObject.name == "shockwave_plane fast")
                {
                    shockwave = child;
                }
            }

            if (shockwave == null)
            {
                return null;
            }

            return shockwave.gameObject;
        }


        public List<string> objectNamesToDelete = new List<string>()
        {
            "flame ring",
            "smoke"
        };
    }
}