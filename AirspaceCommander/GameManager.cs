//-----------------------------------------------------------------------
// <copyright file="GameManager.cs" company="Tallyrald">
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
using System.Windows.Controls;

namespace Airspace_Commander
{
    /// <summary>
    /// This class manages all in-game elements
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// Stores the ViewModel reference
        /// </summary>
        private ViewModel vm;

        /// <summary>
        /// Stores the actual canvas width and height
        /// </summary>
        private double cw, ch;

        /// <summary>
        /// Rocket queue which contains rockets that will be inserted in the ViewModel on the next tick
        /// </summary>
        private Queue<Rocket> rocketsToInsert;

        /// <summary>
        /// RExplosion queue which contains explosions that will be inserted in the ViewModel on the next tick
        /// </summary>
        private Queue<Explosion> explosionsToInsert;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        /// <param name="w">The play area width</param>
        /// /// <param name="h">The play area height</param>
        /// <param name="newVM">The ViewModel to use</param>
        public GameManager(double w, double h, ViewModel newVM)
        {
            vm = newVM;
            cw = w;
            ch = h;
            rocketsToInsert = new Queue<Rocket>();
            explosionsToInsert = new Queue<Explosion>();
        }

        /// <summary>
        /// Gets the actual canvas width
        /// </summary>
        public double Cw
        {
            get { return cw; }
        }

        /// <summary>
        /// Gets the actual canvas height
        /// </summary>
        public double Ch
        {
            get { return ch; }
        }

        /// <summary>
        /// Checks if two game objects intersect with each other
        /// </summary>
        /// <param name="first">A moving game object</param>
        /// <param name="second">Any game object</param>
        /// <returns>returns true if the two intersect, false otherwise</returns>
        public static bool Intersects(IMovable first, object second)
        {
            if (second is Building)
            {
                Building building = (Building)second;
                double widthFirst = (double)(int)first.GetType().GetField("W").GetValue(null);
                double heightFirst = (double)(int)first.GetType().GetField("H").GetValue(null);

                Rect firstRect = new Rect(first.Position.X - (widthFirst / 2), first.Position.Y - (heightFirst / 2), widthFirst, heightFirst);
                Rect buildingRect = new Rect(building.Position.X, building.Position.Y, Building.W, Building.H);
                return firstRect.IntersectsWith(buildingRect);
            }
            else if (second is Explosion)
            {
                Explosion explosion = (Explosion)second;
                Rect rect = new Rect(first.Position.X, first.Position.Y, (double)(int)first.GetType().GetField("W").GetValue(null), (double)(int)first.GetType().GetField("H").GetValue(null));
                double circleDistanceX = Math.Abs(explosion.Position.X - rect.X);
                double circleDistanceY = Math.Abs(explosion.Position.Y - rect.Y);

                if (circleDistanceX > ((rect.Width / 2) + explosion.Radius))
                {
                    return false;
                }

                if (circleDistanceY > ((rect.Height / 2) + explosion.Radius))
                {
                    return false;
                }

                if (circleDistanceX <= (rect.Width / 2))
                {
                    return true;
                }

                if (circleDistanceY <= (rect.Height / 2))
                {
                    return true;
                }

                double cornerDistance_sq = ((circleDistanceX - (rect.Width / 2)) * (circleDistanceX - (rect.Width / 2))) + ((circleDistanceY - (rect.Height / 2)) * (circleDistanceY - (rect.Height / 2)));

                return cornerDistance_sq <= explosion.Radius * explosion.Radius;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Generates the logic for buildings
        /// </summary>
        /// <param name="num">The amount to generate</param>
        /// <returns>Returns a list which has all the newly generated building logics</returns>
        public BindingList<Building> GenerateBuildings(int num)
        {
            BindingList<Building> newbuildings = new BindingList<Building>();
            int[] x = new int[num];
            for (int i = 0; i < num; i++)
            {
                bool end = false;
                Point coords;
                do
                {
                    coords = new Point(MainWindow.R.Next(1, (int)Math.Round(cw - Building.W, 0)), ch - Building.H);
                    int j;
                    int noCollision = 0;
                    for (j = 0; j < newbuildings.Count; j++)
                    {
                        if (!(coords.X > newbuildings[j].Position.X && coords.X < newbuildings[j].Position.X + Building.W) &&
                            !(newbuildings[j].Position.X > coords.X && newbuildings[j].Position.X < coords.X + Building.W))
                        {
                            noCollision++;
                        }
                    }

                    if (j == 0 || noCollision == newbuildings.Count)
                    {
                        end = true;
                    }
                }
                while (!end);
                
                newbuildings.Add(new Building(MainWindow.R.Next(3, 6), coords));
            }

            return newbuildings;
        }

        /// <summary>
        /// Generate the logic for a rocket
        /// </summary>
        /// <param name="is_friendly">States if the rocket was fired by the player</param>
        /// <param name="clickX">The X coordinate of the mouse click</param>
        /// <param name="clickY">The Y coordinate of the mouse click</param>
        /// <returns>Returns a rocket instance</returns>
        public Rocket GenerateRocket(bool is_friendly, double clickX = 0, double clickY = 0)
        {
            IEngine e = GenerateEngine(is_friendly);
            IWarhead w = GenerateWarhead(is_friendly);
            Rocket rocket;
            if (is_friendly)
            {
                Point start = new Point(Cw / 2, Ch - (Rocket.H / 2));
                rocket = new Rocket(e, w, start, clickX, clickY, is_friendly);
            }
            else
            {
                Point start;
                if (clickX == 0 && clickY == 0)
                {
                    start = new Point(MainWindow.R.Next(1, (int)Math.Round(Cw - 1)), Rocket.H / 2);
                }
                else
                {
                    start = new Point(clickX, clickY);
                }

                int targetX = MainWindow.R.Next(1, (int)Math.Round(Cw - Rocket.W));
                int targetY = (int)Math.Round(Ch - Rocket.H);
                rocket = new Rocket(e, w, start, targetX, targetY, is_friendly);

                if (clickX != 0 || clickY != 0)
                {
                    rocketsToInsert.Enqueue(rocket);
                }
            }

            return rocket;
        }

        /// <summary>
        /// Generates a new meteor
        /// </summary>
        /// <param name="ultraSolid">False for 1 HP, true for 2 or 3</param>
        /// <returns>Returns the Meteor generated</returns>
        public Meteor GenerateMeteor(bool ultraSolid = false)
        {
            Meteor meteor;
            Point start = new Point(MainWindow.R.Next(1, (int)Math.Round(Cw - Meteor.W)), 1);
            int hp = 1;
            if (ultraSolid)
            {
                hp = 2;
            }

            meteor = new Meteor(start, ch - Meteor.H, hp);

            return meteor;
        }

        /// <summary>
        /// Generates a new UFO
        /// </summary>
        /// <returns>Returns the UFO generated</returns>
        public Ufo GenerateUfo()
        {
            Ufo ufo;
            int startX = 1;
            int targetX = (int)Math.Round(Cw - 2, 0);
            int hp = MainWindow.R.Next(2, 4);

            if (MainWindow.R.Next(0, 2) == 1)
            {
                startX = targetX;
                targetX = 2;
            }

            Point start = new Point(startX, MainWindow.R.Next(1, (int)Math.Round(Ch * 0.2, 0)));
            ufo = new Ufo(start, targetX, hp, GenerateEngine(false));

            return ufo;
        }

        /// <summary>
        /// Generates a new Jet
        /// </summary>
        /// <returns>Returns the Jet generated</returns>
        public Jet GenerateJet()
        {
            Jet jet;
            int startX = 1;
            int targetX = (int)Math.Round(Cw - 2, 0);

            if (MainWindow.R.Next(0, 2) == 1)
            {
                startX = targetX;
                targetX = 2;
            }

            Point start = new Point(startX, MainWindow.R.Next(1, (int)Math.Round(Ch * 0.2, 0)));
            jet = new Jet(start, targetX, GenerateEngine(false));

            return jet;
        }

        /// <summary>
        /// Generates an explosion and adds it directly to the ViewModel
        /// </summary>
        /// <param name="coords">The position the explosion should be spawned at</param>
        /// <param name="radius">The radius the explosion should have</param>
        /// <param name="damage">The damage the explosion should cause</param>
        public void GenerateExplosion(Point coords, int radius, int damage)
        {
            if (radius > Cw)
            {
                // Nuclear
                explosionsToInsert.Enqueue(new Explosion(coords, radius, damage, 2));
            }
            else
            {
                // Check for out of play area
                if (coords.X - radius < 0)
                {
                    coords.X += Math.Abs(coords.X - radius - 2);
                }
                else if (coords.X + radius > Cw)
                {
                    coords.X -= coords.X + radius - Cw - 2;
                }

                if (coords.Y - radius < 0)
                {
                    coords.Y += Math.Abs(coords.Y - radius - 2);
                }
                else if (coords.Y + radius > Ch)
                {
                    coords.Y -= coords.Y + radius - Ch - 2;
                }

                explosionsToInsert.Enqueue(new Explosion(coords, radius, damage, 80));
            }
        }

        /// <summary>
        /// Generates a new Shield
        /// </summary>
        /// <returns>Returns the Shield generated</returns>
        public Shield GenerateShield()
        {
            Shield shield = new Shield(vm.ShieldResearch);
            return shield;
        }

        /// <summary>
        /// Handles the queues
        /// </summary>
        public void GmTick()
        {
            for (int i = 0; i < rocketsToInsert.Count; i++)
            {
                vm.Movables.Add(rocketsToInsert.Dequeue());
            }

            for (int i = 0; i < explosionsToInsert.Count; i++)
            {
                vm.Explosions.Add(explosionsToInsert.Dequeue());
            }
        }

        /// <summary>
        /// Adds 1 to the Score
        /// </summary>
        /// <param name="amount">The score to add</param>
        public void Score(int amount = 1)
        {
            vm.Score += amount;
            if (vm.Score % 10 == 0)
            {
                vm.RemainingRockets += 10;
            }
        }

        /// <summary>
        /// Moves IMovable elements
        /// </summary>
        public void MoveElements()
        {
            List<IMovable> removableMovables = new List<IMovable>();

            foreach (IMovable akt in vm.Movables)
            {
                for (int i = 0; i < vm.Explosions.Count; i++)
                {
                    if (Intersects(akt, vm.Explosions[i]))
                    {
                        if (akt.GetHit(vm.Explosions[i]))
                        {
                            if (akt is Rocket)
                            {
                                Rocket temp = (Rocket)akt;
                                if (!temp.IsFriendly)
                                {
                                    // This is definitely an enemy rocket -> score!
                                    Score();
                                }

                                temp.Explode(this);
                            }
                            else
                            {
                                Score();
                            }

                            removableMovables.Add(akt);
                            break;
                        }
                    }
                }

                if (!removableMovables.Contains(akt) && !akt.Move(this, vm.Buildings))
                {
                    removableMovables.Add(akt);
                }
            }

            foreach (IMovable item in removableMovables)
            {
                vm.Movables.Remove(item);
            }
        }

        /// <summary>
        /// Checks if any explosions need removal
        /// </summary>
        public void CheckExplosions()
        {
            int counter = vm.Explosions.Count;
            List<Explosion> removableExplosions = new List<Explosion>();

            for (int i = 0; i < counter; i++)
            {
                vm.Explosions[i].Ttl--;
                if (vm.Explosions[i].Ttl <= 0)
                {
                    removableExplosions.Add(vm.Explosions[i]);
                }
            }

            foreach (Explosion item in removableExplosions)
            {
                vm.Explosions.Remove(item);
            }
        }

        /// <summary>
        /// Checks all buildings for removability
        /// </summary>
        /// <returns>Returns true if any building is still standing, false if all are destroyed</returns>
        public bool CheckBuildings()
        {
            List<Building> removableBuildings = new List<Building>();
            foreach (Building building in vm.Buildings)
            {
                if (!building.Is_OK)
                {
                    removableBuildings.Add(building);
                }
            }

            foreach (Building building in removableBuildings)
            {
                vm.Buildings.Remove(building);
            }

            if (vm.Buildings.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Generates an engine
        /// </summary>
        /// <param name="is_friendly">States if the engine is for a player rocket</param>
        /// <returns>Returns an engine instance</returns>
        private IEngine GenerateEngine(bool is_friendly)
        {
            if (is_friendly)
            {
                return vm.SelectedEngine.Clone();
            }
            else
            {
                IEngine engine = vm.Engines[MainWindow.R.Next(vm.Engines.Count)].Clone();

                // this a key for gameplay management
                engine.Speed *= 0.3;

                if (MainWindow.R.Next(0, 100) < 10)
                {
                    engine.SpeedRate = 2;
                }

                return engine;
            }
        }

        /// <summary>
        /// Generates a warhead
        /// </summary>
        /// <param name="is_friendly">States if the warhead is for a player rocket</param>
        /// <returns>Returns a warhead instance</returns>
        private IWarhead GenerateWarhead(bool is_friendly)
        {
            if (is_friendly)
            {
                return vm.SelectedWarhead;
            }
            else
            {
                return vm.Warheads[MainWindow.R.Next(vm.Warheads.Count)];
            }
        }
    }
}
