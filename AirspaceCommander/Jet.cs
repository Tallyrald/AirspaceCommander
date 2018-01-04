//-----------------------------------------------------------------------
// <copyright file="Jet.cs" company="Tallyrald">
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
    /// This class represents a Jet fighter
    /// </summary>
    public class Jet : Bindable, IMovable
    {
        /// <summary>
        /// The width of the Jet
        /// </summary>
        public const int W = 70;

        /// <summary>
        /// The height of the Jet
        /// </summary>
        public const int H = 40;

        /// <summary>
        /// Contains the delta speed on the X axis
        /// </summary>
        private double dx = 0;

        /// <summary>
        /// Indicates whether the Jet is destroyed
        /// </summary>
        private bool isDestroyed;

        /// <summary>
        /// Contains the position of the Jet
        /// </summary>
        private Point position;

        /// <summary>
        /// Contains the starting position on the X axis
        /// </summary>
        private double startX;

        /// <summary>
        /// Contains the starting position on the Y axis
        /// </summary>
        private double startY;

        /// <summary>
        /// Contains the target position on the X axis
        /// </summary>
        private double targetX;

        /// <summary>
        /// Contains the target position on the Y axis
        /// </summary>
        private double targetY;

        /// <summary>
        /// Contains the number of rockets the Jet can fire
        /// </summary>
        private int rocketNum;

        /// <summary>
        /// Contains the engine this jet uses
        /// </summary>
        private IEngine engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Airspace_Commander.Jet" /> class.
        /// </summary>
        /// <param name="coords">The starting coordinates</param>
        /// <param name="newTargetX">The target coordinate on the X axis</param>
        /// <param name="newEngine">The engine to use</param>
        public Jet(Point coords, double newTargetX, IEngine newEngine)
        {
            isDestroyed = false;
            position = coords;
            startX = position.X;
            startY = position.Y;
            targetX = newTargetX;
            targetY = position.Y;
            engine = newEngine;
            
            if (position.X < newTargetX)
            {
                dx = engine.Speed * 1.5;
            }
            else
            {
                dx = -1.5 * engine.Speed;
            }

            rocketNum = MainWindow.R.Next(2, 5); 
        }

        /// <summary>
        /// Gets a value indicating whether the Jet is destroyed
        /// </summary>
        public bool IsDestroyed
        {
            get { return isDestroyed; }
        }

        /// <summary>
        /// Gets the position of the Jet
        /// </summary>
        public Point Position
        {
            get { return position; }
        }

        /// <summary>
        /// Gets the starting coordinate on the X axis
        /// </summary>
        public double StartX
        {
            get { return startX; }
        }

        /// <summary>
        /// Gets the starting coordinate on the Y axis
        /// </summary>
        public double StartY
        {
            get { return startY; }
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
        /// Gets the direction (display) of the Jet (0 - left, 180 - right)
        /// </summary>
        public int Angle
        {
            get
            {
                if (dx < 0)
                {
                    return 0;
                }
                else
                {
                    return 180;
                }
            }
        }

        /// <summary>
        /// Moves the Jet
        /// </summary>
        /// <param name="gm">The GameManager to use</param>
        /// <param name="buildings">The parameter is not used.</param>
        /// <returns>Returns false if the Jet is no more, true otherwise</returns>
        public bool Move(GameManager gm, BindingList<Building> buildings = null)
        {
            if (IsDestroyed)
            {
                Explode(gm);
                return false;
            }

            ChangeSpeed();

            double targetDistance = Math.Sqrt((Math.Abs(targetX - startX) * Math.Abs(targetX - startX)) + (Math.Abs(targetY - startY) * Math.Abs(targetY - startY)));
            double curDistance = Math.Sqrt((Math.Abs(position.X - startX) * Math.Abs(position.X - startX)) + (Math.Abs(position.Y - startY) * Math.Abs(position.Y - startY)));

            if (curDistance >= targetDistance)
            {
                return false;
            }
            else
            {
                if (position.X > targetX && position.X + dx < targetX)
                {
                    position.X = targetX;
                }
                else
                {
                    position.X += dx;
                }

                if (rocketNum > 0 && MainWindow.R.Next(0, 900) < 20)
                {
                    gm.GenerateRocket(false, position.X + (W / 3), position.Y + (H / 2));
                    rocketNum--;
                }
            }

            return true;
        }

        /// <summary>
        /// Increases the rocket's delta speed defined by the engine's properties
        /// </summary>
        public void ChangeSpeed()
        {
            if (position.X < targetX)
            {
                dx += engine.SpeedRate / 100;
            }
            else
            {
                dx += (-1 * engine.SpeedRate) / 100;
            }
        }

        /// <summary>
        /// Simulates the explosion of the Jet
        /// </summary>
        /// <param name="gm">The GameManager to use</param>
        /// <param name="buildings">The parameter is not used.</param>
        public void Explode(GameManager gm, Building buildings = null)
        {
            Point explosionCoords = new Point(position.X + (W / 2), position.Y + (H / 2));
            gm.GenerateExplosion(explosionCoords, 50, 1);
            gm.Score();
        }

        /// <summary>
        /// Simulates the Jet taking a hit from an explosion
        /// </summary>
        /// <param name="explosion">The explosion that caused the hit</param>
        /// <returns>Returns false</returns>
        public bool GetHit(Explosion explosion)
        {
            // Always gets killed by a single rocket (would be too OP otherwise)
            isDestroyed = true;

            // We need to prevent immediate removal, so let's return like an unsuccessful hit
            return false;
        }
    }
}
