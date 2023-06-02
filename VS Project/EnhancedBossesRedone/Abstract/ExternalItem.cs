using EnhancedBossesRedone.Data;
using UnityEngine;

namespace EnhancedBossesRedone.Abstract
{
    public abstract class ExternalItem
    {
        public virtual void Setup(ObjectDB objectDB)
        {
            if (objectDB == null)
            {
                Main.Log!.LogError("No objectDB found.");
            }
        }

        public void AddToObjectDB(ObjectDB objectDB)
        {
            if (objectDB == null)
            {
                Main.Log!.LogError("objectDB not found. Skipping adding item.");
                return;
            }
            if (item == null)
            {
                Main.Log!.LogError("No entity found to load.");
                return;
            }
            objectDB.AddCustomAttack(item);
        }

        public GameObject? item;
        public static string? name;
    }
}
