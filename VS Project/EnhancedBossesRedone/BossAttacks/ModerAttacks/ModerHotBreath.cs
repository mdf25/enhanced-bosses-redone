using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Bosses;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.BossAttacks.ModerAttacks
{
    public class ModerHotBreath : CustomAttack
    {
        public ModerHotBreath()
        {
            name = "dragon_hotbreath";
            baseName = "dragon_coldbreath";
            bossName = "Dragon";
            stopOriginalAttack = false;
        }

        public override void AdjustAttackParameters()
        {
            HitData.DamageTypes damages = new HitData.DamageTypes();
            damages.m_chop = 1000;
            damages.m_pickaxe = 1000;
            damages.m_fire = 200;

            itemDrop!.m_itemData.m_shared.m_damages = damages;
        }

        public override void AdjustAttackParametersLate()
        {
            itemDrop!.m_itemData.m_shared.m_trailStartEffect.m_effectPrefabs[0].m_prefab = ZNetScene.instance.GetPrefab("eb_dragon_hotbreath");

            EffectList hitEffect = new EffectList();
            hitEffect.m_effectPrefabs = new EffectList.EffectData[3];
            for (int i = 0; i < hitEffect.m_effectPrefabs.Length; i += 1)
            {
                hitEffect.m_effectPrefabs[i] = new EffectList.EffectData();
                switch (i)
                {
                    case 0:
                        hitEffect.m_effectPrefabs[i].m_prefab = ZNetScene.instance.GetPrefab("vfx_HitSparks");
                        break;
                    case 1:
                        hitEffect.m_effectPrefabs[i].m_prefab = ZNetScene.instance.GetPrefab("sfx_greydwarf_attack_hit");
                        break;
                    case 2:
                        hitEffect.m_effectPrefabs[i].m_prefab = ZNetScene.instance.GetPrefab("eb_landfire");
                        break;
                }
            }
            itemDrop!.m_itemData.m_shared.m_hitEffect = hitEffect;

            EffectList hitTerrainEffect = new EffectList();
            hitTerrainEffect.m_effectPrefabs = new EffectList.EffectData[1];
            for (int i = 0; i < hitTerrainEffect.m_effectPrefabs.Length; i += 1)
            {
                switch (i)
                {
                    case 1:
                        hitTerrainEffect.m_effectPrefabs[i].m_prefab = ZNetScene.instance.GetPrefab("eb_landfire");
                        break;
                }
            }
            itemDrop!.m_itemData.m_shared.m_hitTerrainEffect = hitTerrainEffect;
        }

        public override bool CanUseAttack(Character character, MonsterAI monsterAI)
        {
            Moder? moder = character.GetBoss() as Moder;
            if (moder == null)
            {
                return false;
            }

            return moder.IsInAncientState() && base.CanUseAttack(character, monsterAI);
        }


        public override void OnAttackTriggered(Character character, MonsterAI monsterAI)
        {
            if (!character.m_nview.IsValid() || !character.m_nview.IsOwner())
            {
                return;
            }

            Moder? moder = character.GetBoss() as Moder;
            if (moder == null)
            {
                return;
            }

            moder.StartCoroutine(moder.GenerateLandFire());
        }
    }
}
