﻿using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SpaceInvading.Resources.Classes
{
    public class Player
    {
        #region Variables
        //Village interaction
        public List<Item> Drops { get; set; }
        public List<Item> Upgrades { get; set; }
        public int Money { get; set; }

        //Basics
        public string Name { get; set; }
        public int Score { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public double PlayerSpeed { get; set; }

        //Current Player Look
        private Image _playerState;
        public Image PlayerState { get { return _playerState; } }

        private int playerSpriteNumber = 1;
        private int playerAttackSprite = 0;

        private bool _playerLeft = false;
        private bool _playerRight = false;

        public bool PlayerLeft { get { return _playerLeft; } }
        public bool PlayerRight { get { return _playerRight; } }
        public bool PlayerSpriteTurnedRight { get; set; }

        //Animations
        DispatcherTimer playerAttackAnimation = new DispatcherTimer();
        DispatcherTimer playerWalkAnimation = new DispatcherTimer();

        public bool IsAttacking { get { return playerAttackAnimation.IsEnabled; } }

        #endregion

        public Player()
        {
            Drops = new List<Item>();
            Upgrades = new List<Item>();
            Name = "Player";
            Health = MaxHealth = 2;
            _playerState = new Image
            {
                Width = 100,
                Height = 100,
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Player/player_still.png"))
            };

            PlayerSpeed = 2.5;

            playerAttackAnimation.Tick += AttackAnimation;
            playerAttackAnimation.Interval = new TimeSpan(0, 0, 0, 0, 80);

            playerWalkAnimation.Tick += WalkAnimation;
            playerWalkAnimation.Interval = new TimeSpan(0, 0, 0, 0, 500);
        }

        private void AttackAnimation(object? sender, EventArgs e)
        {
            if (playerAttackSprite < 4) playerAttackSprite++;
            // koniec klatek - koniec animacji, ustaw domyslny wyglad
            else
            {
                playerAttackSprite = 1;

                //jezeli gracz rusza sie w jedna z dwoch stron
                if (PlayerLeft != PlayerRight)
                    Walk();
                else
                    PlayerState.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Player/player_still.png"));

                playerAttackAnimation.Stop();
            }
            PlayerState.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Player/Player_attack_" + playerAttackSprite.ToString() + ".png"));
        }

        private void WalkAnimation(object? sender, EventArgs e)
        {
            if (playerSpriteNumber < 4) playerSpriteNumber++;
            else playerSpriteNumber = 1;
            PlayerState.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Player/Player_side_" + playerSpriteNumber.ToString() + ".png"));
        }

        public void TurnLeft(bool left)
        {
            _playerLeft = left;
            if (left)
            {
                PlayerState.RenderTransform = new ScaleTransform(-1, 1);
                PlayerSpriteTurnedRight = false;
            }
        }

        public void TurnRight(bool right)
        {
            _playerRight = right;
            if (right)
            {
                PlayerState.RenderTransform = new ScaleTransform(1, 1);
                PlayerSpriteTurnedRight = true;
            }
            }

        public void Attack()
        {
            playerAttackAnimation.Start();
            playerWalkAnimation.Stop();
        }

        public void Walk()
        {
            if (playerWalkAnimation.IsEnabled) return;

            playerAttackAnimation.Stop();
            playerWalkAnimation.Start();

            WalkAnimation(null, new EventArgs());
        }

        public void Stay()
        {
            playerWalkAnimation.Stop();
            playerAttackAnimation.Stop();
            PlayerState.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Player/player_still.png"));
        }
    }
}
