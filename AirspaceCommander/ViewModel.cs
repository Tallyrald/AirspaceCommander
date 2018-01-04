//-----------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="Tallyrald">
// Copyright (c) Tallyrald @ Github
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Airspace_Commander
{
    /// <summary>
    /// The ViewModel handles data exchange between the UI and the business logic
    /// </summary>
    public class ViewModel : Bindable
    {
        /// <summary>
        /// Contains the current score of the player
        /// </summary>
        private int score = 0;

        /// <summary>
        /// Contains the player's remaining rocket he/she can still fire
        /// </summary>
        private int remainingRockets = 30;

        /// <summary>
        /// Contains all buildings in the game
        /// </summary>
        private BindingList<Building> buildings;

        /// <summary>
        /// Contains the currently existing explosions
        /// </summary>
        private BindingList<Explosion> explosions;

        /// <summary>
        /// Contains the currently existing movable objects
        /// </summary>
        private BindingList<IMovable> movables;

        /// <summary>
        /// Contains the currently available engines
        /// </summary>
        private BindingList<IEngine> engines;

        /// <summary>
        /// Contains the currently available warheads
        /// </summary>
        private BindingList<IWarhead> warheads;

        /// <summary>
        /// The selected (by player) engine
        /// </summary>
        private IEngine selectedEngine;

        /// <summary>
        /// The selected (by player) warhead
        /// </summary>
        private IWarhead selectedWarhead;

        /// <summary>
        /// Contains the seconds until a nuclear rocket can be fired again
        /// </summary>
        private int nuclearTimer;

        /// <summary>
        /// Stores the current shield research boost
        /// </summary>
        private int shieldResearch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Airspace_Commander.ViewModel" /> class.
        /// </summary>
        public ViewModel()
        {
            buildings = new BindingList<Building>();
            Explosions = new BindingList<Explosion>();
            movables = new BindingList<IMovable>();
            engines = new BindingList<IEngine>();
            warheads = new BindingList<IWarhead>();
            nuclearTimer = 0;
            shieldResearch = 1;
        }

        /************
         * Properties
         ***********/

        /// <summary>
        /// Gets the current instance of the ViewModel
        /// </summary>
        public ViewModel Self
        {
            get
            {
                OnPropertyChanged("NuclearColor");
                OnPropertyChanged("MediumEngineResearch");
                OnPropertyChanged("LargeEngineResearch");
                OnPropertyChanged("MediumWarheadResearch");
                OnPropertyChanged("LargeWarheadResearch");
                OnPropertyChanged("NuclearWarheadResearch");
                return this;
            }
        }

        /// <summary>
        /// Gets or sets a BindingList containing all buildings
        /// </summary>
        public BindingList<Building> Buildings
        {
            get
            {
                return buildings;
            }

            set
            {
                SetProperty(ref buildings, value);
                OnPropertyChanged("Self");
            }
        }

        /// <summary>
        /// Gets or sets a BindingList containing all explosions
        /// </summary>
        public BindingList<Explosion> Explosions
        {
            get
            {
                return explosions;
            }

            set
            {
                SetProperty(ref explosions, value);
                OnPropertyChanged("Self");
            }
        }

        /// <summary>
        /// Gets or sets a BindingList containing all movable objects
        /// </summary>
        public BindingList<IMovable> Movables
        {
            get
            {
                return movables;
            }

            set
            {
                SetProperty(ref movables, value);
                OnPropertyChanged("Self");
            }
        }

        /// <summary>
        /// Gets or sets a BindingList containing all available engines
        /// </summary>
        public BindingList<IEngine> Engines
        {
            get
            {
                return engines;
            }

            set
            {
                SetProperty(ref engines, value);
                OnPropertyChanged("Self");
            }
        }

        /// <summary>
        /// Gets or sets a BindingList containing all available warheads
        /// </summary>
        public BindingList<IWarhead> Warheads
        {
            get
            {
                return warheads;
            }

            set
            {
                SetProperty(ref warheads, value);
                OnPropertyChanged("Self");
            }
        }

        /// <summary>
        /// Gets or sets the selected engine
        /// </summary>
        public IEngine SelectedEngine
        {
            get
            {
                return selectedEngine;
            }

            set
            {
                SetProperty(ref selectedEngine, value);
                OnPropertyChanged("Self");
            }
        } 

        /// <summary>
        /// Gets or sets the selected warhead
        /// </summary>
        public IWarhead SelectedWarhead
        {
            get
            {
                return selectedWarhead;
            }

            set
            {
                SetProperty(ref selectedWarhead, value);
                OnPropertyChanged("Self");
            }
        }

        /// <summary>
        /// Gets or sets the current score
        /// </summary>
        public int Score
        {
            get
            {
                return score;
            }

            set
            {
                SetProperty(ref score, value);
                OnPropertyChanged("Self");
            }
        }

        /// <summary>
        /// Gets or sets the currently available rocket number
        /// </summary>
        public int RemainingRockets
        {
            get
            {
                return remainingRockets;
            }

            set
            {
                SetProperty(ref remainingRockets, value);
                OnPropertyChanged("Self");
            }
        }

        /// <summary>
        /// Gets or sets the nuclear launch countdown timer
        /// </summary>
        public int NuclearTimer
        {
            get
            {
                return nuclearTimer;
            }

            set
            {
                SetProperty(ref nuclearTimer, value);
                OnPropertyChanged("Self");
            }
        }

        /// <summary>
        /// Gets the color for the nuclear launch countdown text
        /// </summary>
        public string NuclearColor
        {
            get
            {
                if (Warheads.Count <= 3)
                {
                    return "Black";
                }
                else
                {
                    if (NuclearTimer == 0)
                    {
                        return "Green";
                    }
                    else
                    {
                        return "Red";
                    }
                }
            }
        }

        /// <summary>
        /// Gets the color for the Medium Engine research status indicator
        /// </summary>
        public string MediumEngineResearch
        {
            get
            {
                if (Engines.Count == 1)
                {
                    return "Red";
                }
                else
                {
                    return "Green";
                }
            }
        }

        /// <summary>
        /// Gets the color for the Large Engine research status indicator
        /// </summary>
        public string LargeEngineResearch
        {
            get
            {
                if (Engines.Count <= 2)
                {
                    return "Red";
                }
                else
                {
                    return "Green";
                }
            }
        }

        /// <summary>
        /// Gets the color for the Medium Warhead research status indicator
        /// </summary>
        public string MediumWarheadResearch
        {
            get
            {
                if (Warheads.Count <= 1)
                {
                    return "Red";
                }
                else
                {
                    return "Green";
                }
            }
        }

        /// <summary>
        /// Gets the color for the Large Warhead research status indicator text
        /// </summary>
        public string LargeWarheadResearch
        {
            get
            {
                if (Warheads.Count <= 2)
                {
                    return "Red";
                }
                else
                {
                    return "Green";
                }
            }
        }

        /// <summary>
        /// Gets the color for the Nuclear Warhead research status indicator text
        /// </summary>
        public string NuclearWarheadResearch
        {
            get
            {
                if (Warheads.Count <= 3)
                {
                    return "Red";
                }
                else
                {
                    return "Green";
                }
            }
        }

        /// <summary>
        /// Gets or sets the shield research value
        /// </summary>
        public int ShieldResearch
        {
            get
            {
                return shieldResearch;
            }

            set
            {
                SetProperty(ref shieldResearch, value);
                OnPropertyChanged("Self");
            }
        }

        /// <summary>
        /// Gets the brush for buildings
        /// </summary>
        /// <param name="x">The coordinate on the X axis</param>
        /// <param name="y">The coordinate on the Y axis</param>
        /// <returns>Returns the brush for a building</returns>
        public Brush GetBuildingBrush(double x, double y)
        {
            return GetBrush("_building.png", x, y, Building.W, Building.H);
        }

        /// <summary>
        /// Gets the brush for shields
        /// </summary>
        /// <param name="x">The coordinate on the X axis</param>
        /// <param name="y">The coordinate on the Y axis</param>
        /// <returns>Returns the brush for a building</returns>
        public Brush GetShieldBrush(double x, double y)
        {
            return GetBrush("_shield.png", x, y, Shield.W, Shield.H);
        }

        /// <summary>
        /// Gets the Brush for meteors
        /// </summary>
        /// <param name="x">The coordinate on the X axis</param>
        /// <param name="y">The coordinate on the Y axis</param>
        /// <returns>Returns the brush for a building</returns>
        public Brush GetMeteorBrush(double x, double y)
        {
            return GetBrush("_meteor.png", x, y, Meteor.W, Meteor.H);
        }

        /// <summary>
        /// Gets the brush for rockets
        /// </summary>
        /// <param name="x">The coordinate on the X axis</param>
        /// <param name="y">The coordinate on the Y axis</param>
        /// <param name="angle">The angle by which the texture should be rotated</param>
        /// <returns>Returns the brush for a rocket</returns>
        public Brush GetRocketBrush(double x, double y, int angle)
        {
            Brush b = GetBrush("_rocket.png", x, y, Rocket.W, Rocket.H);
            TransformGroup tg = new TransformGroup();
            tg.Children.Add(new RotateTransform(angle, x + (Rocket.W / 2), y + (Rocket.H / 2)));
            b.Transform = tg;
            return b;
        }

        /// <summary>
        /// Gets the brush for UFOs
        /// </summary>
        /// <param name="x">The coordinate on the X axis</param>
        /// <param name="y">The coordinate on the Y axis</param>
        /// <returns>Returns the brush for a UFO</returns>
        public Brush GetUfoBrush(double x, double y)
        {
            return GetBrush("_ufo.png", x, y, Ufo.W, Ufo.H);
        }

        /// <summary>
        /// Gets the brush for Jets
        /// </summary>
        /// <param name="x">The coordinate on the X axis</param>
        /// <param name="y">The coordinate on the Y axis</param>
        /// <param name="angle">The angle by which the texture should be rotated</param>
        /// <returns>Returns the brush for a Jet</returns>
        public Brush GetJetBrush(double x, double y, int angle)
        {
            Brush b = GetBrush("_jet.png", x, y, Jet.W, Jet.H);
            TransformGroup tg = new TransformGroup();
            tg.Children.Add(new RotateTransform(angle, x + (Jet.W / 2), y + (Jet.H / 2)));
            b.Transform = tg;
            return b;
        }

        /// <summary>
        /// Draws the current state on the play area
        /// </summary>
        public void DoDraw()
        {
            OnPropertyChanged("Self");
        }

        /// <summary>
        /// Creates an ImageBrush
        /// </summary>
        /// <param name="fname">The file path and name where the image is located</param>
        /// <param name="x">X coordinate for ViewPort</param>
        /// <param name="y">Y coordinate for ViewPort</param>
        /// <param name="w">Width for ViewPort</param>
        /// <param name="h">Height for ViewPort</param>
        /// <returns>Returns an ImageBrush that has the desired texture</returns>
        private Brush GetBrush(string fname, double x, double y, int w, int h)
        {
            ImageBrush ib = new ImageBrush(new BitmapImage(new Uri(fname, UriKind.Relative)));
            
            ib.TileMode = TileMode.None;
            ib.Viewport = new Rect(x, y, w, h);
            ib.ViewportUnits = BrushMappingMode.Absolute;
            return ib;
        }
    }
}
