using EnhancedBossesRedone.Abstract;
using System;
using UnityEngine;

namespace EnhancedBossesRedone.Data
{
    internal class PinManager
    {
        public static Minimap.PinData AddBossPin(Vector3 pos)
        {
            Minimap.PinData pinData = new Minimap.PinData
            {
                m_type = BossPinType,
                m_icon = BossPinSprite,
                m_name = "",
                m_pos = pos,
                m_save = false,
                m_checked = false,
                m_ownerID = 0L
            };
            Minimap.instance.m_pins.Add(pinData);
            return pinData;
        }

        public static void CheckBossPins()
        {
            for (int i = Main.pinsList.Count - 1; i >= 0; i--)
            {
                Boss boss = Main.pinsList[i];
                bool flag = boss.character != null;
                if (flag)
                {
                    boss.UpdatePosition();
                }
                else
                {
                    boss.OnDeath();
                    Main.pinsList.RemoveAt(i);
                }
            }
        }

        public static Minimap.PinType BossPinType = Minimap.PinType.Boss;
        public static Sprite BossPinSprite = Minimap.instance.GetSprite(Minimap.PinType.Boss);
    }
}
