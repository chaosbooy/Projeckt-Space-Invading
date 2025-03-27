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
        public static List<Item> Upgrades = new List<Item>();
        public static Dictionary<string, int> ItemCount = new Dictionary<string, int>();
        public static Dictionary<string, int> PermamentUpgrades = new Dictionary<string, int>();
        public static Dictionary<string, int> UsableUpgrades = new Dictionary<string, int>();
        
        public static void count()
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
        public static void AddItem(Item item, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                Items.Add(item);
            }
            count();
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
            count();
        }
        public static void AddUpgrade(Item item)
        {
            Upgrades.Add(item);
            PermamentUpgradesCount();
        }
        public static void RemoveUpgrade(Item item)
        {
            Upgrades.Remove(item);
            PermamentUpgradesCount();
        }
        public static void PermamentUpgradesCount()
        {
            PermamentUpgrades.Clear();
            foreach (Item item in Upgrades)
            {
                if (PermamentUpgrades.ContainsKey(item.Name))
                {
                    PermamentUpgrades[item.Name]++;
                }
                else
                {
                    PermamentUpgrades.Add(item.Name, 1);
                }
            }
        }
        public static void UsableUpgradesCount()
        {
            UsableUpgrades.Clear();
            foreach (Item item in Upgrades)
            {
                if (UsableUpgrades.ContainsKey(item.Name))
                {
                    UsableUpgrades[item.Name]++;
                }
                else
                {
                    UsableUpgrades.Add(item.Name, 1);
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
        public static int GetPermamentUpgradeCount(string name)
        {
            if (PermamentUpgrades.ContainsKey(name))
            {
                return PermamentUpgrades[name];
            }
            return 0;
        }
        public static List<Item> GetItemsForShop(char shop)
        {
            List<Item> Return = new List<Item>();
            foreach (Item item in Items)
            {
                if (item.Shop == shop && !Return.Contains(item))
                {
                    Return.Add(item);
                }
            }
            return Return;
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
    }
}
