using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvading.Resources.Classes
{
    internal class AllItems
    {
        public readonly static Item Coin = new Item
        {
            Name = "Coin",
            Description = "currency",
            Shop = 'n',
            Worth = 1,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Coin.png"))
            }
        };
        

        #region Drops

        public readonly static Item SlimeDrop = new Item
        {
            Name = "Slime goo",
            Description = "slime goo, often used in medicine",
            Shop = 'm',
            Worth = 10,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/Slime/Slime_item.png"))
            }
        };

        public readonly static Item SpiderDrop = new Item
        {
            Name = "Spider web",
            Description = "very durable spider web",
            Shop = 'm',
            Worth = 20,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/Spider/Spider_item.png"))
            }
        };
        public readonly static Item SkeletonDrop = new Item
        {
            Name="Skull",
            Description = "cracked skull from a skeleton",
            Shop = 'm',
            Worth = 50,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/Skeleton/Skeleton_item.png"))
            }
        };
        public readonly static Item GolemDrop = new Item
        {
            Name = "Rune",
            Description = "A piece of rock with some magical rune inscribed on it",
            Shop = 'm',
            Worth = 100,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/Village/Close_frame.png"))
            }
        };
        public readonly static Item FireSpiritDrop = new Item
        {
            Name = "Fire essence",
            Description = "A fire in Pure form, used in magical research",
            Shop = 'm',
            Worth = 120,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/Village/Close_frame.png"))
            }
        };
        public readonly static Item Boss_1_drop_1 = new Item
        {
            Name = "Magic Crystal",
            Description = "A piece of a Powerfull magic crystal containg a lot of energy",
            Shop = 'm',
            Worth = 1000,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/Boss_1/Boss_item_1.png"))
            }
        };
        public readonly static Item Boss_1_drop_2 = new Item
        {
            Name = "Giant Iron Helmet",
            Description = "Giant iron Helmet imbued with magic, you can still feel electricity coursing  through it",
            Shop = 'm',
            Worth = 1800,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/Boss_1/Boss_item_2.png"))
            }
        };
        public readonly static Item Boss_1_drop_3 = new Item
        {
            Name = "Piece of armor",
            Description = "A small piece of armor made of expensive material",
            Shop = 'm',
            Worth = 150,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/Boss_1/Boss_item_3.png"))
            }
        };


        #endregion

        #region OneTimeUpgrades

        public readonly static Item HealthPotion = new Item
        {
            Name = "Health Potion",
            Description = "When used restores 1 health",
            Shop = 'w',
            Worth = 5,
            WorthItem = SlimeDrop,
            isUsable = true,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_health_potion.png"))
            }
        };

        public readonly static Item ShieldPotion = new Item
        {
            Name = "Shield Potion",
            Description = "When used gives you a shield that blocks one attack",
            Shop = 'w',
            Worth = 5,
            WorthItem = GolemDrop,
            isUsable = true,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_shield_potion.png"))
            }
        };
        public readonly static Item RagePotion = new Item
        {
            Name = "Rage Potion",
            Description = "When used increases your attack speed for 10s",
            Shop = 'w',
            Worth = 5,
            WorthItem = FireSpiritDrop,
            isUsable = true,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Rage_potion.png"))
            }
        };

        public readonly static Item Gun = new Item
        {
            Name = "Cannon",
            Description = "Shoots a cannonball that explodes on impact, dealing damage to all enemies in the area",
            Shop = 'b',
            Worth = 800,
            WorthItem = Coin,
            isUsable = true,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Canon.png"))
            }
        };


        public readonly static Item Crossbow = new Item
        {
            Name = "Crossbow",
            Description = "Shoots a three bolts in difrent directions",
            Shop = 'b',
            Worth = 500,
            WorthItem = Coin,
            isUsable = true,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Crossbow.png"))
            }
        };

        public readonly static Item EnchantedSword = new Item
        {
            Name = "Enchanted Scroll",
            Description = "Enchants your weapon for 10s, makes the projectiles go through walls and enemies",
            Shop = 'b',
            Worth = 800,
            WorthItem = Coin,
            isUsable = true,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Sword_enchant.png"))
            }
        };

        public readonly static List<Item> DroppableUpgrades = new List<Item>
        {
            Gun, Crossbow, EnchantedSword, RagePotion, ShieldPotion, HealthPotion
        };

        #endregion

        #region PermamentUpgrades
        public readonly static Item Armor_1 = new Item
        {
            Name = "Armor Upgrade",
            Description = "Upgrades your armor allowing you to take 1 more damage",
            Shop = 'b',
            Worth = 300,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Armor.png"))
            }
        };
        public readonly static Item Armor_2 = new Item
        {
            Name = "Armor Upgrade",
            Description = "Upgrades your armor allowing you to take 1 more damage",
            Shop = 'b',
            Worth = 1000,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Armor.png"))
            }
        };
        public readonly static Item Armor_3 = new Item
        {
            Name = "Armor Upgrade",
            Description = "Upgrades your armor allowing you to take 1 more damage",
            Shop = 'b',
            Worth = 5,
            WorthItem = Boss_1_drop_3,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Armor.png"))
            }
        };
        public readonly static Item Barrier_1 = new Item
        {
            Name = "Barrier Upgrade",
            Description = "upgrades your barrier to hay",
            Shop = 'b',
            Worth = 100,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Barrier_1.png"))
            }
        };
        public readonly static Item Barrier_2 = new Item
        {
            Name = "Barrier Upgrade",
            Description = "upgrades your barrier to crates",
            Shop = 'b',
            Worth = 800,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Barrier_2.png"))
            }
        };
        public readonly static Item Barrier_3 = new Item
        {
            Name = "Barrier Upgrade",
            Description = "upgrades your barrier to bricks",
            Shop = 'b',
            Worth = 1500,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Barrier_3.png"))
            }
        };
        public readonly static Item Shadow_potion_1 = new Item
        {
            Name = "Shadow Potion",
            Description = "gives you +1% chance to nullify damage when hit",
            Shop = 'w',
            Worth = 5,
            WorthItem = SkeletonDrop,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Shadow_potion.png"))
            }
        };
        public readonly static Item Shadow_potion_2 = new Item
        {
            Name = "Shadow Potion",
            Description = "gives you +1% chance to nullify damage when hit",
            Shop = 'w',
            Worth = 10,
            WorthItem = SkeletonDrop,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Shadow_potion.png"))
            }
        };
        public readonly static Item Shadow_potion_3 = new Item
        {
            Name = "Shadow Potion",
            Description = "gives you +1% chance to nullify damage when hit",
            Shop = 'w',
            Worth = 15,
            WorthItem = SkeletonDrop,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Shadow_potion.png"))
            }
        };
        public readonly static Item Shadow_potion_4 = new Item
        {
            Name = "Shadow Potion",
            Description = "gives you +1% chance to nullify damage when hit",
            Shop = 'w',
            Worth = 1000,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Shadow_potion.png"))
            }
        };
        public readonly static Item Shadow_potion_5 = new Item
        {
            Name = "Shadow Potion",
            Description = "gives you +1% chance to nullify damage when hit",
            Shop = 'w',
            Worth = 1300,
            WorthItem = Coin,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Shadow_potion.png"))
            }
        };
        public readonly static Item Luck_potion_1 = new Item
        {
            Name = "Luck Potion",
            Description = "gives you +5% chance for enemies to drop items when killed",
            Shop = 'w',
            Worth = 5,
            WorthItem = SpiderDrop,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Luck_potion.png"))
            }
        };
        public readonly static Item Luck_potion_2 = new Item
        {
            Name = "Luck Potion",
            Description = "gives you +5% chance for enemies to drop items when killed",
            Shop = 'w',
            Worth = 10,
            WorthItem = SpiderDrop,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Luck_potion.png"))
            }
        };
        public readonly static Item Luck_potion_3 = new Item
        {
            Name = "Luck Potion",
            Description = "gives you +5% chance for enemies to drop items when killed",
            Shop = 'w',
            Worth = 15,
            WorthItem = SpiderDrop,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Luck_potion.png"))
            }
        };
        public readonly static Item Luck_potion_4 = new Item
        {
            Name = "Luck Potion",
            Description = "gives you +5% chance for enemies to drop items when killed",
            Shop = 'w',
            Worth = 20,
            WorthItem = SpiderDrop,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Luck_potion.png"))
            }
        };
        public readonly static Item Luck_potion_5 = new Item
        {
            Name = "Luck Potion",
            Description = "gives you +5% chance for enemies to drop items when killed",
            Shop = 'w',
            Worth = 25,
            WorthItem = SpiderDrop,
            isUsable = false,
            Sprite = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/Images/misc/Upgrade_Luck_potion.png"))
            }
        };
        #endregion

    }
}
