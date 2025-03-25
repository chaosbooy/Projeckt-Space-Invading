﻿using SpaceInvading.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpaceInvading.Resources.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Shop.xaml
    /// </summary>
    public partial class Shop : Page
    {
        public Shop()
        {
            InitializeComponent();
            CreateOffers();
        }

        private void BackToVillage(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Village());

        }

        private void CreateOffers()
        {
            for (int i = 0; i < 10; i++)
            {
            }
        }
    }
}