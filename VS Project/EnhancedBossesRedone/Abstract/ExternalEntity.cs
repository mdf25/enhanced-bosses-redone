using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.Abstract
{
    public abstract class ExternalEntity
    {
        public virtual void Setup(ZNetScene zNetScene)
        {
            if (zNetScene == null)
            {
                Main.Log!.LogError("No ZNetScene found.");
            }
        }

        public void AddToPrefabs(ZNetScene zNetScene)
        {
            if (zNetScene == null)
            {
                Main.Log!.LogError("ZNetScene not found. Skipping adding item.");
                return;
            }
            if (entity == null)
            {
                Main.Log!.LogError("No entity found to load.");
                return;
            }
            zNetScene!.AddCustomPrefab(entity);
        }

        public GameObject? entity;
    }
}
