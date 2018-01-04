//-----------------------------------------------------------------------
// <copyright file="Rocket.cs" company="Tallyrald">
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

namespace Airspace_Commander
{
    /// <summary>
    /// Rocket business logic
    /// </summary>
    public class Rocket : Bindable, IMovable
    {
        /// <summary>
        /// The width of the Rocket
        /// </summary>
        public const int W = 30;

        /// <summary>
        /// The height of the Rocket
        /// </summary>
        public const int H = 30;

        /// <summary>
        /// Contains the position of the rocket
        /// </summary>
        private Point position;

        /// <summary>
        /// Contains the delta speed on the X axis
        /// </summary>
        private double dx = 0;

        /// <summary>
        /// Contains the delta speed on the Y axis
        /// </summary>
        private double dy = 0;

        /// <summary>
        /// Contains the target coordinate on the X axis
        /// </summary>
        private double startX;

        /// <summary>
        /// Contains the target coordinate on the Y axis
        /// </summary>
        private double startY;

        /// <summary>
        /// Contains the target coordinate on the X axis
        /// </summary>
        private double targetX;

        /// <summary>
        /// Contains the target coordinate on the Y axis
        /// </summary>
        private double targetY;

        /// <summary>
        /// States if this rocket is friendly
        /// </summary>
        private bool isFriendly;

        /// <summary>
        /// Indicates whether this rocket has been destroyed by an explosion
        /// </summary>
        private bool isDestroyed;

        /// <summary>
        /// Contains the engine
        /// </summary>
        private IEngine engine;

        /// <summary>
        /// Contains the warhead
        /// </summary>
        private IWarhead warhead;

        /// <summary>
        /// Initializes a new instance of the <see cref="Airspace_Commander.Rocket" /> class.
        /// </summary>
        /// <param name="newEngine">The engine this rocket will use</param>
        /// <param name="newWarhead">The warhead this rocket will use</param>
        /// <param name="coords">The coordinates this rocket is at</param>
        /// <param name="newTargetX">Target coordinate on the X axis</param>
        /// <param name="newTargetY">Target coordinate on the Y axis</param>
        /// <param name="newIsFriendly">The "friendlyness" of the rocket</param>
        public Rocket(IEngine newEngine, IWarhead newWarhead, Point coords, double newTargetX, double newTargetY, bool newIsFriendly)
        {
            engine = newEngine;
            warhead = newWarhead;
            position = coords;
            startX = position.X;
            startY = position.Y;
            targetX = newTargetX;
            targetY = newTargetY;
            isFriendly = newIsFriendly;

            double distanceX = (startX - targetX) * -1;
            double distanceY = (startY - targetY) * -1;
            double multiplier = 1 / (Math.Abs(distanceX) + Math.Abs(distanceY));
            dx = Engine.Speed * multiplier * distanceX;
            dy = Engine.Speed * multiplier * distanceY;
        }

        /// <summary>
        /// Gets the position of this rocket
        /// </summary>
        public Point Position
        {
            get { return position; }
        }

        /// <summary>
        /// Gets the target coordinate on the X axis
        /// </summary>
        public double TargetX
        {
            get { return targetX; }
        }

        /// <summary>
        /// Gets the target coordinate on the Y axis
        /// </summary>
        public double TargetY
        {
            get { return targetY; }
        }

        /// <summary>
        /// Gets the start coordinate on the X axis
        /// </summary>
        public double StartX
        {
            get { return startX; }
        }

        /// <summary>
        /// Gets the start coordinate on the Y axis
        /// </summary>
        public double StartY
        {
            get { return startY; }
        }

        /// <summary>
        /// Gets a value indicating whether the rocket is destroyed
        /// </summary>
        public bool IsDestroyed
        {
            get { return isDestroyed; }
        }

        /// <summary>
        /// Gets a value indicating whether the rocket is friendly
        /// </summary>
        public bool IsFriendly
        {
            get { return isFriendly; }
        }

        /// <summary>
        /// Gets the direction (display) of the rocket
        /// </summary>
        public int Angle
        {
            get
            {
                int angle = (int)Math.Round(180 * Math.Atan2(dy, dx) / Math.PI);
                return angle - 90;
            }
        }

        /// <summary>
        /// Gets the engine of the rocket
        /// </summary>
        public IEngine Engine
        {
            get { return engine; }
        }

        /// <summary>
        /// Gets the warhead installed on the rocket
        /// </summary>
        public IWarhead Warhead
        {
            get { return warhead; }
        }

        /*************
        *** Methods
        *************/

        /// <summary>
        /// Increases the rocket's delta speed defined by the engine's properties
        /// </summary>
        public void ChangeSpeed()
        {
            double distanceX = (startX - targetX) * -1;
            double distanceY = (startY - targetY) * -1;
            double multiplier = 1 / (Math.Abs(distanceX) + Math.Abs(distanceY));
            dx += Engine.SpeedRate / 100 * multiplier * distanceX;
            dy += Engine.SpeedRate / 100 * multiplier * distanceY;
        }

        /// <summary>
        /// Moves the rocket
        /// </summary>
        /// <param name="gm">The GameManager to use</param>
        /// <param name="buildings">A list containing all buildings</param>
        /// <returns>Returns true if the move was successful, false if the rocket went boom</returns>
        public bool Move(GameManager gm, BindingList<Building> buildings = null)
        {
            if (IsDestroyed)
            {
                Explode(gm);
                return false;
            }

            // if this rocket is an enemy, we need to check collision with buildings
            if (!IsFriendly)
            {
                foreach (Building building in buildings)
                {
                    if (GameManager.Intersects(this, building))
                    {
                        if (!IsFriendly && warhead is NuclearWarhead)
                        {
                            foreach (Building singleBuilding in buildings)
                            {
                                Explode(gm, singleBuilding);
                            }
                        }
                        else
                        {
                            Explode(gm, building);
                        }

                        return false;
                    }
                }
            }

            // First, we change the speed for this turn
            ChangeSpeed();

            double targetDistance = Math.Sqrt((Math.Abs(targetX - startX) * Math.Abs(targetX - startX)) + (Math.Abs(targetY - startY) * Math.Abs(targetY - startY)));
            double curDistance = Math.Sqrt((Math.Abs(position.X - startX) * Math.Abs(position.X - startX)) + (Math.Abs(position.Y - startY) * Math.Abs(position.Y - startY)));

            if (curDistance >= targetDistance)
            {
                position.X = targetX;
                position.Y = targetY;
                Explode(gm);
                return false;
            }
            else
            {
                position.X += dx;
                position.Y += dy;
            }

            return true;
        }

        /// <summary>
        /// Creates an explosion around the rocket's coordinates
        /// </summary>
        /// /// <param name="gm">The GameManager to use</param>
        /// <param name="building">The building participating in the collision (if any)</param>
        public void Explode(GameManager gm, Building building = null)
        {
            if (building != null)
            {
                building.Hit(Warhead.Damage);
            }

            if (!(warhead is NuclearWarhead) || (warhead is NuclearWarhead && IsFriendly))
            {
                Point coords;
                if (!IsFriendly)
                {
                    coords = new Point(position.X, position.Y);
                }
                else
                {
                    coords = new Point(position.X, position.Y);
                }

                gm.GenerateExplosion(coords, Warhead.Radius, Warhead.Damage);
            }
        }

        /// <summary>
        /// Simulates taking a hit by an explosion
        /// </summary>
        /// <param name="explosion">The explosion which caused the hit</param>
        /// <returns>Returns true if the rocket has been destroyed</returns>
        public bool GetHit(Explosion explosion)
        {
            if (!IsFriendly)
            {
                isDestroyed = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
