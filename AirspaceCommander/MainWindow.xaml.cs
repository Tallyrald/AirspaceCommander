//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Tallyrald">
// Copyright (c) Tallyrald @ Github
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Airspace_Commander
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Random generator
        /// </summary>
        private static Random r = new Random();

        /// <summary>
        /// Stores the canvas
        /// </summary>
        private static Grid grid;

        /// <summary>
        /// Stores the viewmodel
        /// </summary>
        private ViewModel vm;

        /// <summary>
        /// Stores the game manager
        /// </summary>
        private GameManager gm;

        /// <summary>
        /// Indicates whether cheating is enabled
        /// </summary>
        private bool cheat = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the grid to house the game area
        /// </summary>
        public static Grid Grid
        {
            get { return grid; }
            set { grid = value; }
        }

        /// <summary>
        /// Gets a random number generator
        /// </summary>
        public static Random R
        {
            get { return r; }
        }

        /// <summary>
        /// Executes as soon as the window has loaded
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">Additional parameters</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create VM
            vm = new ViewModel();
            DataContext = vm;

            Grid = Content as Grid;

            // Create new GM
            gm = new GameManager(Grid.ActualWidth * 0.8, Grid.ActualHeight, vm);

            // Generate buildings
            foreach (Building building in gm.GenerateBuildings(4))
            {
                vm.Buildings.Add(building);
            }

            // Add basic variants for engines and warheads and select them
            vm.Engines.Add(new SmallEngine("Small", 0, 0));
            vm.SelectedEngine = vm.Engines.Last();
            vm.Warheads.Add(new SmallWarhead("Small", 0, 0));
            vm.SelectedWarhead = vm.Warheads.Last();

            // Game tick timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(40);
            timer.Tick += Timer_Tick;
            timer.Start();

            // One second timer for nuclear rockets
            DispatcherTimer nTimer = new DispatcherTimer();
            nTimer.Interval = TimeSpan.FromMilliseconds(1000);
            nTimer.Tick += NTimer_Tick;
            nTimer.Start();
        }

        /// <summary>
        /// Nuclear rocket usage countdown
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void NTimer_Tick(object sender, EventArgs e)
        {
            if (vm.NuclearTimer > 0)
            {
                vm.NuclearTimer--;
            }
        }

        /// <summary>
        /// Executes on every tick fired by the timer
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            gm.GmTick();

            if (R.Next(0, 1000) <= 20)
            {
                int next = R.Next(0, 4);
                if (next == 0)
                {
                    vm.Movables.Add(gm.GenerateRocket(false));
                }
                else if (next == 1)
                {
                    if (R.Next(0, 2) == 1)
                    {
                        vm.Movables.Add(gm.GenerateMeteor());
                    }
                    else
                    {
                        vm.Movables.Add(gm.GenerateMeteor(true));
                    }
                }
                else if (next == 2)
                {
                    vm.Movables.Add(gm.GenerateUfo());
                }
                else if (next == 3)
                {
                    vm.Movables.Add(gm.GenerateJet());
                }
            }

            gm.MoveElements();
            gm.CheckExplosions();
            if (!gm.CheckBuildings())
            {
                MessageBox.Show("All buildings have been destroyed! \n You scored " + vm.Score + ". \n\n Try to top it next time!");
                Close();
            }
            else
            {
                vm.DoDraw();
            }
        }

        /// <summary>
        /// Fires a rocket to where the user clicked
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The mouse</param>
        private void FireRocket_Click(object sender, MouseButtonEventArgs e)
        {
            if (cheat || vm.RemainingRockets > 0)
            {
                if (cheat || (vm.SelectedWarhead is NuclearWarhead && vm.NuclearTimer == 0) || !(vm.SelectedWarhead is NuclearWarhead))
                {
                    if (!cheat && vm.SelectedWarhead is NuclearWarhead)
                    {
                        vm.NuclearTimer = 60;
                    }

                    vm.Movables.Add(gm.GenerateRocket(true, e.GetPosition(Grid).X, e.GetPosition(Grid).Y));
                    vm.RemainingRockets--;
                }
            }
        }

        /// <summary>
        /// Handles the click event on the warhead research button
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void EngineResearch_Click(object sender, RoutedEventArgs e)
        {
            switch (vm.Engines.Count)
            {
                case 1:
                    if (cheat || vm.Score >= 50)
                    {
                        vm.Engines.Add(new MediumEngine("Medium", 0, 0));
                        gm.Score(-50);
                    }

                    break;
                case 2:
                    if (cheat || vm.Score >= 80)
                    {
                        vm.Engines.Add(new LargeEngine("Large", 0, 0));
                        gm.Score(-80);
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the click event on the warhead research button
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void WarheadResearch_Click(object sender, RoutedEventArgs e)
        {
            switch (vm.Warheads.Count)
            {
                case 1:
                    if (cheat || vm.Score >= 25)
                    {
                        vm.Warheads.Add(new MediumWarhead("Medium", 0, 0));
                        gm.Score(-25);
                    }

                    break;
                case 2:
                    if (cheat || vm.Score >= 75)
                    {
                        vm.Warheads.Add(new LargeWarhead("Large", 0, 0));
                        gm.Score(-75);
                    }

                    break;
                case 3:
                    if (cheat || vm.Score >= 150)
                    {
                        vm.Warheads.Add(new NuclearWarhead("Nuclear", 0, 0));
                        gm.Score(-150);
                    }

                    break;
            }
        }

        /// <summary>
        /// Handles the click event on the add shield button
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void AddShield_Click(object sender, RoutedEventArgs e)
        {
            if (cheat || vm.Score >= 25)
            {
                foreach (Building building in vm.Buildings)
                {
                    building.ActivateShield(gm.GenerateShield());
                }

                gm.Score(-25);
            }
        }

        /// <summary>
        /// Handles the click event on the renovate buildings button
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Renovate_Click(object sender, RoutedEventArgs e)
        {
            if (cheat || vm.Score >= 70)
            {
                foreach (Building building in vm.Buildings)
                {
                    building.Renovate();
                }

                gm.Score(-70);
            }
        }

        /// <summary>
        /// Handles the click event on the modernize buildings button
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void Modernize_Click(object sender, RoutedEventArgs e)
        {
            if (cheat || vm.Score >= 130)
            {
                foreach (Building building in vm.Buildings)
                {
                    building.Modernize();
                }

                gm.Score(-130);
            }
        }

        /// <summary>
        /// Handles the click event on the modernize buildings button
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        private void UpgradeShield_Click(object sender, RoutedEventArgs e)
        {
            if (cheat || vm.Score >= 100)
            {
                vm.ShieldResearch++;
                gm.Score(-100);
            }
        }

        /// <summary>
        /// Handles KeyDown event
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The key pressed</param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.K)
            {
                cheat = !cheat;
            }
        }
    }
}
