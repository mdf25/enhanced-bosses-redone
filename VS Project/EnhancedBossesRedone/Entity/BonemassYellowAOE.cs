using EnhancedBossesRedone.Abstract;
using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.Entity
{
    public class BonemassYellowAOE : ExternalEntity
    {
        public override void Setup(ZNetScene zNetScene)
        {
            base.Setup(zNetScene);
            entity = zNetScene.GetPrefab("bonemass_aoe").Clone("eb_bonemass_aoe_yellow");
            entity.transform.SetParent(Main.Holder!.transform, false);
            entity.ApplyTint(Color.yellow);
            entity.transform.localScale *= 1.3f;
        }
    }
}
