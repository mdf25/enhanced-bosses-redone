using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.Entity
{
    public class HotBreath : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = zNetScene.GetPrefab("vfx_dragon_coldbreath").Clone("eb_dragon_hotbreath");
            entity.transform.SetParent(Main.Holder!.transform, false);
            entity.ApplyTint(new Color(1.0f, 0.325f, 0.286f));
        }
    }
}
