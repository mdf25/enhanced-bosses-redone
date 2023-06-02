using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.Entity
{
    public class BonemassBlueAOE : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = zNetScene.GetPrefab("bonemass_aoe").Clone("eb_bonemass_aoe_blue");
            entity.transform.SetParent(Main.Holder!.transform, false);
            entity.ApplyTint(Color.blue);
            entity.transform.localScale *= 1.15f;
        }
    }
}
