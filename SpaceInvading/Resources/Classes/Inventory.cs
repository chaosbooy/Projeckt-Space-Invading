using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvading.Resources.Classes
{
    public static class Inventory
    {
        public static List<Item> Items = new List<Item>();
        public static List<Item> PermamentUpgrades = new List<Item>();
        public static List<Item> UsableUpgrades = new List<Item>();
        public static Dictionary<string, int> ItemCount = new Dictionary<string, int>();
        public static Dictionary<string, int> PermamentUpgradesCount = new Dictionary<string, int>();
        public static Dictionary<string, int> UsableUpgradesCount = new Dictionary<string, int>();

        public static List<Item> GetItemsForShop(char shop)
        {
            List<Item> input = Items ;
            input.AddRange(PermamentUpgrades);
            input.AddRange(UsableUpgrades);
            List<Item> Return = new List<Item>();
            foreach (Item item in input)
            {
                if (item.Shop == shop && !Return.Contains(item))
                {
                    Return.Add(item);
                }
            }
            return Return;
        }
        #region Items
        public static void AddItem(Item item, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                Items.Add(item);
            }
            ItemCountf();
        }
        public static void RemoveItem(Item item, int quantity)
        {
            if (GetItemCount(item.Name) >= quantity)
            {
                for (int i = 0; i < quantity; i++)
                {
                    Items.Remove(item);
                }
            }
            ItemCountf();
        }
        public static void ItemCountf()
        {
            ItemCount.Clear();
            foreach (Item item in Items)
            {
                if (ItemCount.ContainsKey(item.Name))
                {
                    ItemCount[item.Name]++;
                }
                else
                {
                    ItemCount.Add(item.Name, 1);
                }
            }
        }
        public static int GetItemCount(string name)
        {

            if (ItemCount.ContainsKey(name))
            {
                return ItemCount[name];
            }
            return 0;
        }
        public static bool CheckForItem(Item item, int quantity)
        {
            if (ItemCount.ContainsKey(item.Name))
            {
                if (ItemCount[item.Name] >= quantity)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region permamentUpgrades
        public static void AddPermanentUpgrade(Item item)
        {
            PermamentUpgrades.Add(item);
            PermamentUpgradesCountf();
        }
        public static void RemovePermanentUpgrade(Item item)
        {
            PermamentUpgrades.Remove(item);
            PermamentUpgradesCountf();
        }
        public static void PermamentUpgradesCountf()
        {
            PermamentUpgradesCount.Clear();
            foreach (Item item in PermamentUpgrades)
            {
                if (PermamentUpgradesCount.ContainsKey(item.Name))
                {
                    PermamentUpgradesCount[item.Name]++;
                }
                else
                {
                    PermamentUpgradesCount.Add(item.Name, 1);
                }
            }
        }
        public static int GetPermamentUpgradeCount(string name)
        {
            if (PermamentUpgradesCount.ContainsKey(name))
            {
                return PermamentUpgradesCount[name];
            }
            return 0;
        }
        #endregion

        #region UsableUpgrades
        public static void AddUsableUpgrade(Item item)
        {
            UsableUpgrades.Add(item);
            UsableUpgradesCountf();
        }
        public static void RemoveUsableUpgrade(Item item)
        {
            UsableUpgrades.Remove(item);
            UsableUpgradesCountf();
        }
        public static void UsableUpgradesCountf()
        {
            UsableUpgradesCount.Clear();
            foreach (Item item in UsableUpgrades)
            {
                if (UsableUpgradesCount.ContainsKey(item.Name))
                {
                    UsableUpgradesCount[item.Name]++;
                }
                else
                {
                    UsableUpgradesCount.Add(item.Name, 1);
                }
            }
        }
        #endregion
       


    }
}
