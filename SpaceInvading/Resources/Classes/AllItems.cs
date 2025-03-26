using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvading.Resources.Classes
{
    internal class AllItems
    {
        #region Upgrades

        public readonly static Item HealthPotion = new Item
        {
            Worth = 10,
            isUsable = true
        };

        public readonly static Item ShieldPotion = new Item
        {
            Worth = 10,
            isUsable = true
        };

        public readonly static Item Gun = new Item
        {
            Worth = 10,
            isUsable = true
        };

        public readonly static Item RagePotion = new Item
        {
            Worth = 10,
            isUsable = true
        };

        public readonly static Item Crossbow = new Item
        {
            Worth = 10,
            isUsable = true
        };

        public readonly static Item EnchantedSword = new Item
        {
            Worth = 10,
            isUsable = true
        };

        public readonly static List<Item> DroppableUpgrades = new List<Item>
        {
            Gun, Crossbow, EnchantedSword, RagePotion, ShieldPotion, HealthPotion
        };

        #endregion

        #region Drops

        public readonly static Item SlimeDrop = new Item
        {
            Worth = 1,
            isUsable = false
        };

        public readonly static Item SpiderDrop = new Item
        {
            Worth = 1,
            isUsable = false
        };

        #endregion

    }
}
